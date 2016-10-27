using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Partyme.Resources;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace Partyme
{
    public partial class intro : PhoneApplicationPage
    {
        static bool isMen;
        public intro()
        {
            InitializeComponent();
            checkconexion();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //lanza al cargar la pagina, si tiene exito navega a Main sin que el usuario lo note      
            if (IsolatedStorageSettings.ApplicationSettings.Contains("userID"))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                //si no, combrueba la localizacion y comprueba el deviceid
                permisoloc();
                checkdevice();
            }
           
        }
        //comprueba si la id del dispositivo esta registrada en la bd
        public async void checkdevice()
        {
            objetoslistas.checkdeviceidinput paquete = new objetoslistas.checkdeviceidinput();
            Uri url = new Uri("url");
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var respuestajson = JsonConvert.DeserializeObject<objetoslistas.checkdeviceidoutput>(respuesta.ToString());
            if (respuestajson.perfil == null)
            {
                //la id no esta registrada, asi que se queda a la espera de que el usuario elija sexo y pulse al boton
            }
            else
            {
                //la id esta registrada, almacena el userid y marca en la app que el user no es user nuevo

                var perfil = IsolatedStorageSettings.ApplicationSettings;
                perfil.Add("perfil", respuestajson.perfil);
                IsolatedStorageSettings.ApplicationSettings["usernew"] = false;
                IsolatedStorageSettings.ApplicationSettings.Save();
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }
        //manda una peticion para recibir una id de usuario al pulsar el boton, si se ha seleccionado sexo
        private async void create_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Uri url = new Uri("url");
                objetoslistas.createprofileinput paquete = new objetoslistas.createprofileinput();
                paquete.sexo = isMen;
               
                string respuesta = await metodosJson.jsonPOST(url, paquete);
                var respuestajson = JsonConvert.DeserializeObject<objetoslistas.createprofileoutput>(respuesta.ToString());
                if (respuestajson.ERROR == "")
                {
                    objetoslistas.Profile perfilprovisional = new objetoslistas.Profile();
                    IsolatedStorageSettings.ApplicationSettings["userID"] = respuestajson.userID;
                    IsolatedStorageSettings.ApplicationSettings["usernew"] = true;
                    IsolatedStorageSettings.ApplicationSettings["timestamp"] = respuestajson.timestamp;
                    IsolatedStorageSettings.ApplicationSettings.Save();
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBoxResult result =
                    MessageBox.Show(respuestajson.ERROR,
                    "ERROR",
                    MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.nosexo,
                    AppResources.nosexocab,
                    MessageBoxButton.OK);
            }
        }
        //Comprueba que el usuario haya dado el consentimiento de la localizacion
        public void permisoloc()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                // El usuario ya decidio si queria usar la localizacion
                return;
            }
            else
            {
                //el usuario no dio el consentimiento, lo pide y guarda el resultado.
                MessageBoxResult result =
                    MessageBox.Show(AppResources.locconsent,
                    AppResources.loccab,
                    MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
        //Comprueba que el telefono tiene conexion a internet, sino avisa y se cierra
        public static void checkconexion()
        {
            bool conexion =  Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (conexion == false)
            {
                MessageBox.Show(AppResources.conexion,
                    AppResources.conexioncab,
                    MessageBoxButton.OK);
                Application.Current.Terminate();
            }
        }
        //metodos de los botones de sexo
        private void men_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isMen = true;
            men.Background = new SolidColorBrush(Colors.Gray);
            women.Background = new SolidColorBrush(Colors.Transparent);
        }
        private void women_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isMen = false;
            men.Background = new SolidColorBrush(Colors.Transparent);
            women.Background = new SolidColorBrush(Colors.Gray);
        }
    }
}