using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMPLib;
using WinForms = System.Windows.Forms;

namespace BetterMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /* Goals -
     * Open music file and listen to it
     * Display metadata (Artist, Song Title, Length... etc)
        - Uses TagLib for metadata
     * Pause / play functionality
     * Volume slider
     * Tray functionality
     */

    public partial class MainWindow : Window
    {

        public string recordLocation;
        public string recordName;
        public string defaultDir;
        //This is recordName, but with / and extension removed
        public string formatted;
        public bool newSong;

        public string musicDirectory;

        public List<string> songs = new List<string>();

        public bool playingMusic;
        public double currentTime;
        public double songDuration;
        public WinForms.FolderBrowserDialog folderBrowse = new WinForms.FolderBrowserDialog();

        System.Timers.Timer myTimer = new System.Timers.Timer(1000);
        WMPLib.WindowsMediaPlayer playerNew = new WMPLib.WindowsMediaPlayer();

        //Timer so music autoplays
        System.Timers.Timer tmr = new System.Timers.Timer();

        //Volume settings
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_UP = 10 * 65536;
        private const int APPCOMMAND_VOLUME_DOWN = 9 * 65536;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public MainWindow()
        {
            InitializeComponent();
            recordLocation = @"C:\Users\Matt\Desktop\MusicFile";
            defaultDir = @"C:\Users\Matt\Desktop\MusicFile";
            recordName = @"\sprung.mp3";
            startupFetch();
            playerNew.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(playerNew_PlayStateChange);
            tmr.Interval = 10;
            tmr.Stop();
            tmr.Elapsed += tmr_Tick;
    }

        void tmr_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("Tick tock party rock");
            tmr.Stop();
            SkipSong();
        }

        void playerNew_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                this.Dispatcher.Invoke(() =>
                {
                    //playerNew.controls.currentPosition = 0;
                    tmr.Start();
                });
            }
        }

        private void VolumeUp()
        {
            // APPCOMMAND_VOLUME_UP or APPCOMMAND_VOLUME_DOWN
            var windowInteropHelper = new WindowInteropHelper(this);
            //SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        //Logic to play / resume music
        public void playMusic()
        {
            //Starts timer to display song time
            myTimer.Elapsed += timeLabel;
            myTimer.Interval = 1000;
            myTimer.Start();
            //This is logic to resume song
            if (currentTime != 0)
            {
                playerNew.URL = recordLocation + recordName;
                playerNew.controls.play();
                playerNew.controls.currentPosition = currentTime;
                playingMusic = true;
                
            }
            // This will start song from beginning
            else
            {
                playerNew.URL = recordLocation + recordName;
                playerNew.controls.play();
                playingMusic = true;
            }
            //Formats the song title, removes backslash and extension
            formatted = recordName;
            formatted = formatted.Substring(0, recordName.Length - 4);
            formatted = (formatted.Remove(0, 1));
            this.Dispatcher.Invoke(() =>
            {
                songTitle.Content = formatted;
                playBtn.Content = "Pause";
            });

            MessageBox.Show(recordLocation + recordName);
            TagLib.File tagLibSong = TagLib.File.Create(recordLocation + recordName);
            var convert = tagLibSong.Properties.Duration;
            songDuration = convert.TotalSeconds;
            if(songDuration == 0 )
            {
                MessageBox.Show("Something is wrong with this audiofile. The file will attempt to play, however issues may arise. Some features will not work. Please fix the file to have it run as intended.");
            }
            MessageBox.Show(songDuration.ToString());

            try
            {
                TagLib.File tagLibFile = TagLib.File.Create(recordLocation + recordName);
                //var songPic = tagLibFile.Tag.Pictures;

                TagLib.IPicture pic = tagLibFile.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);

                // ImageSource for System.Windows.Controls.Image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();

                // Create a System.Windows.Controls.Image control
                musicImg.Source = bitmap;
            }
            catch
            {
                if (newSong == true && newSong == false)
                {
                    #region rng
                    string pic;
                    Random rnd = new Random();
                    int rng = rnd.Next(1, 5);
                    if (rng == 1)
                    {
                        pic = "music.jpg";
                    }
                    else if (rng == 2)
                    {
                        pic = "phone.jpg";
                    }
                    else if (rng == 3)
                    {
                        pic = "record.jpg";
                    }
                    else if (rng == 4)
                    {
                        pic = "records.jpg";
                    }
                    else if (rng == 5)
                    {
                        pic = "cassette.jpg";
                    }
                    else
                    {
                        pic = "cassette.jpg";
                    }
                    #endregion
                    //set img to random image
                    MessageBox.Show("setting image");
                    string uriPre = ("pack://application:,,,/assetts/");
                    var uriCombined = new Uri(uriPre + pic);
                    this.Dispatcher.Invoke(() =>
                    {
                        musicImg.Source = new BitmapImage(uriCombined);
                    });
                }
            }
            newSong = false;
        }

        //Pauses song, and records time music was stopped at
        public void pauseMusic()
        {
            currentTime = playerNew.controls.currentPosition;
            playerNew.controls.pause();
            playingMusic = false;
            playBtn.Content = "Play";
        }

        public void Backwards()
        {
            //Previous song
            if(currentTime <= 5)
            {
                try
                {
                    var formattedIndexName = (recordName.Remove(0, 1));
                    int index = songs.FindIndex(str => str.Contains(formattedIndexName));
                    recordName = @"\" + songs[index - 1];
                    playerNew.controls.pause();
                    currentTime = 0;
                    newSong = true;
                    playMusic();
                }
                catch
                {
                    playerNew.controls.currentPosition = 0;
                }
            }
            //Rewind
            else
            {
                playerNew.controls.currentPosition = 0;
            }
        }

        public void SkipSong()
        {
            //Try to play next song, if there is no next song
            //Goes to song 0 in list
            try
            {
                var formattedIndexName = (recordName.Remove(0, 1));
                int index = songs.FindIndex(str => str.Contains(formattedIndexName)); 
                recordName = @"\" + songs[index + 1];
                playerNew.controls.pause();
                currentTime = 0;
                MessageBox.Show("Name: " + recordName);
                newSong = true;
                playMusic();
            }
            //Reverts back to first song in list
            catch
            {
                recordName = @"\" + songs[0];
                playerNew.controls.pause();
                currentTime = 0;
                MessageBox.Show("Name: " + recordName + " first song");
                newSong = true;
                playMusic();
            }
        }

        //This is the play / pause button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Not playing music
            if(playingMusic == false)
            {
                playMusic();
            }
            //If your playing music, will pause it
            else
            {
                pauseMusic();
            }
        }
        private void timeLabel(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                currentTime = playerNew.controls.currentPosition;
                var currentTimeTrim = currentTime.ToString("0.0");
                timeCounter.Content = currentTimeTrim + "/" + songDuration;
                progressSlider.Maximum = songDuration;
                progressSlider.Value = currentTime;
            });
        }

        private void fileDialog_Click(object sender, RoutedEventArgs e)
        {
            folderBrowse.ShowDialog();
            musicDirectory = folderBrowse.SelectedPath;
            fetchSongs();
        }

            //Finds all valid songs in a chosen directory
            public void fetchSongs()
            {
                DirectoryInfo d = new DirectoryInfo(musicDirectory);
                FileInfo[] Files = d.GetFiles("*.mp3");
                MessageBox.Show(musicDirectory);
                foreach(FileInfo file in Files)
                {
                MessageBox.Show(file.ToString());
                songs.Add(file.ToString());
                }
            songMenu.ItemsSource = songs;
            }

        //Finds all valid songs in a chosen directory
        public void startupFetch()
        {
            DirectoryInfo d = new DirectoryInfo(defaultDir);
            FileInfo[] Files = d.GetFiles("*.mp3");
            foreach (FileInfo file in Files)
            {
                songs.Add(file.ToString());
            }
            songMenu.ItemsSource = songs;
            try
            {
                recordName = @"\" + songs[0];
            }
            catch
            {
                Console.WriteLine("No valid songs found.");
            }
        }

        // Clicked Backwards btn
        private void backClick(object sender, RoutedEventArgs e)
        {
            Backwards();
        }

        // Clicked skip btn
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SkipSong();
        }

        private void selectSong(object sender, RoutedEventArgs e)
        {
            try
            {
                playerNew.controls.pause();
                string selected = songMenu.SelectedValue.ToString();
                recordName = @"\" + selected;
                currentTime = 0;
                playMusic();
            }
            catch
            {
                MessageBox.Show("No song selected");
            }
        }

        private void SkipSong(object sender, RoutedEventArgs e)
        {
            SkipSong();
        }

        private void songMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                playerNew.controls.pause();
                string selected = songMenu.SelectedValue.ToString();
                recordName = @"\" + selected;
                currentTime = 0;
                playMusic();
            }
            catch
            {
                MessageBox.Show("No song selected");
            }
        }

        private void SongMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
