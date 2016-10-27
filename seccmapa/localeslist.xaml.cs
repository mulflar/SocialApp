using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Partyme.seccmapa
{
    public partial class localeslist : PhoneApplicationPage
    {
        public static List<objetoslistas.Pincho> listapinchos = new List<objetoslistas.Pincho>();
        public localeslist()
        {
            InitializeComponent();
            Loaded += localeslist_Loaded;
            localespincho(MainPage.centrox,MainPage.centroy);
            
            
        }
        //Los dos metodos siguientes gestionan la barra de progreso
        void localeslist_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }
        private static void SetProgressindicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }
        public  async void localespincho(double latitude, double longitude)
        {
            SetProgressindicator(true);
            SystemTray.ProgressIndicator.Text = "Solicitando lista de locales";
            Uri url = new Uri("http://www.exos.es/partyme/getgeograficplaces");
            objetoslistas.getgeograficplacesinput paquete = new objetoslistas.getgeograficplacesinput();
            paquete.latitude = latitude;
            paquete.longitude = longitude;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var listajson = JsonConvert.DeserializeObject<objetoslistas.getgeograficplacesoutput>(respuesta.ToString());
            vistalistalocales.ItemsSource = listajson.pinchos;
            SetProgressindicator(false);
        }
		private async void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// If selected index is -1 (no selection) do nothing
			if (vistalistalocales.SelectedIndex == -1)
				return;
		
			// Navigate to the new page
            objetoslistas.Pincho data = (sender as ListBox).SelectedItem as objetoslistas.Pincho;
            Uri url = new Uri("http://www.exos.es/partyme/getplaces");
            objetoslistas.getPlacesinput paquete = new objetoslistas.getPlacesinput();
            paquete.placeID = data.placeId;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var listajson = JsonConvert.DeserializeObject<objetoslistas.getPlacesoutput>(respuesta.ToString());
            //objetoslistas.DetalleLocal detalle = new objetoslistas.DetalleLocal(data.id, data.latitude, data.longitude, data.name, listajson.place.numberOfMen, listajson.place.numberOfWomen, data.type, data.logo_image, data.subtype, 
            //                                                                    listajson.place.imageURL, data.   


            NavigationService.Navigate(new Uri("/detalles/DetalleLocal.xaml?id=" + data.placeId, UriKind.Relative));
      		// Reset selected index to -1 (no selection)
			vistalistalocales.SelectedIndex = -1;
		}

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //obtiene la ubicacion actual, limpia la lista y la rellena de nuevo
        }
    }
}