using FilmsCatalog.Helpers;
using FilmsCatalog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels
{
	public class FilmsIndexVm
	{
		public FilmsIndexVm(PaginatedList<FilmIndexData> films)
		{
			Items = films;
		}

		public PaginatedList<FilmIndexData> Items { get; private set; }
		public Guid? UserId { get; set; }
	}

    public class FilmIndexData
    {
        public FilmIndexData() { }

		public long Id { get; set; }

		[Display(Name = "Название")]
		public string Title { get; private set; }

		[Display(Name = "Год выпуска")]
		public int ReleaseYear { get; set; }

		[Display(Name = "Режиссер")]
		public string Producer { get; set; }

		[Display(Name = "Постер")]
		public string PosterPath { get; set; }
		public Guid OwnerId { get; set; }

		public bool IsOwner { get; set; }

	}
}
