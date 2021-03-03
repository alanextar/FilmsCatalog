using AutoMapper;
using FilmsCatalog.Models;
using FilmsCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog
{
	public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Film, FilmCreateEdit>();
            CreateMap<FilmCreateEdit, Film>();
            CreateMap<Film, FilmIndexData>();
        }
    }
}
