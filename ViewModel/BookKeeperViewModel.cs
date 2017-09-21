using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class BookKeeperViewModel
    {
        public BookKeeperViewModel() { }
        public int BookKeeperId { get; set; }

        public string BookName { get; set; }

        public string Category { get; set; }

        public DateTime Startreading { get; set; }
        public DateTime StopReading { get; set; }
        public short Rating { get; set; }
        public string BookUrl { get; set; }
        public string UserName { get; set; }
    }
}
