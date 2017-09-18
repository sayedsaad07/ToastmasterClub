using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class BookKeeperRepository : IBookKeeperRepository
    {
        private readonly BookKeeperContext _context;
        public BookKeeperRepository(BookKeeperContext context)
        {
            _context = context;
        }
        public async Task<List<BookKeeperViewModel>> GettBooksByUser(int userid)
        {
            List<BookKeeperViewModel> bookKeeperList = new List<BookKeeperViewModel>();
            var datalist = await (from b in _context.BookKeeper
                                  where b.Userid == userid
                                  select new { b.BookKeeperId, b.BookName, b.BookUrl, b.Category, b.Rating, b.Startreading, b.StopReading }
                                  ).ToListAsync();
            datalist.ForEach(bk =>
            {
                var b = new BookKeeperViewModel
                {
                    BookKeeperId = bk.BookKeeperId,
                    BookName = bk.BookName,
                    BookUrl = bk.BookUrl,
                    Category = bk.Category,
                    Rating = bk.Rating,
                    Startreading = bk.Startreading,
                    StopReading = bk.StopReading
                };
                b.StopReading = bk.StopReading;
                bookKeeperList.Add(b);
            }
            );
            return bookKeeperList;
        }



        public async Task<BookKeeper> GetbookKeeperByID(int id)
        {
            return await _context.BookKeeper.SingleOrDefaultAsync(m => m.BookKeeperId == id);
        }

        public async Task<BookKeeper> bookKeeperExists(int bookKeeperid)
        {
            return await _context.BookKeeper.SingleOrDefaultAsync(m => m.BookKeeperId == bookKeeperid);
        }
        public async Task UpdatebookKeeperByID(BookKeeper bookKeeper)
        {
            _context.Entry(bookKeeper).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddbookKeeper(BookKeeper book)
        {
            _context.BookKeeper.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task RemovebookKeeper(BookKeeper bookKeeper)
        {

            _context.BookKeeper.Remove(bookKeeper);
            await _context.SaveChangesAsync();

        }
    }
}
