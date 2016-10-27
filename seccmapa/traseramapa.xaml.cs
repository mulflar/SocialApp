using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Partyme
{
    public partial class traseramapa : PhoneApplicationPage
    {
        public traseramapa()
        {
            InitializeComponent();
            this.longListLocales.ItemsSource = this.GetPinchoGroups(); 

        }
        //Solo para places
        //trae la lista de Main y "separa" el premium
        private static IEnumerable<objetoslistas.Reduced_usable> GetPinchoList()
        {

            List<objetoslistas.Reduced_usable> reducedList = MainPage.listapinchosord;
            foreach (objetoslistas.Reduced_usable item in reducedList)
            {
                if (item.placeID == MainPage.premium)
                {
                    item.grupo = "Destacado";
                    item.back = "#FFA89416";
                }
            }
            return reducedList;
        }
        //Solo para places
        //identifica los distintos valores en el atributo grupo
        private List<Group<objetoslistas.Reduced_usable>> GetPinchoGroups()
        {
            IEnumerable<objetoslistas.Reduced_usable> pinchoList = GetPinchoList();
            return GetItemGroups(pinchoList, c => c.grupo);
        }

        //Vale para todos los longlist
        private static List<Group<T>> GetItemGroups<T>(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
        {
            IEnumerable<Group<T>> groupList = from item in itemList
                                              group item by getKeyFunc(item) into g
                                              orderby g.Key
                                              select new Group<T>(g.Key, g);

            return groupList.ToList();
        }
        public class Group<T> : List<T>
        {
            public Group(string name, IEnumerable<T> items)
                : base(items)
            {
                this.Title = name;
            }

            public string Title
            {
                get;
                set;
            }
        }

        private void longListLocales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // si la seleccion es null (no hay nada elegido) no hace nada
            if (longListLocales.SelectedItem == null)
                return;

            // Navega a una pagina con id = placeID
            objetoslistas.Reduced_usable data = (sender as LongListSelector).SelectedItem as objetoslistas.Reduced_usable;
            NavigationService.Navigate(new Uri("/detalles/DetalleLocal.xaml?id=" + data.placeID, UriKind.Relative));

            // Vuelve la seleccion a -1 (no selection)
            longListLocales.SelectedItem = null;
        }


        //cambio de barra al cambio de pivot
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["traseraAppBar1"]);
                    break;
                case 1:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["traseraAppBar2"]);
                    break;
                case 2:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["traseraAppBar3"]);
                    break;
                case 3:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["traseraAppBar4"]);
                    break;
            }
        }
    }
}