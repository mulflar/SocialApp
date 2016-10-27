using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Partyme.ViewModels;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

namespace Partyme.servicios
{
    public class CatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string title = "";
            foreach (objetoslistas.categories item in carta.categorias)
            {
                if (item.categoryID.ToString() == (String)value)
                {
                    title = item.name;
                }
            }
            return title;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string source = "";
            foreach (objetoslistas.categories item in carta.categorias)
            {
                if (item.categoryID.ToString() == (String)value)
                {
                    source = item.image;
                }
            }
            return source;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AplyDiscount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double finalprice=(double)value;
            int[]vip = new int[1];
            if ((IsolatedStorageSettings.ApplicationSettings.Contains("vipPlaces")) && ((int[])IsolatedStorageSettings.ApplicationSettings["vipPlaces"] != null))
                vip = (int[])IsolatedStorageSettings.ApplicationSettings["vipPlaces"];
            foreach (int item in vip)
            {
                if (item.ToString() == carta.idstring)
                    finalprice = (double)value - ((double)value*carta.discount);
            }
            return finalprice;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class carta : PhoneApplicationPage
    {
        ViewModelCarta vcarta;
        public static ObservableCollection<objetoslistas.categories> categorias;
        public static List<objetoslistas.products> pedido;
        public double preciototal;
        public static string idstring;
        public static double discount;
        public carta()
        {
            InitializeComponent();
            vcarta = new ViewModelCarta();
            pedido = new List<objetoslistas.products>();
            pedidolist.DataContext = pedido;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("id", out idstring);
            vcarta.GetCarta(idstring);
            cartalist.DataContext = GetItemGroups(vcarta.productos.OrderBy(o => o.name).ToList(), c => c.categoryID.ToString());
            categorias = new ObservableCollection<objetoslistas.categories>(vcarta.categorias);
            discount = vcarta.productos[0].discountToVIP;

        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (vistapedido.Height > 0)
            {
                listaout.Begin();
                e.Cancel = true;
            }
        }
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

        private void pedir_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (cartalist.SelectedItem == null)
                return;

            bool exist = false;
            var item = (sender as LongListSelector).SelectedItem as objetoslistas.products;
            if (pedido!=null)
            {
                foreach (objetoslistas.products prod in pedido)
                {
                    if (prod.productID == item.productID)
                    {
                        prod.cantidad++;
                        preciototal += (item.price-item.price*discount);
                        exist = true;
                    }
                }
            }
            if (exist == false)
            {
                pedido.Add(item);
                preciototal += item.price - item.price * discount;
            }
            // Reset selected item to null
            cartalist.SelectedItem = null;
        }

        private void quitar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (cartalist.SelectedItem == null)
                return;

            var item = (sender as LongListSelector).SelectedItem as objetoslistas.products;
            if (pedido != null)
            {
                foreach (objetoslistas.products prod in pedido)
                {
                    if (prod.productID == item.productID)
                    {
                        prod.cantidad--;
                        preciototal -= item.price - item.price * discount;
                        if (prod.cantidad<=0)
                        {
                            pedido.Remove(item);
                        }
                    }
                }
            }
            // Reset selected item to null
            cartalist.SelectedItem = null;
        }

        private void pedidobot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (vistapedido.Height == 80)
                listain.Begin();
            else
                listaout.Begin();
        }
    }
}