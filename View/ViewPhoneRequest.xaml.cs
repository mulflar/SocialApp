using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Partyme.Resources;
using Newtonsoft.Json;

namespace Partyme.View
{
    public partial class ViewPhoneRequest : UserControl
    {
        SaveContactTask saveContactTask;
        public ViewPhoneRequest()
        {
            InitializeComponent();
            saveContactTask = new SaveContactTask();
            saveContactTask.Completed += new EventHandler<SaveContactResult>(saveContactTask_Completed);
        }

        private void aceptar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_phone;

            saveContactTask.FirstName = user.nickName;
            saveContactTask.Notes = "Añadido desde PartyME";
            saveContactTask.PersonalEmail = user.phone;
            saveContactTask.Show();
        }
        void saveContactTask_Completed(object sender, SaveContactResult e)
        {
            switch (e.TaskResult)
            {
                //Logic for when the contact was saved successfully
                case TaskResult.OK:
                    MessageBox.Show("Teléfono guardado");
                    break;

                //Logic for when the task was cancelled by the user
                case TaskResult.Cancel:
                    MessageBox.Show("Operacion cancelada");
                    break;

                //Logic for when the contact could not be saved
                case TaskResult.None:
                    MessageBox.Show("No ha sido posible guardar el teléfono");
                    break;
            }
        }

        private async void rechazar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_phone;
            Uri url = new Uri(AppResources.link + "delPhone");
            objetoslistas.delWink paquete = new objetoslistas.delWink();
            paquete.userFrom = user.userID;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var respuestajson = JsonConvert.DeserializeObject<objetoslistas.errorOut>(respuesta.ToString());
            if (respuestajson.error == "")
            {
                MainPage.notifications[2]--;
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
            var user = element.DataContext as VModel.user_phone;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/detalles/DetalleUser.xaml?id=" + user.userID, UriKind.Relative));
        }
    }
}