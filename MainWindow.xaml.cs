using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
     * Get avg or most amount of pixels, take that color, darken it, apply to background
     *   ^ somewhat done, just gets one pixel thusfar
     */

    public partial class MainWindow : Window
    {

        public string recordLocation;
        public string recordName;
        public string defaultDir;
        //This is recordName, but with / and extension removed
        public string formatted;
        public bool newSong;

        public BitmapImage albumArt;
        
        //Sound stuff
        public double volume = 100;
        public bool muted = false;

        public string musicDirectory;

        public List<string> songs = new List<string>();

        public bool playingMusic;
        public double currentTime;
        public double songDuration;
        public double songDurMin;
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
            //recordLocation = defaultDir;
            //defaultDir = @"C:\Users\Matt\Desktop\MusicFile";
            recordName = "";
            //startupFetch();
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
            if (currentTime != 0 && newSong != true)
            {
                playerNew.URL = musicDirectory + recordName;
                playerNew.controls.play();
                playerNew.controls.currentPosition = currentTime;
                playingMusic = true;
                newSong = false;
            }
            // This will start song from beginning
            else
            {
                playerNew.URL = musicDirectory + recordName;
                playerNew.controls.play();
                playingMusic = true;
                newSong = true;
            }
            if (newSong == true)
            {
                try
                {
                    //MessageBox.Show(newSong.ToString());
                    //Formats the song title, removes backslash and extension
                    formatted = recordName;
                    formatted = formatted.Substring(0, recordName.Length - 4);
                    formatted = (formatted.Remove(0, 1));
                    this.Dispatcher.Invoke(() =>
                    {
                        songTitle.Content = formatted;
                        playBtn.Content = FindResource("pause");
                    });
                }
                catch
                {
                }

                //MessageBox.Show(recordLocation + recordName);
                try
                {
                    TagLib.File tagLibSong = TagLib.File.Create(musicDirectory + recordName);
                    var convert = tagLibSong.Properties.Duration;
                    songDuration = convert.TotalSeconds;
                    songDurMin = convert.TotalMinutes;
                    songDurMin = Math.Round(songDurMin, 2);
                    if (songDuration == 0)
                    {
                        MessageBox.Show("Something is wrong with this audiofile. The file will attempt to play, however issues may arise. Some features will not work. Please fix the file to have it run as intended.");
                    }
                    //MessageBox.Show(songDuration.ToString());
                }
                catch
                {
                    MessageBox.Show("Directory not found!");
                    //TODO - logic to remove null items from list
                    return;
                }
                try
                {
                    TagLib.File tagLibFile = TagLib.File.Create(musicDirectory + recordName);
                    //var songPic = tagLibFile.Tag.Pictures;

                    ArtistName.Content = tagLibFile.Tag.FirstAlbumArtist;
                    TagLib.IPicture pic = tagLibFile.Tag.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data);
                    ms.Seek(0, SeekOrigin.Begin);

                    // ImageSource for System.Windows.Controls.Image
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    albumArt = bitmap;

                    // Create a System.Windows.Controls.Image control
                    musicImg.Source = bitmap;

                    GetPixel();
                }
                catch
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
                    string uriPre = ("pack://application:,,,/assetts/");
                    var uriCombined = new Uri(uriPre + pic);
                    this.Dispatcher.Invoke(() =>
                    {
                        musicImg.Source = new BitmapImage(uriCombined);
                    });
                }
            }
            this.Dispatcher.Invoke(() =>
            {
                playBtn.Content = FindResource("pause");
            });
            newSong = false;
        }

        //Pauses song, and records time music was stopped at
        public void pauseMusic()
        {
            currentTime = playerNew.controls.currentPosition;
            playerNew.controls.pause();
            playingMusic = false;
            playBtn.Content = FindResource("play");
        }

        public void Backwards()
        {
            //Previous song
            if(currentTime <= 5)
            {
                try
                {
                    newSong = true;
                    var formattedIndexName = (recordName.Remove(0, 1));
                    int index = songs.FindIndex(str => str.Contains(formattedIndexName));
                    recordName = @"\" + songs[index - 1];
                    playerNew.controls.pause();
                    currentTime = 0;
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
            newSong = true;
            //Try to play next song, if there is no next song
            //Goes to song 0 in list
            try
            {
                var formattedIndexName = (recordName.Remove(0, 1));
                int index = songs.FindIndex(str => str.Contains(formattedIndexName)); 
                recordName = @"\" + songs[index + 1];
                playerNew.controls.pause();
                currentTime = 0;
                playMusic();
            }
            //Reverts back to first song in list
            catch
            {
                recordName = @"\" + songs[0];
                playerNew.controls.pause();
                currentTime = 0;
                MessageBox.Show("Name: " + recordName + " first song");
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
                //var convertedToString = currentTimeTrim.ToString();
                //convertedToString = currentTimeTrim.Replace(".", ":");
                var songDurString = songDurMin.ToString();
                var replaced = songDurString.Replace(".", ":");
                timeCounter.Content = currentTimeTrim + "/" + replaced;
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

            //*TODO* Sort Songs by album instead of name
            public void fetchSongs()
            {
            try
            {
                DirectoryInfo d = new DirectoryInfo(musicDirectory);
                FileInfo[] Files = d.GetFiles("*.mp3");
                MessageBox.Show(musicDirectory);
                foreach (FileInfo file in Files)
                {
                    //MessageBox.Show(file.ToString());
                    songs.Add(file.ToString());
                }
            }
            catch
            {
            }
            RefreshSongs();
            songMenu.ItemsSource = songs;
            songMenu.Items.Refresh();
            songMenu.InvalidateArrange();
            songMenu.UpdateLayout();
            }

        public void RefreshSongs()
        {
            //Going backwards ensures all files are removed
            for (int i = songs.Count- 1; i >= 0; i--)
            {
                //Check file location
                var song = songs[i];
                var combined = musicDirectory + @"\" + song;
                //MessageBox.Show(combined);
                FileInfo stream = new FileInfo(combined);
                if(stream.Exists)
                {
                }
                else
                {
                    //Code to remove from list.
                    songs.RemoveAt(i);
                }
            }
        }

        //Finds all valid songs in a chosen directory
        /** public void startupFetch()
         {
             DirectoryInfo d = new DirectoryInfo();
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
         */

        //Converts BitmapImage(WPF) to Bitmap(WinForms)
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            //BitmapImage bitmapImage = new BitmapImage(albumArt);

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        
        //Code to get color of album, and then display it,
        //In background
        public void GetPixel()
        {
            //Colors
            float red;
            float green;
            float blue;

            //This is the converted bitmap(winforms)
            var bitmap = BitmapImage2Bitmap(albumArt);
            //This is RGBA code for background color
            System.Drawing.Color pixelColor = bitmap.GetPixel(50, 50);
            red = pixelColor.R;
            green = pixelColor.G;
            blue = pixelColor.B;

            // >1 lighter, <1 darker
            float correctionFactor = 0.5f;
            red *= correctionFactor;
            green *= correctionFactor;
            blue *= correctionFactor;

            //Ensures RGB values cannot be greater than 255, lower than 0
            #region Color Value Checks
            if (red > 255)
            {
                red = 255;
            }
            else if(red < 0)
            {
                red = 0;
            }
            if (green > 255)
            {
                green = 255;
            }
            else if (green < 0)
            {
                green = 0;
            }
            if (blue > 255)
            {
                blue = 255;
            }
            else if (blue < 0)
            {
                blue = 0;
            }
            #endregion

            //Converter to turn color into brush
            var converter = new System.Windows.Media.BrushConverter();
            //Darkens color so it doesnt blend into album art
            //System.Drawing.Color newColor = (int)(pixelColor.A * 50, pixelColor.G *50);
            var darkened = System.Drawing.Color.FromArgb(pixelColor.A, (int)red, (int)green, (int)blue);
            //Converts RGBA colorcode to html color code
            string htmlColor = ColorTranslator.ToHtml(darkened);
            //Color is then turned into a brush
            var brush = (System.Windows.Media.Brush)converter.ConvertFrom(htmlColor);
            //Finally the background color is set to brush color
            bgGrid.Background = brush;
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
           // try
            //{
                playerNew.controls.pause();
                MessageBox.Show("pausing.");
                string selected = songMenu.SelectedValue.ToString();
                MessageBox.Show(selected);
                recordName = @"\" + selected;
                MessageBox.Show(selected);
                currentTime = 0;
                playMusic();
            //}
            //catch
            //{
             //  MessageBox.Show("No song selected");
           // }
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
                newSong = true;
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

        private void Mute(object sender, RoutedEventArgs e)
        {
            /** if(volume == 0)
             {
                 muted = false;
                 volume = playerNew.settings.volume;
                 setVolImg();
             }
     */

            //This will unmute
            if (muted == true)
            {
                muted = false;
                //Code to unmute sound
                volume = volSlider.Value;
                var volInt = (int)Math.Round(volume);
                playerNew.settings.volume = volInt;
                //Code to switch image
                setVolImg();
            }
            //This will mute
            else if (muted == false)
            {
                muted = true;
                //Code to mute sound
                playerNew.settings.volume = 0;
                //Code to switch image
                setVolImg();
            }
            else if(playerNew.settings.volume != 0)
            {
                muted = false;
                setVolImg();
            }
        }

        private void VolSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Button mute = FindName("mute") as Button;
            volume = volSlider.Value;
            var volInt = (int)Math.Round(volume);
            //MessageBox.Show(volInt.ToString());
            playerNew.settings.volume = volInt;
            setVolImg();
        }

        public void setVolImg()
        {
            try
            {
                Button mute = FindName("mute") as Button;

                //Low volume
                if (playerNew.settings.volume < 50 && playerNew.settings.volume != 0)
                {
                    muted = false;
                    //Change image to volume low
                    mute.Content = FindResource("volumelow");
                    //mute.Content = FindResource("volumelow");
                }
                //High volume
                else if (playerNew.settings.volume >= 50)
                {
                    muted = false;
                    //Change image to volume high
                    mute.Content = FindResource("volumehigh");
                }
                //Muted
                else if (playerNew.settings.volume == 0)
                {
                    muted = true;
                    //Change image to muted
                    mute.Content = FindResource("mute");
                }
                else if (volume == 0)
                {
                    muted = true;
                    //Change image to muted
                    mute.Content = FindResource("mute");
                }
                else if(muted == true)
                {
                    muted = true;
                    //Change image to muted
                    mute.Content = FindResource("mute");
                }
            }
            catch
            {

            }
        }

        private void Better_Music_KeyUp(object sender, KeyEventArgs e)
        {
            #region Play/pause
            if(e.Key == Key.Space)
            {
                if(playingMusic == true)
                {
                    pauseMusic();
                }
                else if(playingMusic == false)
                {
                    playMusic();
                }
                else
                {
                    pauseMusic();
                }
            }
            #endregion
            #region Skip / Reverse
            if (e.Key == Key.Right)
            {
                SkipSong();
            }
            if(e.Key == Key.Left)
            {
                Backwards();
            }
            #endregion
        }
    }
}
