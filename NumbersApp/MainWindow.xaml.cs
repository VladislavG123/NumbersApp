using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NumbersApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int mounth;
            int day;
            try
            {
                mounth = calendar.SelectedDate.Value.Month;
                day = calendar.SelectedDate.Value.Day;
            }
            catch (Exception)
            {
                MessageBox.Show("Chouse date");
                return;
            }

            var numberInfo = Task.Run(() =>
            {
                WebRequest request = WebRequest.Create($"https://numbersapi.p.rapidapi.com/{mounth}/{day}/date?fragment=true&json=true");
                request.Headers.Add("X-RapidAPI-Host", "numbersapi.p.rapidapi.com");
                request.Headers.Add("X-RapidAPI-Key", "384d62cd25mshe08e0874f7810d8p18766ajsn8a22f40bc584");

                WebResponse response = request.GetResponse();
                string json = "";
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            json += line;
                        }
                    }
                }
                response.Close();

                var result = JsonConvert.DeserializeObject<NumberInfo>(json);

                return result;
            }).Result;

            textBlock.Text = $"Number - {numberInfo.Number}\nYear - {numberInfo.Year}\n{numberInfo.Text}";
        }
    }
}
