using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Partyme
{
    public class objetoslistas
    {
        //metodos serializables de llamada y recepcion
        //checkdevice
        [DataContract]
        public class checkdeviceidinput
        {
            [DataMember(Name = "deviceID")]
            public string deviceID = App.device_id; //"tSzqi/G68SbYLN6JC+28ZncFoKTbCN/14LKy6oE74TU=";
        }
        [DataContract]
        public class checkdeviceidoutput
        {
            [DataMember(Name = "error")]
            public string error { get; set; }
            [DataMember(Name = "perfil")]
            public Profile perfil { get; set; }
        }
        //createprofile
        [DataContract]
        public class createprofileinput
        {
            [DataMember(Name = "deviceID")]
            public string textodeviceid = App.device_id;
            [DataMember(Name = "isMen")]
            public bool sexo { get; set; }
            [DataMember(Name = "os")]
            public int numos = 3;
            [DataMember(Name = "locale")]
            public string textolocale = CultureInfo.CurrentUICulture.Name.ToString();
        }
        [DataContract]
        public class createprofileoutput
        {
            [DataMember(Name = "error")]
            public string ERROR { get; set; }
            [DataMember(Name = "userID")]
            public string userID { get; set; }
            [DataMember(Name = "timestamp")]
            public int timestamp { get; set; }
        }
        //getprofile
        [DataContract]
        public class getprofileinput
        {
            [DataMember(Name = "userID")]
            public int userID = 154; // (int)IsolatedStorageSettings.ApplicationSettings["userid"];
            [DataMember(Name = "profileTimestamp")]
            public string profileTime { get; set; }
            [DataMember(Name = "timestamp")]
            public int timestamp { get; set; }

        }
        [DataContract]
        public class getprofileoutput
        {
            [DataMember(Name = "error")]
            public string ERROR { get; set; }
            [DataMember(Name = "response")]
            public Profile perfil { get; set; }
        }
        //getgeograficplaces       
        [DataContract]
        public class getgeograficplacesinput
        {
            [DataMember(Name = "userID")]
            public int userID = 154; //(int)IsolatedStorageSettings.ApplicationSettings["userid"];
            [DataMember(Name = "latitude")]
            public double latitude { get; set; }
            [DataMember(Name = "longitude")]
            public double longitude { get; set; }

        }
        [DataContract]
        public class getgeograficplacesoutput
        {
            [DataMember(Name = "places")]
            public List<Pincho> pinchos { get; set; }
            [DataMember(Name = "hotspots")]
            public List<Hotspot> hotspots { get; set; }
            [DataMember(Name = "events")]
            public List<Evento> events { get; set; }
            [DataMember(Name = "premiumPlaceID")]
            public int premiumPlaceID { get; set; }
        }
        //getplaces
        [DataContract]
        public class getPlacesinput
        {
            [DataMember(Name = "placeID")]
            public int placeID { get; set; }

        }
        [DataContract]
        public class getPlacesoutput
        {
            [DataMember(Name = "place")]
            public Local place { get; set; }

        }
        //setprofile
        [DataContract]
        public class setprofileinput
        {
            [DataMember(Name = "userID")]
            public int userID = 154; // (int)IsolatedStorageSettings.ApplicationSettings["userid"];
            [DataMember(Name = "nickName")]
            public string nickName { get; set; }
            [DataMember(Name = "statusMessage")]
            public string statusMessage { get; set; }
            [DataMember(Name = "mail")]
            public string mail { get; set; }
            [DataMember(Name = "isMen")]
            public bool isMen { get; set; }
            [DataMember(Name = "mapVisibility")]
            public bool mapVisibility { get; set; }
            [DataMember(Name = "locale")]
            public string locale = CultureInfo.CurrentUICulture.Name.ToString();
            [DataMember(Name = "phoneCountryCode")]
            public string countrycode = CultureInfo.CurrentUICulture.Name.ToString();
            [DataMember(Name = "phone")]
            public string phone { get; set; }
            [DataMember(Name = "birthDate")]
            public string birthDate { get; set; }
        }
        //nearusers
        [DataContract]
        public class nearusersinput
        {
            [DataMember(Name = "latitude")]
            public double latitude { get; set; }
            [DataMember(Name = "longitude")]
            public double longitude { get; set; }

        }
        [DataContract]
        public class nearusersoutput
        {
            [DataMember(Name = "user")]
            public List<user> usuarios { get; set; }

        }
        
        //objetos serializables construidos en metodos anteriores
        //getgeograficplaces
        [DataContract]
        public class Pincho
        {
            [DataMember(Name = "placeID")]
            public int placeId { get; set; }
            [DataMember(Name = "name")]
            public string name { get; set; }
            [DataMember(Name = "latitude")]
            public double latitude { get; set; }
            [DataMember(Name = "longitude")]
            public double longitude { get; set; }
            [DataMember(Name = "numberOfMen")]
            public int number_of_men { get; set; }
            [DataMember(Name = "numberOfWomen")]
            public int number_of_women { get; set; }
            [DataMember(Name = "type")]
            public int type { get; set; }
            [DataMember(Name = "subtype")]
            public string subtype { get; set; }
            [DataMember(Name = "logoURL")]
            public string logo_image { get; set; }
            [DataMember(Name = "numberOfLikePeople")]
            public int number_like_people { get; set; }
            [DataMember(Name = "isSubscriptionActive")]
            public bool isSubscriptionActive { get; set; }
            public string user_vip_in_place
            {
                get
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Contains("favorites"))
                    {
                        int[] favorites = (int[])IsolatedStorageSettings.ApplicationSettings["favorites"];
                        foreach (int item in favorites)
                        {
                            if (item == placeId)
                            {
                                user_favourite_place = "/Images/iconos/heart.png";
                            }
                            else
                            {
                                user_favourite_place = "/Images/iconos/heart.outline.png";
                            }
                        }
                    }
                    else
                    {
                        user_favourite_place = "/Images/iconos/heart.outline.png";
                    }
                    return user_favourite_place;
                }
                set { }
            }
            public string user_favourite_place
            {
                get
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Contains("vipPlaces"))
                    {
                        int[] vip = (int[])IsolatedStorageSettings.ApplicationSettings["vipPlaces"];
                        foreach (int item in vip)
                        {
                            if (item == placeId)
                            {
                                user_vip_in_place = "/Images/iconos/vip.png";
                            }
                            else
                            {
                                user_vip_in_place = "/Images/iconos/vip.wh.png";
                            }
                        }
                    }
                    else
                    {
                        user_vip_in_place = "/Images/iconos/vip.wh.png";
                    }
                    return user_vip_in_place;
                }
                set { }
            }
            public string type_usable
            {
                get
                {
                    if (type == 1)
                    {
                        type_usable = "/images/iconos/cup.png";
                    }
                    else if (type == 2)
                    {
                        type_usable = "/images/iconos/appbar.star.png";
                    }
                    else if (type == 3)
                    {
                        type_usable = "/images/iconos/food.png";
                    }
                    else if (type == 4)
                    {
                        type_usable = "/images/iconos/appbar.star.png";
                    }
                    else if (type == 5)
                    {
                        type_usable = "/images/iconos/appbar.star.png";
                    }
                    else if (type == 6)
                    {
                        type_usable = "/images/iconos/cup.png";
                    }
                    else if (type == 7)
                    {
                        type_usable = "/images/iconos/food.cross.png";
                    }
                    else if (type == 8)
                    {
                        type_usable = "/images/iconos/appbar.star.png";
                    }
                    else if (type == 9)
                    {
                        type_usable = "/images/iconos/beer.png";
                    }
                    else
                    {
                        type_usable = "/images/iconos/appbar.star.png";
                    }
                    return type_usable;
                }
                set { }
            }
            public GeoCoordinate posicion
            {
                get
                {
                    GeoCoordinate posicion = new GeoCoordinate(latitude, longitude);
                    return posicion;
                }
                set { }
            }
            public string distancia
            {
                get
                {
                    var localcoord = new GeoCoordinate(latitude, longitude);
                    var centroccord = new GeoCoordinate(MainPage.centrox, MainPage.centroy);
                    distancia = (centroccord.GetDistanceTo(localcoord) / 1000).ToString() + " KM";
                    return distancia;
                }
                set { }
            }
        }
        [DataContract]
        public class Hotspot
        {
            [DataMember(Name="latitude")]
            public double latitude { get; set; }
            [DataMember(Name = "longitude")]
            public double longitude { get; set; }
            [DataMember(Name = "numberOfMen")]
            public int numberOfMen { get; set; }
            [DataMember(Name = "numberOfWomen")]
            public int numberOfWomen { get; set; }
        }
        [DataContract]
        public class Evento
        {
            [DataMember(Name = "eventID")]
            public string eventID { get; set; }
            [DataMember(Name = "type")]
            public int type { get; set; }
            public string type_usable
            {
                get
                {
                    if (type == 1)
                    {
                        type_usable = "/images/iconos/appbar.email.png";
                    }
                    else
                    {
                        type_usable = "/images/iconos/appbar.email.png";
                    }
                    return type_usable;
                }
                set { }
            }
            [DataMember(Name = "subtype")]
            public string subtype { get; set; }
            [DataMember(Name = "title")]
            public string title { get; set; }
            [DataMember(Name = "begin")]
            public string begin { get; set; }
            [DataMember(Name="latitude")]
            public double latitude { get; set; }
            [DataMember(Name = "longitude")]
            public double longitude { get; set; }
            [DataMember(Name = "numberOfMen")]
            public int number_of_men { get; set; }
            [DataMember(Name = "numberOfWomen")]
            public int number_of_women { get; set; }
            [DataMember(Name = "price")]
            public double price { get; set; }
            [DataMember(Name = "imageURL")]
            public string imageURL { get; set; }
        }
        //checkdeviceid - getprofile
        [DataContract]
        public class Profile
        {
            [DataMember(Name = "otherImageURLs")]
            public string[] otherImageURLs { get; set; }
            [DataMember(Name = "preferences")]
            public PREFERENCES preferences { get; set; }
            [DataMember(Name = "favorites")]
            public int[] favorites { get; set; }
            [DataMember(Name = "parties")]
            public int[] parties { get; set; }
            [DataMember(Name = "events")]
            public string[] events { get; set; }
            [DataMember(Name = "vipPlaces")]
            public int[] vipPlaces { get; set; }
            [DataMember(Name = "userID")]
            public int userID { get; set; }
            [DataMember(Name = "mail")]
            public string mail { get; set; }
            [DataMember(Name = "phone")]
            public string phone { get; set; }
            [DataMember(Name = "birthDate")]
            public string birthDate { get; set; }
            [DataMember(Name = "isMen")]
            public bool isMen { get; set; }
            [DataMember(Name = "imageURL")]
            public string imageURL { get; set; }
            [DataMember(Name = "nickName")]
            public string nickName { get; set; }
            [DataMember(Name = "statusMessage")]
            public string statusMessage { get; set; }
            [DataMember(Name = "mapVisibility")]
            public bool mapVisibility { get; set; }
            [DataMember(Name = "lastupdate")]
            public int lastUpdate { get; set; }
            [DataMember(Name = "isValidMail")]
            public bool isValidMail { get; set; }
            [DataMember(Name = "isValidPhone")]
            public bool isValidPhone { get; set; }
            [DataMember(Name = "phoneCountryCode")]
            public string phoneCountryCode { get; set; }

            public DateTime birthDate_usable
            {
                get
                {
                    birthDate_usable = DateTime.ParseExact(birthDate, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                    return birthDate_usable;
                }
                set { }
            }
        }
        [DataContract]
        public class PREFERENCES
        {
           [DataMember(Name = "likesMen")]
           public bool likesMen { get; set; }
           [DataMember(Name = "likesWomen")]
           public bool likesWomen { get; set; }
           [DataMember(Name = "minAge")]
           public double minAge { get; set; }
           [DataMember(Name = "maxAge")]
           public double maxAge { get; set; }
           [DataMember(Name = "radio")]
           public double radio { get; set; }
           [DataMember(Name = "placeTypes")]
           public string[] placeTypes { get; set; }
           [DataMember(Name = "eventTypes")]
           public string[] eventTypes { get; set; }
         }
        [DataContract]
        public class user
        {
            [DataMember (Name= "userID")]
            public string userID {get;set;}
            [DataMember (Name= "name")]
            public string name {get;set;}
            [DataMember (Name= "statusMessage")]
            public string statusMessage {get;set;}
            [DataMember(Name = "birthDate")]
            public string birthDate { get; set; }
            [DataMember(Name = "isMen")]
            public bool isMen { get; set; }
            [DataMember(Name = "numImages")]
            public int numImages { get; set; }
            [DataMember(Name = "profileImage")]
            public string profileImage { get; set; }
            [DataMember(Name = "images")]
            public string[] images { get; set; }
            [DataMember(Name = "latitude")]
            public double latitude { get; set; }
            [DataMember(Name = "longitude")]
            public double longitude { get; set; }
            [DataMember(Name = "isFriend")]
            public bool isFriend { get; set; }
            [DataMember(Name = "parties")]
            public int[] parties { get; set; }
        }
        [DataContract]
        public class Local
        {
                [DataMember (Name="placeID")]
                public int placeID {get;set;}
                [DataMember(Name = "name")]
                public string name { get; set; }
                [DataMember(Name = "info")]
                public string info { get; set; }
                [DataMember(Name = "address")]
                public string address { get; set; }
                [DataMember(Name = "city")]
                public string city { get; set; }
                [DataMember(Name = "postalCode")]
                public string postalCode { get; set; }
                [DataMember(Name = "phone")]
                public string phone { get; set; }
                [DataMember(Name = "hoursOfOpeningText")]
                public string hoursOfOpeningText { get; set; }
                [DataMember(Name = "messageForMen")]
                public string messageForMen { get; set; }
                [DataMember(Name = "messageForWomen")]
                public string messageForWomen { get; set; }
                [DataMember(Name = "numberOfLikePeople")]
                public int numberOfLikePeople { get; set; }
                [DataMember(Name = "isSubscriptionActive")]
                public bool isSubscriptionActive { get; set; }
                [DataMember(Name = "numberOfMen")]
                public int numberOfMen { get; set; }
                [DataMember(Name = "numberOfWomen")]
                public int numberOfWomen { get; set; }
                [DataMember (Name="services")]
                public int[] services {get;set;}
                [DataMember (Name="imageURLs")]
                public string[] imageURLs {get;set;}
        }
        [DataContract]
        public class PlaceImage
        {
                [DataMember (Name="imageDate")]
                public DateTime imageDate {get;set;}
                [DataMember (Name="imagePlaceID")]
                public int imagePlaceID {get;set;}
                [DataMember (Name="profileImageURL")]
                public string profileImageURL {get;set;}
                [DataMember (Name="urlString")]
                public string urlString {get;set;}
                [DataMember (Name="userName")]
                public string userNameL {get;set;}
                [DataMember (Name="userID")]
                public int userID {get;set;} 
        }
        
        //objetos no serializables para uso directo
        public class Usuario
        {
            public int userID { get; set; }
            public string name { get; set; }
            public string statusMessage { get; set; }
            public DateTime birthDate { get; set; }
            public int edad;
            public string isMen { get; set; }
            public int numImages { get; set; }
            public string profileImage { get; set; }
            public string[] images { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string distancia;
            public bool isFriend { get; set; }
            public int[] parties { get; set; }

            public Usuario(string _userID, string _name, string _statusMessage, string _birthDate, bool _isMen, int _numImages, string _profileImage,
                            string[] _images, double _latitude, double _longitude, bool _isFriend, int[] _parties)
            {
                userID = int.Parse(_userID);
                name = _name;
                statusMessage = _statusMessage;
                birthDate = DateTime.Parse(_birthDate);

                DateTime today = DateTime.Today;
                edad = today.Year - birthDate.Year;
                if (birthDate > today.AddYears(-edad)) edad--;

                if (_isMen == true)
                {
                    isMen = "/Images/iconos/icon.gender.male.png";
                }
                else
                {
                    isMen = "/Images/iconos/icon.gender.female.png";
                }
                numImages = _numImages;
                profileImage = _profileImage;
                images = _images;
                latitude = _latitude;
                longitude = _longitude;

                var profcoord = new GeoCoordinate(_latitude, _longitude);
                var centroccord = new GeoCoordinate(MainPage.centrox, MainPage.centroy);
                distancia = (centroccord.GetDistanceTo(profcoord) / 1000).ToString() + " KM";

                isFriend = _isFriend;
                parties = _parties;
            }
        }
        public class Tipousuario : System.Collections.IEnumerable 
        {
            public string Name { get; private set; }
            public System.Collections.Generic.List<Usuario> Items { get; private set; }

            public Tipousuario(string categoryName)
            {
                Name = categoryName;
                Items = new System.Collections.Generic.List<Usuario>();
            }
            public void AddUserItem(Usuario userItem)
            {
                Items.Add(userItem);
            }
            public System.Collections.IEnumerator GetEnumerator()
            {
                return this.Items.GetEnumerator();
            }

        }
       
        //creo que estos ya no se usan
        public class DetalleLocal
        {
            //estos vienen de Local
            public int placeID { get; set; }
            public string name { get; set; }
            public string info { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string postalCode { get; set; }
            public string phone { get; set; }
            public string hoursOfOpeningText { get; set; }
            public string messageForMen { get; set; }
            public string messageForWomen { get; set; }
            public int numberOfLikePeople { get; set; }
            public bool isSubscriptionActive { get; set; }
            public int numberOfMen { get; set; }
            public int numberOfWomen { get; set; }
            public int[] services { get; set; }
            public string[] imageURLs { get; set; }
            //estos vienen de Pincho
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string subtype { get; set; }
            public string logo_image { get; set; }
            public string user_vip_in_place { get; set; }
            public string user_favourite_place { get; set; }
            public string type_usable { get; set; }
            public string distancia { get; set; }

            public DetalleLocal(int local_id, string local_name, string local_info, string local_address, string local_city, string local_postalCode, string local_phone,
                                string local_hoursOfOpeningText, string local_messageForMen, string local_messageForWomen, int local_numberOfLikePeople,
                                bool local_isSubscriptionActive, int local_numberOfMen, int local_numberOfWomen, int[] local_services, string[] local_imageURLs,
                                double pincho_latitude, double pincho_longitude, string pincho_subtype, string pincho_logo_image, string pincho_user_vip_in_place,
                                string pincho_userfavouriteplace, string pincho_type_usable, string pincho_distancia)
            {
                placeID = local_id;
                name = local_name;
                info = local_info;
                address = local_address;
                city = local_city;
                postalCode = local_postalCode;
                phone = local_phone;
                hoursOfOpeningText = local_hoursOfOpeningText;
                messageForMen = local_messageForMen;
                messageForWomen = local_messageForWomen;
                numberOfLikePeople = local_numberOfLikePeople;
                isSubscriptionActive = local_isSubscriptionActive;
                numberOfMen = local_numberOfMen;
                numberOfWomen = local_numberOfWomen;
                services = local_services;
                imageURLs = local_imageURLs;
                latitude = pincho_latitude;
                longitude = pincho_longitude;
                subtype = pincho_subtype;
                logo_image = pincho_logo_image;
                user_vip_in_place = pincho_user_vip_in_place;
                user_favourite_place = pincho_userfavouriteplace;
                type_usable = pincho_type_usable;
                distancia = pincho_distancia;
            }
            public class People
            {
                public string profile_id { get; set; }
                public string profile_picture { get; set; }
                public int profile_sex { get; set; }
                public DateTime profile_date_of_birth { get; set; }
                public string profile_nickname { get; set; }
                public string profile_status { get; set; }
                public string profile_mail { get; set; }
                public double profile_latitude { get; set; }
                public double profile_longitude { get; set; }
                public string profile_distancia { get; set; }
                public string place_name { get; set; }
                public bool profile_online { get; set; }

                public People(string _profile_id, string _profile_picture, string _profile_sex, string _profile_date_of_birth, string _profile_nickname,
                                string _profile_status, string _profile_mail, string _profile_latitude, string _profile_longitude, string _place_name, string _profile_online)
                {
                    profile_id = _profile_id;
                    if (_profile_sex == "h")
                    {
                        profile_sex = 1;
                    }
                    else
                    {
                        profile_sex = 0;
                    }
                    profile_date_of_birth = DateTime.Parse(_profile_date_of_birth);
                    profile_nickname = _profile_nickname;
                    profile_status = _profile_status;
                    profile_mail = _profile_mail;
                    profile_latitude = Double.Parse(_profile_latitude);
                    profile_longitude = Double.Parse(_profile_longitude);
                    var profcoord = new GeoCoordinate(profile_latitude, profile_longitude);
                    var centroccord = new GeoCoordinate(MainPage.centrox, MainPage.centroy);
                    profile_distancia = (centroccord.GetDistanceTo(profcoord) / 1000).ToString() + " KM";
                    place_name = _place_name;
                    if (_profile_online == "1")
                    {
                        profile_online = true;
                    }
                    else
                    {
                        profile_online = false;
                    }

                }
            }



        }

    }
}
