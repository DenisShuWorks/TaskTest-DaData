using Dadata;
using Dadata.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string token = "ecfab0da2fee88084afcc433d6addea11d225dd1";
        private string secret = "57c18714b5555b39156c7d87ddb8c6cafc719a2c";

        public MainWindow()
        {
            InitializeComponent();
        }

        public async Task<SuggestResponse<Address>> GetResultsAsync(string txt)
        {
            return await new SuggestClientAsync(token).SuggestAddress(txt);
        }

        private async void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            var result = await GetResultsAsync(comboBoxAddress.Text);
            ObservableCollection<string> list = new ObservableCollection<string>();
            foreach (var item in result.suggestions)
            {
                list.Add(item.value);
            }

            comboBoxAddress.ItemsSource = list;
            comboBoxAddress.IsDropDownOpen = true;
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboBoxAddress.SelectedItem is null)
            {
                Application.Current.MainWindow.Height = 200;
            }
            else
            {
                comboBoxAddress.IsDropDownOpen = false;
                var query = comboBoxAddress.SelectedItem.ToString();
                Application.Current.MainWindow.Height = 800;

                var api = new CleanClientAsync(token, secret);
                var result = await api.Clean<Address>(query);
                var flat = "Нет";
                var floor = "Нет";
                var block = "Нет";
                if (result.flat != null)
                {
                    flat = result.flat;
                }
                if (result.block != null)
                {
                    block = result.block;
                }
                if (result.floor != null)
                {
                    floor = result.floor;
                }
                t1.Text = result.city;
                t2.Text = result.city_fias_id;
                t3.Text = result.street;
                t4.Text = result.street_fias_id;
                t5.Text = result.house;
                t6.Text = block;
                t7.Text = result.house_fias_id;
                t8.Text = result.geo_lat + ", " + result.geo_lon;
                t9.Text = floor;
                t10.Text = flat;
            }


        }

    }
}
