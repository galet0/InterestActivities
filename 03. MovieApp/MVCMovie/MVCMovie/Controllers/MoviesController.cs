using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using MVCMovie.Models;
using MVCMovie.Models.BindingModels;
using MVCMovie.Models.ViewModels;

namespace MVCMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MVCMovieContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _wwwRootPath;

        public MoviesController(MVCMovieContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this._webHostEnvironment = webHostEnvironment;
            _wwwRootPath = _webHostEnvironment.WebRootPath;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchString)
        {
            //Task<List<Movie>> movies = _context.Movies.ToListAsync();
            var movies = from m in _context.Movies
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            //return View(await _context.Movies.ToListAsync());
            return View(await movies.ToListAsync());
        }

        //[HttpPost]
        //public async Task<IActionResult> Index(string searchString, bool notUsed)
        //{
        //    //Task<List<Movie>> movies = _context.Movies.ToListAsync();
        //    var movies = from m in _context.Movies
        //                 select m;

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        movies = movies.Where(m => m.Title.Contains(searchString));
        //    }

        //    //return View(await _context.Movies.ToListAsync());
        //    return View(await movies.ToListAsync());
        //}

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);


            if (movie == null)
            {
                return NotFound();
            }

            DetailsMovieViewModel detailsModel = new DetailsMovieViewModel
            {
                Id=movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Description = movie.Description,
                Category = movie.Category,
                Genre = movie.Genre,
                Price = movie.Price,
                Image = movie.Image, 
                Trailer = movie.Trailer
            };

            return View(detailsModel);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Description,Category,Image,Trailer")] MovieBindingModel createModel)
        {
            if (ModelState.IsValid)
            {
                string newGeneratedImageName = this.GetGeneratedFileName(createModel.Image.FileName);

                this.UploadImageFile(createModel, newGeneratedImageName);


                string newGeneratedTrailerName = this.GetGeneratedFileName(createModel.Trailer.FileName);

                this.UploadVideoFile(createModel, newGeneratedTrailerName);

                Movie movie = new Movie
                {
                    Title = createModel.Title,
                    ReleaseDate = createModel.ReleaseDate,
                    Description = createModel.Description,
                    Category = createModel.Category,
                    Genre = createModel.Genre,
                    Price = createModel.Price,
                    Image = newGeneratedImageName,
                    Trailer = newGeneratedTrailerName
                };

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createModel);
        }

        private string GetGeneratedFileName(string modelFileName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelFileName);

            string fileExtension = Path.GetExtension(modelFileName);

            string newGeneratedFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

            return newGeneratedFileName;
        }

        private void UploadImageFile(MovieBindingModel bindingModel, string imageName)
        {
            string path = this.CreateFilesPath("images", imageName, bindingModel);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                bindingModel.Image.CopyTo(fileStream);
            }
        }

        private void UploadVideoFile(MovieBindingModel bindingModel, string videoName)
        {
            string path = this.CreateFilesPath("videos", videoName, bindingModel);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                bindingModel.Trailer.CopyTo(fileStream);
            }
        }

        private string  CreateFilesPath(string directory, string fileName, MovieBindingModel bindingModel)
        {
            string path = Path.Combine($"{_wwwRootPath}/{directory}/", bindingModel.Title);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine($"{_wwwRootPath}/{directory}/{bindingModel.Title}", fileName);

            return path;
        }

        // GET: Movies/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            EditMovieBindingModel editModel = new EditMovieBindingModel
            {
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Description = movie.Description,
                Category = movie.Category,
                Genre = movie.Genre,
                Price = movie.Price,
                ImageUrl = movie.Image,
                VideoUrl = movie.Trailer
            };

            return View(editModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Category,Description,Image,Trailer")] EditMovieBindingModel editModel)
        {
            if (id != editModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    Movie movie = _context.Movies.Find(editModel.Id);

                    if (editModel.Image != null)
                    {
                        string newGeneratedImageFile = this.GetGeneratedFileName(editModel.Image.FileName);
                        this.UploadImageFile(editModel, newGeneratedImageFile);

                        movie.Image = newGeneratedImageFile;
                    }
                   
                    if(editModel.Trailer != null)
                    {
                        string newGeneratedVideoFile = this.GetGeneratedFileName(editModel.Trailer.FileName);
                        this.UploadVideoFile(editModel, newGeneratedVideoFile);

                        movie.Trailer = newGeneratedVideoFile;
                    }
                   


                    movie.Title = editModel.Title;
                    movie.ReleaseDate = editModel.ReleaseDate;
                    movie.Description = editModel.Description;
                    movie.Category = editModel.Category;
                    movie.Genre = editModel.Genre;
                    movie.Price = editModel.Price;

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(editModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editModel);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
