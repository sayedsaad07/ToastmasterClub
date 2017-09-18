using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookKeeperSPAAngular.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookKeeperSPAAngular.Controllers
{

    [Produces("application/json")]
    [Route("api/Book")]
    public class BookKeeperController : Controller
    {
        private readonly IBookKeeperRepository _repository;
        private readonly ILogger<BookKeeperController> _logger;

        public BookKeeperController(IBookKeeperRepository context
                                    , ILogger<BookKeeperController> logger)
        {
            _repository = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBook()
        {
            int userid = 1;
            return Json(await _repository.GettBooksByUser(userid));

        }


        // GET: api/Posts/5
        [HttpGet("{id}", Name = "BookGET")]
        public async Task<IActionResult> GetbookKeeper([FromRoute] int id, bool includechilddata = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BookKeeper book = await _repository.GetbookKeeperByID(id);

            if (includechilddata)
            {
                int userid = 1;
                return Json(await _repository.GettBooksByUser(userid));
            }
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBookItem([FromBody]BookKeeperViewModel bookKeeperVM)
        {

            try
            {
                _logger.LogInformation("Add a new book to my user Library");
                if (bookKeeperVM != null)
                {
                    var book = new BookKeeper()
                    {
                        BookName = bookKeeperVM.BookName,
                        BookUrl = bookKeeperVM.BookUrl,
                        Category = bookKeeperVM.Category,
                        Rating = bookKeeperVM.Rating,
                        Startreading = bookKeeperVM.Startreading,
                        StopReading = bookKeeperVM.StopReading
                        ,
                        Userid = 1
                    };
                    await _repository.AddbookKeeper(book);
                    if (book == null || book.BookKeeperId <= 0)
                    {
                        _logger.LogWarning("Book is not added to current user");
                    }
                    var newurl = Url.Link("BookGET", new { id = book.BookKeeperId });
                    return Created(newurl, book);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Add book API throws an exception{ex}");
            }
            return BadRequest();
        }


        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookKeeper([FromRoute] int id, [FromBody] BookKeeperViewModel bookKeeperVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != book.BookKeeperId)
            //{
            //    return BadRequest();
            //}

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Provided book id {id} is invalid");
                    return BadRequest();
                }
                var book = await _repository.GetbookKeeperByID(id);
                //map book viewmodel to existing book
                book.Rating = bookKeeperVM.Rating > 0 ? bookKeeperVM.Rating : book.Rating;
                book.BookName = !string.IsNullOrEmpty(bookKeeperVM.BookName) ? bookKeeperVM.BookName : book.BookName;
                book.BookUrl = !string.IsNullOrEmpty(bookKeeperVM.BookUrl) ? bookKeeperVM.BookUrl : book.BookUrl;
                book.Category = !string.IsNullOrEmpty(bookKeeperVM.Category) ? bookKeeperVM.Category : book.Category;
                book.Startreading = bookKeeperVM.Startreading != DateTime.MinValue ? bookKeeperVM.Startreading : book.Startreading;
                book.StopReading = bookKeeperVM.StopReading != DateTime.MinValue ? bookKeeperVM.StopReading : book.StopReading;
                await _repository.UpdatebookKeeperByID(book);
                return Ok(book);
            }
            catch (DbUpdateConcurrencyException ex)
            {

                BookKeeper b = await _repository.GetbookKeeperByID(id);
                if (b == null)
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Unable to update book id{b.BookKeeperId}. throw exception {ex}");
                }
            }
            return NoContent();
        }

        // PUT: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBookKeeper([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            try
            {
                var book = await _repository.GetbookKeeperByID(id);
                if (book != null)
                {
                    await _repository.RemovebookKeeper(book);
                    return Ok();
                }
            }
            catch (DbUpdateConcurrencyException)
            {

                BookKeeper b = await _repository.GetbookKeeperByID(id);
                if (b == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    }
}
