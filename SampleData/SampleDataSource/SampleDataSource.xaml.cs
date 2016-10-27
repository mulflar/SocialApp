//      *********    NO MODIFIQUE ESTE ARCHIVO     *********
//      Este archivo se regenera mediante una herramienta de diseño.
//       Si realiza cambios en este archivo, puede causar errores.
namespace Blend.SampleData.SampleDataSource
{
	using System; 
	using System.ComponentModel;

// Para reducir de forma significativa la superficie de los datos de ejemplo en la aplicación de producción, puede establecer
// la constante de compilación condicional DISABLE_SAMPLE_DATA y deshabilitar los datos de ejemplo en tiempo de ejecución.
#if DISABLE_SAMPLE_DATA
	internal class SampleDataSource { }
#else

	public class SampleDataSource : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleDataSource()
		{
			try
			{
				Uri resourceUri = new Uri("/Partyme;component/SampleData/SampleDataSource/SampleDataSource.xaml", UriKind.RelativeOrAbsolute);
				System.Windows.Application.LoadComponent(this, resourceUri);
			}
			catch
			{
			}
		}

		private ItemCollection _Collection = new ItemCollection();

		public ItemCollection Collection
		{
			get
			{
				return this._Collection;
			}
		}
	}

	public class Item : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private double _años = 0;

		public double años
		{
			get
			{
				return this._años;
			}

			set
			{
				if (this._años != value)
				{
					this._años = value;
					this.OnPropertyChanged("años");
				}
			}
		}

		private string _nickName = string.Empty;

		public string nickName
		{
			get
			{
				return this._nickName;
			}

			set
			{
				if (this._nickName != value)
				{
					this._nickName = value;
					this.OnPropertyChanged("nickName");
				}
			}
		}
	}

	public class ItemCollection : System.Collections.ObjectModel.ObservableCollection<Item>
	{ 
	}
#endif
}
