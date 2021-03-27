using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace MemeFolder.Services.ImageAsyncService
{
	public class ImageAsyncCollection<T> : ObservableCollection<T>
		where T : ImageAsyncBase
	{
		private ImageSource _imageDefault;

		/// <summary>Изображение по умолчанию.
		/// Выводится пока не загружено основное изображение</summary>
		public ImageSource ImageDefault
		{
			get => _imageDefault;
			set
			{
				if (_imageDefault == value) return;
				_imageDefault = value;
				OnPropertyChanged(ImageDefaultArgs);
			}
		}
		private static readonly PropertyChangedEventArgs ImageDefaultArgs
			= new PropertyChangedEventArgs(nameof(ImageDefault));

		public ImageAsyncCollection()
		{
			PropertyChanged += ImageAsyncCollection_PropertyChanged;
		}

		public ImageAsyncCollection(ImageSource imageDefault)
			: this()
		{
			ImageDefault = imageDefault;
		}

		protected override void InsertItem(int index, T item)
		{
			item.ImageDefault = ImageDefault;
			base.InsertItem(index, item);
		}

		private void ImageAsyncCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(ImageDefault))
				foreach (ImageAsyncBase image in this)
					image.ImageDefault = ImageDefault;
		}


	}
}
