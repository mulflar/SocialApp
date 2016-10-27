using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Partyme.View
{
    public partial class ViewMailnotif : UserControl
    {
        public ViewMailnotif()
        {
            InitializeComponent();
        }

        private void ver_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var user = element.DataContext as VModel.user_mail;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/detalles/DetalleUser.xaml?id=" + user.userID, UriKind.Relative));
        }
    }
}
