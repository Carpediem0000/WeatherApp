using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TelegramBot_Dz;
using static WeatherApp.Form1;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        private List<City> _citys;
        public class City
        {
            public string Name { get; set; }
            public string Region { get; set; }
        }
        public class IpInfo
        {
            public string Ip { get; set; }
            public string Hostname { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public string Loc { get; set; }
            public string Org { get; set; }
            public string Postal { get; set; }
            public string Timezone { get; set; }
        }
        public static List<City> GetUkrainianRegionalCenters()
        {
            return new List<City>
        {
            new City { Name = "Kyiv", Region = "Kyiv" },
            new City { Name = "Vinnytsia", Region = "Vinnytsia Oblast" },
            new City { Name = "Dnipro", Region = "Dnipropetrovsk Oblast" },
            new City { Name = "Donetsk", Region = "Donetsk Oblast" },
            new City { Name = "Zhytomyr", Region = "Zhytomyr Oblast" },
            new City { Name = "Zaporizhzhia", Region = "Zaporizhzhia Oblast" },
            new City { Name = "Ivano-Frankivsk", Region = "Ivano-Frankivsk Oblast" },
            new City { Name = "Kropyvnytskyi", Region = "Kirovohrad Oblast" },
            new City { Name = "Luhansk", Region = "Luhansk Oblast" },
            new City { Name = "Lviv", Region = "Lviv Oblast" },
            new City { Name = "Mykolaiv", Region = "Mykolaiv Oblast" },
            new City { Name = "Odesa", Region = "Odesa Oblast" },
            new City { Name = "Poltava", Region = "Poltava Oblast" },
            new City { Name = "Rivne", Region = "Rivne Oblast" },
            new City { Name = "Sumy", Region = "Sumy Oblast" },
            new City { Name = "Ternopil", Region = "Ternopil Oblast" },
            new City { Name = "Kharkiv", Region = "Kharkiv Oblast" },
            new City { Name = "Kherson", Region = "Kherson Oblast" },
            new City { Name = "Khmelnytskyi", Region = "Khmelnytskyi Oblast" },
            new City { Name = "Cherkasy", Region = "Cherkasy Oblast" },
            new City { Name = "Chernivtsi", Region = "Chernivtsi Oblast" },
            new City { Name = "Chernihiv", Region = "Chernihiv Oblast" }
        };
        }
        public Form1()
        {
            InitializeComponent();
            _citys = GetUkrainianRegionalCenters();
            foreach (var item in _citys)
            {
                cb_sity.Items.Add(item.Name);
            }
            cb_sity.SelectedIndex = 0;
            lbl_CityName.Text = cb_sity.Items[cb_sity.SelectedIndex].ToString();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var httpClient = new HttpClient();
            string pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org"); // Внешний IP

            var response = await httpClient.GetStringAsync($"https://ipinfo.io/{pubIp}/geo");
            //XmlDocument res = JsonConvert.DeserializeXmlNode(response);
            //MessageBox.Show(res.GetElementsByTagName("city_name").Item(0).Value);

            IpInfo ipInfo = null;
            if (response != null)
            {
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(response);
            }
            //MessageBox.Show($"City: {ipInfo.City}");
            for (int i = 0; i < cb_sity.Items.Count; i++)
            {
                if (cb_sity.Items[i].ToString() == ipInfo.City)
                {
                    cb_sity.SelectedIndex = i;
                    lbl_CityName.Text = cb_sity.Items[cb_sity.SelectedIndex].ToString();
                    break;
                }
            }

            LoadWether(cb_sity.Items[cb_sity.SelectedIndex].ToString());
        }

        private async void LoadWether(string city)
        {
            string apiKey = "bcb395514086595b809ab3007e0d819a";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient()) { 
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    WeatherResponse weatherInfo = JsonConvert.DeserializeObject<WeatherResponse>(responseBody);

                    int temp = Convert.ToInt32(weatherInfo.main.temp);
                    lbl_Temp.Text = temp.ToString();
                }
                else
                {
                    MessageBox.Show("Не удалось получить информацию о погоде.");
                }
            }
        }

        private void cb_sity_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_CityName.Text = cb_sity.Items[cb_sity.SelectedIndex].ToString();
            LoadWether(cb_sity.Items[cb_sity.SelectedIndex].ToString());
        }
    }
}
