using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookKeeperSPAAngular.Model;
using Microsoft.EntityFrameworkCore;

namespace BookKeeperSPAAngular.Controllers
{

    [Produces("application/json")]
    [Route("api/Book")]
    public class BookKeeperController : Controller
    {
        private readonly IBookKeeperRepository _repository;
        public BookKeeperController(IBookKeeperRepository context)
        {
            _repository = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBook()
        {
            int userid = 1;
            return Json(await _repository.GettBooksByUser(userid));

        }


        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbookKeeper([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BookKeeper book = await _repository.GetbookKeeperByID(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }



        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookKeeper([FromRoute] int id, [FromBody] BookKeeper book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookKeeperId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdatebookKeeperByID(book);
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

        // PUT: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBookKeeper([FromRoute] int id, [FromBody] BookKeeper book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookKeeperId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.RemovebookKeeper(book);
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
