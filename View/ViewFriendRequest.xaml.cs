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
    public partial class ViewFriendRequest : UserControl
    {
        public ViewFriendRequest()
        {
            InitializeComponent();
        }

        private void aceptar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var friend = element.DataContext as VModel.user_nickname;
            doRequest(true, friend.userID);
            ((MainPage)(((System.Windows.Controls.ContentControl)(App.RootFrame)).Content)).pideamigos();
        }
        private void rechazar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var friend = element.DataContext as VModel.user_nickname;
            doRequest(false, friend.userID);
        }
        private async void doRequest(bool state, int friendID)
        {
            Uri url = new Uri(AppResources.link + "confirmFriendshipRequest");
            objetoslistas.confirmFriendshipRequest paquete = new objetoslistas.confirmFriendshipRequest();
            paquete.friendUserID = friendID;
            paquete.confirm = state;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var respuestajson = JsonConvert.DeserializeObject<objetoslistas.errorOut>(respuesta.ToString());
            if (respuestajson.error == "")
            {
                MainPage.notifications[0]--;
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

        private void view_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var friend = element.DataContext as VModel.user_nickname;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/detalles/DetalleUser.xaml?id=" + friend.userID, UriKind.Relative));
        }
    }
}
