using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmsCatalog.FileSystemBucket
{
	public class FileSystemBucketStorageService : IBucketStorageService
	{
		public string RootPath { get; }
		public string MainDirectory { get; } = "uploads";

		public FileSystemBucketStorageService(IWebHostEnvironment appEnvironment)
		{
			RootPath = appEnvironment.WebRootPath;
		}

		public IBucket GetBucket(string bucketName)
		{
			bucketName = bucketName.ToLower();
			bucketName = bucketName.Replace('\\', '/');
			return new FileSystemBucket(this, bucketName);
		}
	}
}