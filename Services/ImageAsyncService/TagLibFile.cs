using System.IO;

namespace MemeFolder.Services.ImageAsyncService
{
	/// <summary>Класс для передачи TagLib.File.Create() потоков в явном виде.</summary>
	public class TagLibFile : TagLib.File.IFileAbstraction
	{
		public string Name { get; }
		public Stream ReadStream { get; }
		public Stream WriteStream { get; }

		public void CloseStream(Stream stream)
		{
			stream?.Close();
			stream?.Dispose();
		}

		/// <summary>Конструктор с заданием всех свойств</summary>
		/// <param name="name">Gets the name or identifier used by the implementation.</param>
		/// <param name="readStream">Gets a readable, seekable stream for the file referenced by the current instance.</param>
		/// <param name="writeStream">Gets a writable, seekable stream for the file referenced by the current instance.</param>
		public TagLibFile(string name, Stream readStream, Stream writeStream)
		{
			Name = name;
			ReadStream = readStream;
			WriteStream = writeStream;
		}
	}
}
