using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
     * Pause / volume buttons
     * Tray functionality
     */

    public partial class MainWindow : Window
    {

        public string recordLocation;
        public string recordName;
        public string defaultDir;


        public string musicDirectory;

        public List<string> songs = new List<string>();

        public bool playingMusic;
        public double currentTime;
        public WinForms.FolderBrowserDialog folderBrowse = new WinForms.FolderBrowserDialog();

        System.Timers.Timer myTimer = new System.Timers.Timer(1000);
        WMPLib.WindowsMediaPlayer playerNew = new WMPLib.WindowsMediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            recordLocation = @"C:\Users\Matt\Desktop\MusicFile";
            defaultDir = @"C:\Users\Matt\Desktop\MusicFile";
            recordName = @"\sprung.mp3";
            startupFetch();
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
            //Formats the song title
            char[] MyChar = { '.', 'w', 'a', 'v', '.', 'm', 'p', '3' };
            var formatted = recordName.TrimEnd(MyChar);
            formatted = (formatted.Remove(0, 1));
            songTitle.Content = formatted;
            playBtn.Content = "Pause";
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
            System.Windows.MessageBox.Show("work");
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
                System.Windows.MessageBox.Show("rewind");
                playerNew.controls.currentPosition = 0;
            }
        }

        public void SkipSong()
        {
            MessageBox.Show("Skipping");
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
                timeCounter.Content = currentTime;
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
    }
}
