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
using Newtonsoft.Json;

namespace Partyme.View
{
    public partial class ViewWinks : UserControl
    {
        public ViewWinks()
        {
            InitializeComponent();
        }

        private void ver_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_nickname;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/detalles/DetalleUser.xaml?id=" + user.userID, UriKind.Relative));
        }

        private async void delwink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_nickname;
            Uri url = new Uri(AppResources.link + "delWink");
            objetoslistas.delWink paquete = new objetoslistas.delWink();
            paquete.userFrom = user.userID;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var respuestajson = JsonConvert.DeserializeObject<objetoslistas.errorOut>(respuesta.ToString());
            if (respuestajson.error == "")
            {
                MainPage.notifications[3]--;
                ((MainPage)(((System.Windows.Controls.ContentControl)(App.RootFrame)).Content)).socialnotif();
            }
            else
            {
                MessageBoxResult result =
                MessageBox.Show(respuestajson.error,
                "ERROR",
                MessageBoxButton.OK);
            }
        }

        private void return_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_nickname;
            metodosContextuales.sendToUser(user.userID, "sendWink");
        }
    }
}
