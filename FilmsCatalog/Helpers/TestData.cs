using FilmsCatalog.Data;
using FilmsCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Helpers
{
	public static class TestData
	{
		public static async Task Initialize(ApplicationDbContext context)
		{
			var films = new List<Film>();
			for (int i = 0; i < 100; i++)
			{
				films.Add(new Film
				{
					Title = $"Title #{i}",
					Description = $"Description #{i}",
					Producer = $"Producer #{i}",
				});
			}

			await context.Films.AddRangeAsync(films);
			await context.SaveChangesAsync();
		}
	}
}
