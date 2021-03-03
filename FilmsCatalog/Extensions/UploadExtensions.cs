using System;
using System.IO;
using System.Linq;
using FilmsCatalog.FileSystemBucket;
using Microsoft.AspNetCore.Http;

namespace FilmsCatalog.Extensions
{
	public static class UploadExtensions
	{
		private static string fullFilePath = "";

		private static string GetFullFilePath(string directoryName, string fileName)
		{
			fullFilePath = $"{directoryName}/{fileName}";

			fullFilePath = fullFilePath.Replace('\\', Path.DirectorySeparatorChar);
			fullFilePath = fullFilePath.Replace('/', Path.DirectorySeparatorChar);

			return fullFilePath;
		}

		public static string UploadUniqueFileAndGetUrl(this IFormFile formFile, IBucket bucket, string directoryName, string fileName)
		{
			var uniqueDirName = Guid.NewGuid().ToString("N");
			directoryName = $"{directoryName}/{uniqueDirName}";
			return formFile.UploadFileAndGetUrl(bucket, directoryName, fileName);
		}

		public static string UploadFileAndGetUrl(this IFormFile formFile, IBucket bucket, string directoryName, string fileName = null)
		{
			if (fileName == null)
				fileName = formFile.GenerateUniqueFileName();
			else
				fileName = fileName.SanitizeFilePath();

			var fullFilePath = GetFullFilePath(directoryName, fileName);

			using (var stream = formFile.OpenReadStream())
			{
				bucket.WriteObject(fullFilePath, stream);
			}
			string url = bucket.GetPublicURL(fullFilePath);

			return url;
		}

		public static ImagesPaths UploadUniqueImage(this IFormFile formFile, IBucket bucket
			, string directoryName, string fileName)
		{
			var uniqueDirName = Guid.NewGuid().ToString("N");
			directoryName = $"{directoryName}/{uniqueDirName}";
			return formFile.UploadImageAndGetUrl(bucket, directoryName, fileName);
		}

		public static ImagesPaths UploadImageAndGetUrl(this IFormFile formFile, IBucket bucket, 
			string directoryName, string fileName = null)
		{
			string url = formFile.UploadFileAndGetUrl(bucket, directoryName, fileName);

			return new ImagesPaths(url);
		}

		public static string SanitizeFilePath(this string origFileName)
		{
			var invalids = Path.GetInvalidFileNameChars().ToList();
			if (!invalids.Contains('\\'))
				invalids.Add('\\');
			if (!invalids.Contains('/'))
				invalids.Add('/'); // Just in case! :)
			var newName = string.Join("", origFileName.ToLower().Split(invalids.ToArray(), 
				StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
			return newName;
		}

		public static bool IsDataAvailable(this IFormFile formFile)
		{
			return formFile != null && formFile.Length > 0 && !string.IsNullOrEmpty(formFile.FileName);
		}

		public static string GenerateUniqueFileName(this IFormFile formFile)
		{
			return Guid.NewGuid().ToString("N") + Path.GetExtension(formFile.FileName).ToLower();
		}

	}

	public class ImagesPaths
	{
		public string ImagePath { get; set; }


		public ImagesPaths(string image)
		{
			ImagePath = image;
		}
	}
}
