using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Partyme.Resources;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Partyme
{
    class metodosContextuales
    {
        public async static void sendToUser(int ID,string type)
        {
            if ((IsolatedStorageSettings.ApplicationSettings.Contains("isValidMail")) && (((bool)IsolatedStorageSettings.ApplicationSettings["isValidMail"]) == true))
            {
                objetoslistas.sendToUser paquete = new objetoslistas.sendToUser();
                Uri url = new Uri(Resources.AppResources.link + type);
                paquete.userTo = ID;
                string paqueteprueba = JsonConvert.SerializeObject(paquete);
                string respuesta = await metodosJson.jsonPOST(url, paquete);
                var respuestajson = JsonConvert.DeserializeObject<objetoslistas.errorOut>(respuesta.ToString());
                if (respuestajson.error != "")
                {
                    MessageBoxResult result =
                    MessageBox.Show(respuestajson.error,
                    "ERROR",
                    MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBoxResult result =
                MessageBox.Show(AppResources.errornovalid,
                AppResources.error,
                MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml?item=4", UriKind.RelativeOrAbsolute));
                }
            }
        }

    }
}
