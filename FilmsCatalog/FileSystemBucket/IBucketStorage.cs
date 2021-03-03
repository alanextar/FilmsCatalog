using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FilmsCatalog.FileSystemBucket
{
	public interface IBucketStorageService
	{
		IBucket GetBucket(string bucketName);
	}

	public interface IBucketStorageFileProvider
	{
		PathString BasePath { get; }
		IFileProvider FileProvider { get; }
	}

	public interface IBucket
	{
		void ReadObject(string fileName, Stream readStream);
		void WriteObject(string fileName, Stream writeStream);
		void DeleteObject(string fileName);
		string GetPublicURL(string fileName);
	}
}
