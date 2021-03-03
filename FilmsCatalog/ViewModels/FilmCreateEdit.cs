using FilmsCatalog.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels
{
	public class FilmCreateEdit
	{
		public long Id { get; set; }

		[StringLength(800)]
		[Display(Name = "Заголовок")]
		[Required(ErrorMessage = "поле {0} обязательно для заполнения")]
		public string Title { get; set; }

		[Display(Name = "Год выпуска")]
		[RangeUntilCurrentYear(1900, ErrorMessage = "не допустимое значение")]
		public int ReleaseYear { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }
		public Guid OwnerId { get; set; }

		[Display(Name = "Постер")]
		public IFormFile PosterPathUpload { get; set; }
		public string PosterPath { get; set; }

		[Display(Name = "Режиссер")]
		public string Producer { get; set; }
	}
}
