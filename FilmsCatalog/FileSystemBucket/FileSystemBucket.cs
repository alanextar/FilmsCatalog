using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FilmsCatalog.FileSystemBucket
{
	public class FileSystemBucket : IBucket
	{
		public string RootPath { get; }
		public string MainDirectory { get; }
		public string BucketName { get; }

		public FileSystemBucket(FileSystemBucketStorageService fileSystem, string bucketName)
		{
			RootPath = fileSystem.RootPath;
			MainDirectory = fileSystem.MainDirectory;
			BucketName = bucketName;
		}

		public string GetFullFilePath(string fileName)
		{
			fileName = fileName.Replace('\\', Path.DirectorySeparatorChar);
			fileName = fileName.Replace('/', Path.DirectorySeparatorChar);
			return Path.Combine(RootPath, MainDirectory, BucketName, fileName);
		}

		public void ReadObject(string fileName, Stream readStream)
		{
			throw new NotImplementedException();
		}

		public void WriteObject(string fileName, Stream writeStream)
		{
			string path = GetFullFilePath(fileName);

			string dir = Path.GetDirectoryName(path);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using (var file = File.OpenWrite(path))
			{
				writeStream.CopyTo(file);
			}
		}

		public void DeleteObject(string fileName)
		{
			throw new NotImplementedException();
		}

		public string GetPublicURL(string fileName)
		{
			fileName = fileName.Replace('\\', '/');
			return $"/{MainDirectory}/{BucketName}/{fileName}";
		}

	}
}
