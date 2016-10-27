using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace Partyme
{
    public partial class DetalleLocal : PhoneApplicationPage
    {
        int ID;
        public DetalleLocal()
        {
            InitializeComponent();
            MessageBox.Show(ID.ToString(),
                    "ID",
                    MessageBoxButton.OK);
                        
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string idstring;
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("id", out idstring);
            ID = Int32.Parse(idstring);
        }
        public void getlocal()
        {
           
            //ContentPanel.DataContext = listajson.place;

            //listaimg.ItemsSource = listajson.place.placeImages;
           
        }
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["localAppBar1"]);
                    break;
                case 1:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["localAppBar2"]);
                    break;
                case 2:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["localAppBar3"]);

                    break;
            }
        }
    }
}