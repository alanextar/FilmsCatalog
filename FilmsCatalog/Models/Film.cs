using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
	public class Film
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int ReleaseYear { get; set; }
		public string Producer { get; set; }
		public string OwnerId { get; set; }
		[ForeignKey(nameof(OwnerId))]
		public User Owner { get; set; }

		[Display(Name = "Постер")]
		public string PosterPath { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime EditedAt { get; set; } = DateTime.UtcNow;
	}
}
