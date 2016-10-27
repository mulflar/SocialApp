using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Partyme.Resources;
using Windows.Devices.Geolocation;
using System.Device.Location;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Microsoft.Phone.Maps.Toolkit;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.Windows.Media;


namespace Partyme
{
    public partial class MainPage : PhoneApplicationPage
    {
        Geolocator geolocator = new Geolocator();
        bool tracking;
        bool isMen;
        public static double centrox;
        public static double centroy;
        public static string direccion;
        bool changeprofile = false;
        static List<objetoslistas.Pincho> listapinchos = new List<objetoslistas.Pincho>();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            intro.checkconexion();
            centromapa();
            peticionperfil();
            
        }
        //Los dos metodos siguientes gestionan la barra de progreso
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new  ProgressIndicator();
        }
        private static void SetProgressindicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }
        //metodo que muestra una appbar diferente en funcion del elemento pivot visible y lanza los metodos asincronos de cada seccion
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBar1"]);

                    break;
                case 1:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBar2"]);
                    break;
                case 2:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBar3"]);

                    break;
                case 3:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBar4"]);
                    break;
                case 4:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBar5"]);
                    changeprofile = false;
                    
                    //StatusTextBlock.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBar6"]);
                    break;
            }
        }
        //crea una capa en mapa y bindea la lista a pushpins
        private void pinchosmapa()
        {
            SystemTray.ProgressIndicator.Text = "Obteniendo locales y eventos cercanos";
            ObservableCollection<DependencyObject> children = MapExtensions.GetChildren(mapacentral);
            var obj = children.FirstOrDefault(x => x.GetType() == typeof(MapItemsControl)) as MapItemsControl;
            localespincho(mapacentral.Center.Latitude, mapacentral.Center.Longitude);
            obj.ItemsSource = listapinchos;
            listacercanos.DataContext = listapinchos;
            SetProgressindicator(false);
        }
        //pide localizaciones de pinchos, las adapta al objeto pincho y las almacena en una lista
        public async void localespincho(double latitude, double longitude)
        {
            Uri url = new Uri("url");
            objetoslistas.getgeograficplacesinput paquete = new objetoslistas.getgeograficplacesinput();
            paquete.latitude = latitude;
            paquete.longitude = longitude;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var listajson = JsonConvert.DeserializeObject<objetoslistas.getgeograficplacesoutput>(respuesta.ToString());
            foreach (objetoslistas.Pincho item in listajson.pinchos)
            {
                listapinchos.Add(item);
            }
            string ejemplo = checklocal();

        }
        //comprueba si la localizacion actual coincide con la de algun pincho y si es asi te "situa" en el nombre del pincho
        public static string checklocal()
        {
            foreach (objetoslistas.Pincho item in listapinchos)
            {
                if (item.latitude == MainPage.centrox || item.longitude == MainPage.centroy)
                {
                    string sitio = "Estas en " + item.name;
                    return sitio;
                }
                else
                {
                    return direccion;
                }
            }
            return direccion;
        }
        //pide los datos del perfil y los almacena, despues pide que se rellene el pivot perfil
        private async void peticionperfil()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("usernew"))
            {
                if ((bool)IsolatedStorageSettings.ApplicationSettings["usernew"] == false)
                {
                    objetoslistas.getprofileinput paquete = new objetoslistas.getprofileinput();
                    

                    Uri url = new Uri("url");
                    string respuesta = await metodosJson.jsonPOST(url, paquete);
                    var respuestajson = JsonConvert.DeserializeObject<objetoslistas.getprofileoutput>(respuesta.ToString());

                    if (respuestajson.perfil != null)
                    {
                        var perfil = IsolatedStorageSettings.ApplicationSettings;
                        perfil.Add("perfil", respuestajson.perfil);
                        IsolatedStorageSettings.ApplicationSettings["UserData"] = true;
                        IsolatedStorageSettings.ApplicationSettings.Save();
                        rellenoperfil();
                    }
                }
            }
            
        }
        //rellena el pivot del perfil con los datos almacenados
        private void rellenoperfil()
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["UserData"] == true)
            {
                status.Text = (string)IsolatedStorageSettings.ApplicationSettings["statusMessage"];
                if ((bool)IsolatedStorageSettings.ApplicationSettings["mapVisibility"] == true)
                {
                    Visibilidad.IsChecked = true;
                }
                else
                {
                    Visibilidad.IsChecked = false;
                }
                nombre.Text = (string)IsolatedStorageSettings.ApplicationSettings["nickName"];
                if ((bool)IsolatedStorageSettings.ApplicationSettings["isMen"] == true)
                {
                    hombre.Style = (Style)Application.Current.Resources["ButtonStyleselected"];
                    mujer.Style = (Style)Application.Current.Resources["ButtonStylebase"];
                    isMen = true;
                }
                else if ((bool)IsolatedStorageSettings.ApplicationSettings["isMen"] == false)
                {
                    hombre.Style = (Style)Application.Current.Resources["ButtonStylebase"];
                    mujer.Style = (Style)Application.Current.Resources["ButtonStyleselected"];
                    isMen = false;
                }
                Fecha_nacimiento.Value = ((DateTime)IsolatedStorageSettings.ApplicationSettings["birthDate"]);
                email.Text = (string)IsolatedStorageSettings.ApplicationSettings["mail"];
                telefono.Text = (string)IsolatedStorageSettings.ApplicationSettings["Phone"];
            }
        }
        private void track()
        {
            if (!tracking)
            {
                geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                geolocator.MovementThreshold = 50; // Metros
                geolocator.PositionChanged += geolocator_PositionChanged;
                tracking = true;
            }
            else
            {
                geolocator.PositionChanged -= geolocator_PositionChanged;
                geolocator = null;
                tracking = false;
            }
        }
        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        { 
            Dispatcher.BeginInvoke(() => 
            {
                mapacentral.Center = new GeoCoordinate (args.Position.Coordinate.Latitude,args.Position.Coordinate.Longitude);
                centrox = args.Position.Coordinate.Latitude;
                centroy = args.Position.Coordinate.Longitude;
                pinchosmapa();
                ReverseGeocodeQuery query = new ReverseGeocodeQuery();
                query.GeoCoordinate = new GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                query.QueryCompleted += ReverseGeocodeQuery_QueryCompleted;
                query.QueryAsync();
            }); 
        }
        private void ReverseGeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    MapAddress address = e.Result[0].Information.Address;
                    direccion = "Estas en " + address.Street + " " + address.HouseNumber;
                    listacercanos.Header = "Estas en " + address.Street + " " + address.HouseNumber;
                }
            }
            listacercanos.Header = checklocal();
        } 
        private async void centromapa()
        { 
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                SetProgressindicator(true);
                SystemTray.ProgressIndicator.Text = "Obteniendo datos del GPS" ;
                Geoposition geoposition = await geolocator.GetGeopositionAsync
                    (maximumAge: TimeSpan.FromMinutes(5),
                                    timeout: TimeSpan.FromSeconds(10));
                SystemTray.ProgressIndicator.Text = "Localizacion obtenida";
                mapacentral.Center.Latitude = geoposition.Coordinate.Latitude;
                mapacentral.Center.Longitude = geoposition.Coordinate.Longitude;
                centrox = geoposition.Coordinate.Latitude;
                centroy = geoposition.Coordinate.Longitude;
                ReverseGeocodeQuery query = new ReverseGeocodeQuery();
                query.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                query.QueryCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result.Count > 0)
                    {
                        listacercanos.Header = e.Result.ToString();
                    }
                };
                query.QueryAsync();
                track();
                pinchosmapa();
            }
            catch (UnauthorizedAccessException)
            {          // the app does not have the right capability or the location master switch is off 
                listacercanos.Header = "location  is disabled in phone settings.";
            } 
        }
       
        public async void cambioperfil()
        {
            objetoslistas.setprofileinput paquete = new objetoslistas.setprofileinput();
            paquete.nickName = nombre.Text;
            paquete.statusMessage = status.Text;
            paquete.isMen = isMen;
            paquete.birthDate = Fecha_nacimiento.Value.ToString();
            if (Visibilidad.IsChecked == true)
            {
                paquete.mapVisibility = true;
            }
            else
            {
                paquete.mapVisibility = false;
            }
            Uri url = new Uri("url");
            string respuesta = await metodosJson.jsonPOST(url, paquete);

        }
        private void mujer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            hombre.Background = new SolidColorBrush(Colors.Transparent);
            mujer.Background = new SolidColorBrush(Colors.Gray);
            isMen = false;
        }
        private void hombre_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            hombre.Background = new SolidColorBrush(Colors.Gray);
            mujer.Background = new SolidColorBrush(Colors.Transparent);
            isMen = true;
        }
        private void email_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            validaremail.Background = new SolidColorBrush(Colors.Red);
        }
        private void telefono_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            validartlf.Background = new SolidColorBrush(Colors.Red);
        }

        private void fiestas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var item = (sender as Grid).DataContext as //Fiesta; // Given you have City objects in your list
            //NavigationService.Navigate(new Uri("/detalles/Fiesta.xaml?id=" + item.Id, UriKind.Relative);
        }
        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var _ppmodel = sender as Pushpin;
            ContextMenu contextMenu =
                ContextMenuService.GetContextMenu(_ppmodel);
            contextMenu.DataContext = listapinchos.Where
                (c => (c.posicion
                    == _ppmodel.GeoCoordinate)).FirstOrDefault();
            if (contextMenu.Parent == null)
            {
                contextMenu.IsOpen = true;
            }
        }
    }
}