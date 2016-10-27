using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using Partyme.Resources;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Partyme.View
{
    public partial class ViewMailRequest : UserControl
    {
        SaveContactTask saveContactTask;
        public ViewMailRequest()
        {
            InitializeComponent();
            saveContactTask = new SaveContactTask();
            saveContactTask.Completed += new EventHandler<SaveContactResult>(saveContactTask_Completed);         
        }

        private void aceptar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_mail;

            saveContactTask.FirstName = user.nickName;
            saveContactTask.Notes = "Añadido desde PartyME";
            saveContactTask.PersonalEmail = user.mail;
            saveContactTask.Show();
       }
        void saveContactTask_Completed(object sender, SaveContactResult e)
        {
            switch (e.TaskResult)
            {
                //Logic for when the contact was saved successfully
                case TaskResult.OK:
                    MessageBox.Show("Email guardado");
                    break;

                //Logic for when the task was cancelled by the user
                case TaskResult.Cancel:
                    MessageBox.Show("Operacion cancelada");
                    break;

                //Logic for when the contact could not be saved
                case TaskResult.None:
                    MessageBox.Show("No ha sido posible guardar el email");
                    break;
            }
        }

        private async void rechazar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_mail;
            Uri url = new Uri(AppResources.link + "delMail");
            objetoslistas.delWink paquete = new objetoslistas.delWink();
            paquete.userFrom = user.userID;
            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var respuestajson = JsonConvert.DeserializeObject<objetoslistas.errorOut>(respuesta.ToString());
            if (respuestajson.error == "")
            {
                MainPage.notifications[1]--;
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
            var user = element.DataContext as VModel.user_mail;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/detalles/DetalleUser.xaml?id=" + user.userID, UriKind.Relative));
        }
    }
}
