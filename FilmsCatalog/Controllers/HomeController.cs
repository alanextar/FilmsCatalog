using AutoMapper;
using FilmsCatalog.Data;
using FilmsCatalog.Extensions;
using FilmsCatalog.FileSystemBucket;
using FilmsCatalog.Helpers;
using FilmsCatalog.Models;
using FilmsCatalog.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private const int pageSize = 10;
        private const string imageStoragePath = "posters";

        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        IBucket _bucket;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, 
            IMapper mapper, UserManager<User> userManager, IWebHostEnvironment env, IBucket bucket)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _env = env;
            _bucket = bucket;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            _logger.LogInformation($"GET Request {HttpContext.Request.Headers[":path"]}");

            //Test data initializing
            //if (_context.Films.IsNullOrEmpty())
            //{
            //    _logger.LogInformation("Test data init Running");
            //    await TestData.Initialize(_context);
            //    _logger.LogInformation("Test data init Finished");
            //}

            var filmsCount = await _context.Films.CountAsync();

            var films = await _context.Films.Where(x => !x.IsDeleted).OrderByDescending(x => x.EditedAt)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .AsNoTracking()
                .Select(x => _mapper.Map<FilmIndexData>(x))
                .ToListAsync();

            var pagedList = new PaginatedList<FilmIndexData> (films, filmsCount, pageNumber, pageSize);

            var model = new FilmsIndexVm(pagedList);
            model.UserId = UserHelpers.GetUserIdIfRegister(User, _userManager);

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new FilmCreateEdit();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmCreateEdit model)
        {
            var userId = UserHelpers.GetUserIdIfRegister(User, _userManager);
			if (!userId.HasValue)
			{
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                model.OwnerId = userId.Value;

                if (model.PosterPathUpload.IsDataAvailable())
                {
                    var imageName = model.PosterPathUpload.FileName.ToLower();
                    var path = model.PosterPathUpload
                        .UploadUniqueImage(_bucket, imageStoragePath, imageName);

                    model.PosterPath = path.ImagePath;
                }

                var film = _mapper.Map<FilmCreateEdit, Film>(model);


                _context.Films.Add(film);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = film.Id });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (!id.HasValue)
                return NotFound();

            var film = await _context.Films.FindAsync(id.Value);
            if (film == null || film.IsDeleted)
                return NotFound(); 

            var model = _mapper.Map<FilmIndexData>(film);

            var currentUserId = UserHelpers.GetUserIdIfRegister(User, _userManager);
			if (currentUserId.HasValue && Guid.Equals(film.OwnerId, currentUserId))
			{
                model.IsOwner = true;
			}

            return View(model);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (!id.HasValue)
                return NotFound();

            var film = await _context.Films.FindAsync(id.Value);
            if (film == null || film.IsDeleted)
                return NotFound();

            var model = _mapper.Map<FilmCreateEdit>(film);
            film.EditedAt = DateTime.UtcNow;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FilmCreateEdit model)
        {
            var userId = UserHelpers.GetUserIdIfRegister(User, _userManager);
            if (!userId.HasValue)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var film = _context.Films.Find(model.Id);
                if (film == null || film.IsDeleted)
                    return NotFound();

                _mapper.Map(model, film);
                if (model.PosterPathUpload.IsDataAvailable())
                {
                    var imageName = model.PosterPathUpload.FileName.ToLower();
                    model.PosterPathUpload
                        .UploadUniqueImage(_bucket, imageStoragePath, imageName);
                }

                try
                {
                    _context.Films.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Films.Find(film.Id) == null)
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var filmToDelete = await _context.Films.FindAsync(id.Value);

            if (filmToDelete == null)
                return NotFound();

            return View(filmToDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var filmToDelte = await _context.Films.FindAsync(id);
            if (filmToDelte == null)
                return NotFound();

            filmToDelte.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
