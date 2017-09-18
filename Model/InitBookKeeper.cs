using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class InitBookKeeper
    {
        private BookKeeperContext _DBContext;

        public InitBookKeeper(BookKeeperContext dbcontext)
        {
            _DBContext = dbcontext;
        }

        public async Task SeedData()
        {
            if (!_DBContext.BookKeeper.Any())
            {
                _DBContext.BookKeeper.AddRange(
                    new BookKeeper() { BookName = "Entrepreneur book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "Toastmaster book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "4 Hour Week book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "Think fast and slow book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 1, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now },
                    new BookKeeper() { BookName = "Think fast and slow book", Category = "Motivation", BookUrl = "Www.blogpost.com", Rating = 5, Userid = 2, Startreading = DateTime.Now.AddDays(-30), StopReading = DateTime.Now }
                    );
                await _DBContext.SaveChangesAsync();

            }
        }
    }
}
