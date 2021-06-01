namespace MemeFolder.Services.ImageAsyncService
{
	/// <summary>Базовый класс с ImageSource загружаемым
	/// ассинхронно после первого требования и возможностью добавления дополнительного Контента</summary>
	/// <typeparam name="T">Тип Контента</typeparam>
	public abstract class ImageAsyncBase<T> : ImageAsyncBase
	{
		private T _content;

		public T Content { get => _content; set => SetProperty(ref _content, value); }
	}
}
