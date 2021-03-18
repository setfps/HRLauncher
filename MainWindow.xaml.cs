using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Steamworks;
using Newtonsoft.Json.Linq;

namespace HRLauncher
{
    public partial class MainWindow : Window
    {
        private static Mutex MutexInstance;
        public MainWindow()
        {
            MutexInstance = new Mutex(true, "HRustLauncher");
            if (!MutexInstance.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("Лаунчер уже запущен!");
                Environment.Exit(105);
            }
            try
            {
                SteamClient.Init(480);
            }
            catch
            {
                MessageBox.Show("LauncherError \nВозможное решение:\n1) Перезапуск Steam\n2) Правильное разархивирование лаунчера.");
                Environment.Exit(403);
            }
            if (File.Exists("RustClient.exe") || (File.Exists("Zero.exe") || (File.Exists("Radiant.exe"))))
            {       
                InitializeComponent();
                WebClient webClient = new WebClient();
                this.ProgressBar_Text.Visibility = Visibility.Hidden;
                this.ProgressBar_sam.Visibility = Visibility.Hidden;

                this.TextName.FontSize = 11.5f;
                this.TextName.FontWeight = FontWeights.Bold;
                this.TextName.Text = "zalupin";

                
                var response = webClient.DownloadString("https://ehrproverka.000webhostapp.com/info.php");
                JObject json = JObject.Parse(response);
                string online = (string)json["players"];
                this.Text_online.VerticalAlignment = VerticalAlignment.Bottom;
                this.Text_online.FontWeight = FontWeights.Bold;
                this.Text_online.FontSize = 13f;
                this.Text_online.Text =  $"{online}";
                GetWall(1);
                GetWall2(2);
                GetWall3(3);
            }
            //else
            {
                MessageBox.Show("Нет игры! Установите игру, после запускайте лаунчер!");
                Environment.Exit(1337);
            }
            Closing += OnWindowCloes;
            border1.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);

        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        int MaxOffset = 2;
        public void GetWall(int news)
        {
            WebRequest request = WebRequest.Create($"https://api.vk.com/method/wall.get?owner_id=-141337482&extended=0&v=5.92&offset={news}&count=1&access_token=tuttoken");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JToken jToken = JObject.Parse(responseFromServer);
                MaxOffset = int.Parse(jToken["response"]["count"].ToString());
                string text = (string)jToken["response"]["items"].ElementAt(0)["text"];
                this.FirstNews_Text.Text = text;
                this.TimeOnFirstNews.Text = UnixTimeStampToDateTime(long.Parse(jToken["response"]["items"].ElementAt(0)["date"].ToString())).ToString("F");
            }      
        }
        public void GetWall2(int news)
        {
            WebRequest request = WebRequest.Create($"https://api.vk.com/method/wall.get?owner_id=-141337482&extended=0&v=5.92&offset={news}&count=1&access_token=tuttoken");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JToken jToken = JObject.Parse(responseFromServer);
                MaxOffset = int.Parse(jToken["response"]["count"].ToString());
                string text = (string)jToken["response"]["items"].ElementAt(0)["text"];
                this.TextOnSecondNews.Text = text;
                this.TimeOnSecondNews.Text = UnixTimeStampToDateTime(long.Parse(jToken["response"]["items"].ElementAt(0)["date"].ToString())).ToString("F");
            }
        }
        public void GetWall3(int news)
        {
            WebRequest request = WebRequest.Create($"https://api.vk.com/method/wall.get?owner_id=-141337482&extended=0&v=5.92&offset={news}&count=1&access_token=1932aa6e5fb020d835c33a8dff61a230116729b35bbd2c8d760a49a6197b069f25c903fb93c3b93793223");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JToken jToken = JObject.Parse(responseFromServer);
                MaxOffset = int.Parse(jToken["response"]["count"].ToString());
                string text = (string)jToken["response"]["items"].ElementAt(0)["text"];
                this.Text_NewsThree.Text = text;
                this.TimeTextThree.Text = UnixTimeStampToDateTime(long.Parse(jToken["response"]["items"].ElementAt(0)["date"].ToString())).ToString("F");
            }
        }
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void OnWindowCloes(object sender, CancelEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(15);
                Opacity = Opacity - 0.1;
            }
        }

        WebClient web = new WebClient();
        private void downLoadFiles()
        {
            String[] fileList = web.DownloadString("http://halloweenproject.cf/api.php").Split('|');
            
            Dispatcher.BeginInvoke(new Action(delegate
            {
                ProgressBar_sam.Minimum = 0;
                ProgressBar_sam.Value = 0;
                ProgressBar_sam.Maximum = fileList.Length;
            }));
            foreach (String file in fileList)

            {
                if (!File.Exists(file))
                {
                    try
                    {
                        String[] dir = file.Split('/');
                        int ct = dir.Length;
                        ct--;
                        dir[ct] = null;
                        String it = null;
                        foreach (String item in dir)
                        {
                            it = it + item + "/";
                        }
                        if (!Directory.Exists(it))
                        {
                            Directory.CreateDirectory(it);
                        }

                        web.DownloadFile("http://halloweenproject.cf/" + file, file);
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message, "Ошибка загрузки файла: " + file, (MessageBoxButton)MessageBoxButton.OK, (MessageBoxImage)MessageBoxImage.Error);
                    }
                    Dispatcher.BeginInvoke(new Action(delegate
                    {

                        ProgressBar_Text.Text = "Загружаю файл: " + file;
                        ProgressBar_sam.Value++;
                    }));

                }
            }
            Thread thread = new Thread(checkFiles); thread.Start();
        }
        private void checkFiles()
        {
            string guchi = Directory.GetCurrentDirectory();
            String[] fileList = Directory.GetFiles("HRustx199/workshop", "");
            CHECK(fileList);

            /*String[] fileList1 = Directory.GetFiles("HRustx199/HRust_Data/Managed", "*.*");
            CHECK(fileList1);*/


        }
        
        private void CHECK(String[] fileList)
        {
            try
            {
                WebClient web = new WebClient();
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    ProgressBar_sam.Minimum = 0;
                    ProgressBar_sam.Value = 0;
                    ProgressBar_sam.Maximum = fileList.Length;
                }));
                foreach (String file in fileList)
                {
                    String[] fileDirs = file.Split('/');
                    int ct = fileDirs.Length;
                    ct--;
                    String path = null;
                    for (int i = 0; i < ct; i++)
                    {
                        path += fileDirs[i] + "/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    if (!File.Exists(file))
                    {
                        Dispatcher.BeginInvoke(new Action(delegate
                        {
                            ProgressBar_Text.Text = "Загружается файл: " + file;
                        }));
                        web.DownloadFile("http://halloweenproject.cf/" + file, file);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action(delegate
                        {
                            ProgressBar_Text.Text = "Идет проверка файлов,пожалуйста подождите";
                        }));
                        String len1 = web.DownloadString("http://halloweenproject.cf/api.php?get=gt&path=" + file);
                        String len2 = File.ReadAllText(file).Length.ToString();

                        if (len1 != len2)
                        {
                            File.Delete(file);
                            web.DownloadFile("http://halloweenproject.cf/" + file, file);
                        }
                    }
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        ProgressBar_sam.Value++;

                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://vk.com/topic-195672144_47196368");
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://exlusiveshop.gamestores.ru");
        }

        private void Button_Click_steamplayer(object sender, RoutedEventArgs e)
        {
            string playerid = SteamClient.SteamId.ToString();
            Process.Start("https://steamcommunity.com/profiles/{0}" + playerid);
        }

        private void Button_Click_playgame(object sender, RoutedEventArgs e)
        {
            this.ProgressBar_Text.Visibility = Visibility.Visible;
            this.ProgressBar_sam.Visibility = Visibility.Visible;
            Thread thread = new Thread(downLoadFiles); thread.Start();
            SetterBooster booster = new SetterBooster();
            booster.Show();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {        
            Environment.Exit(0);
        }

        private void Button_Click_playrprofile(object sender, RoutedEventArgs e)
        {
            string playerid = SteamClient.SteamId.ToString();
            Process.Start($"https://steamcommunity.com/profiles/{playerid}");
        }
    }
}
