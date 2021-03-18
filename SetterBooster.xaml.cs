using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HRLauncher
{
    public partial class SetterBooster : Window
    {
        public SetterBooster()
        {
            InitializeComponent();
           
        }
        private void booster_Checked(object sender, RoutedEventArgs e)
        {
            BezBooster.Visibility = Visibility.Collapsed;
        }
        private void booster_Unchecked(object sender, RoutedEventArgs e)
        {
            BezBooster.Visibility = Visibility.Visible;
        }

        private void Staaaart(object sender, RoutedEventArgs e)
        {
            try
                //full govnocode
            {
                if (File.Exists("AntiCheat.exe"))
                {
                    ProcessStartInfo startInfo1 = new ProcessStartInfo();
                    startInfo1.WorkingDirectory = Directory.GetCurrentDirectory();
                    startInfo1.FileName = "AntiCheat.exe";
                    Process.Start(startInfo1);
                }          
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

            controllProcess();
        }
        private void controllProcess()
        {
            if (File.Exists("RustClient.exe"))
            {
                ProcessStartInfo startInfo1 = new ProcessStartInfo();
                if (Booster.IsChecked == true)
                {
                    startInfo1.Arguments = "-zeroboost";
                }
                startInfo1.Arguments = "-show-screen-selector";
                startInfo1.WorkingDirectory = Directory.GetCurrentDirectory();
                startInfo1.FileName = "RustClient.exe";
                Process process1 = Process.Start(startInfo1);
                int countHandle1 = process1.HandleCount;
                int countThread1 = process1.Threads.Count;
                //ProgressBar_Text.Text = "Проверка файлов завершена,приятной игры.";
                while (true)
                {
                    if (countHandle1 != process1.HandleCount)
                    {
                        process1.Kill();
                    }
                    else if (countThread1 != process1.Threads.Count)
                    {
                        process1.Kill();
                    }
                }
            }


        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {

        }

        private void bezbooster_Checker(object sender, RoutedEventArgs e)
        {
            Booster.Visibility = Visibility.Collapsed;
        }
        private void bezbooster_Unchecked(object sender, RoutedEventArgs e)
        {
            Booster.Visibility = Visibility.Visible;
        }
    }
}
