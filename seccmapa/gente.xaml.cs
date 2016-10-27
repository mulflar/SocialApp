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

namespace Partyme.seccmapa
{
    public partial class gente : PhoneApplicationPage
    {
        static List<objetoslistas.Usuario> amigos = new List<objetoslistas.Usuario>();
        System.Collections.Generic.List<objetoslistas.Tipousuario> tipoUsuario = new System.Collections.Generic.List<objetoslistas.Tipousuario>();
        public gente()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            usuarios(MainPage.centrox, MainPage.centroy);
        }
        //Los dos metodos siguientes gestionan la barra de progreso
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }
        private static void SetProgressindicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }
        //pide y vincula los datos de usuarios cercanos
        public async void usuarios(double latitude, double longitude)
        {
            SetProgressindicator(true);
            SystemTray.ProgressIndicator.Text = "Buscando usuarios cercanos";
            Uri url = new Uri("http://www.exox.es/partyme/nearusers");
            objetoslistas.nearusersinput paquete = new objetoslistas.nearusersinput();
            paquete.latitude = latitude;
            paquete.longitude = longitude;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var listajson = JsonConvert.DeserializeObject<objetoslistas.nearusersoutput>(respuesta.ToString());
            SystemTray.ProgressIndicator.Text = "Usuarios encontrados";
            foreach (objetoslistas.user item in listajson.usuarios)
            {
                objetoslistas.Tipousuario amigos = new objetoslistas.Tipousuario("amigos");
                objetoslistas.Tipousuario noamigos = new objetoslistas.Tipousuario("Gente Cercana");
                objetoslistas.Usuario amigo = new objetoslistas.Usuario(item.userID, item.name, item.statusMessage, item.birthDate, item.isMen,
                                        item.numImages, item.profileImage, item.images, item.latitude, item.longitude, item.isFriend, item.parties);
                if (amigo.isFriend == true)
                {
                    amigos.AddUserItem(amigo);
                }
                else
                {
                    noamigos.AddUserItem(amigo);
                }
            }
            listagente.ItemsSource = tipoUsuario;
            SetProgressindicator(false);
        }
       
    }
}