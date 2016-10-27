using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;
using Partyme.VModel;
using Partyme.ViewModels;
using Partyme.Resources;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;

namespace Partyme.servicios
{
    public class franja
    {
        public string horamin { get; set; }
        public string horamax { get; set; }
    }
    
    public partial class Reserva : PhoneApplicationPage
    {
        private ViewModelExtended vextended;
        static int placeID;
        Place_extended local;
        public List<franja> listado = new List<franja>();
        public Reserva()
        {
            InitializeComponent();
            vextended = new ViewModelExtended();
        }
        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string idstring;
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("id", out idstring);
            placeID = Int32.Parse(idstring);

            local = await vextended.GetDefaultExtendeds(placeID);
        }
        private void fechareserva_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            switch (e.NewDateTime.Value.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    listado.Clear();
                    if (local.days.fri!=null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Viernes";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
                case DayOfWeek.Monday:
                    listado.Clear();
                    if (local.days.mon != null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Lunes";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
                case DayOfWeek.Saturday:
                    listado.Clear();
                    if (local.days.sat != null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Sabado";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
                case DayOfWeek.Sunday:
                    listado.Clear();
                    if (local.days.sun != null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Domingo";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
                case DayOfWeek.Thursday:
                    listado.Clear();
                    if (local.days.thu != null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Jueves";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
                case DayOfWeek.Tuesday:
                    listado.Clear();
                    if (local.days.tue != null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Martes";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
                case DayOfWeek.Wednesday:
                    listado.Clear();
                    if (local.days.wen != null)
                    {
                        foreach (string[] item in local.days.fri)
                        {
                            franja franja_ = new franja();
                            longListbusqueda.ListHeader = "Miercoles";
                            franja_.horamin = item[0];
                            franja_.horamax = item[1];
                            listado.Add(franja_);
                        }
                    }
                    longListbusqueda.DataContext = listado;
                    break;
            }
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            gridreserva.Visibility = Visibility.Visible;
            gridCabecera.Visibility = Visibility.Collapsed;
            verReserva.Visibility = Visibility.Collapsed;
        }
        private async void doReserva_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DateTime fecha = fechareserva.Value.Value.Date;
            DateTime hora = horaReserva.Value.Value;
            fecha = fecha.Add(hora.TimeOfDay); 

            Uri url = new Uri(AppResources.linkDualink + "insertBooking");
            objetoslistas.insertBookingIn paquete = new objetoslistas.insertBookingIn();
            paquete.placeID = local.placeID;
            paquete.userID = (int)IsolatedStorageSettings.ApplicationSettings["userID"];
            paquete.numberOfPeople = Convert.ToInt32(pax.Text);
            paquete.contactName = name.Text;
            paquete.contactPhone = phone.Text;
            paquete.comments = comments.Text;
            paquete.dateTime = fecha;

            string respuesta = await metodosJson.jsonPOST(url, paquete);
            var respuestajson = JsonConvert.DeserializeObject<objetoslistas.loginoutput>(respuesta.ToString());
            if (respuestajson.error == "")
            {
               
            }
            else
            {
                MessageBoxResult result =
                MessageBox.Show(respuestajson.error,
                "ERROR",
                MessageBoxButton.OK);
            }
        }
    }
}