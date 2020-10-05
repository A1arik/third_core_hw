using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using my_books_shop.Data;
using my_books_shop.Models;

namespace my_books_shop.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();

            return View(await _context.Books.Include(b => b.AuthorsB).Include(b => b.GenresB).ToListAsync());
        }

        [HttpPost]
        public ActionResult GetBooksByAuthorsAndGenres(int[] selectedAuthors = null, int[] selectedGenres = null)
        {
            List<Book> books = new List<Book>();

            if (selectedAuthors != null)
                books.AddRange(_context.Books.Where(b => b.AuthorsB.FirstOrDefault(a => selectedAuthors.Contains(a.Author.Id)) != null));
            if (selectedGenres != null)
                books.AddRange(_context.Books.Where(b => b.GenresB.FirstOrDefault(g => selectedGenres.Contains(g.Genre.Id)) != null));

            return PartialView(books.Distinct().ToList());
        }


        [HttpPost]
        public ActionResult GetBooksName(string name)
        {
            return PartialView("~/Views/Books/GetBooksByAuthorsAndGenres.cshtml", _context.Books.Where(b => b.Name.Contains(name)).ToList());
        }


        // GET: Books
        public ActionResult ByAuthor(string authorSlug)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            _context.Authors.Load();
            return View("~/Views/Books/Index.cshtml", _context.Authors.First(a => a.Slug == authorSlug).BooksA.ToList());
        }

        // GET: Books
        public ActionResult ByAuthorId(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            _context.Authors.Load();
            return View("~/Views/Books/Index.cshtml", _context.Authors.First(a => a.Id == id).BooksA.ToList());
        }

        // GET: Books
        public ActionResult ByGenreId(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            _context.Genres.Load();
            return View("~/Views/Books/Index.cshtml", _context.Genres.First(g => g.Id == id).BooksG.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
