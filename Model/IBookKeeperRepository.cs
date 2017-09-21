using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public interface IBookKeeperRepository
    {
        Task AddbookKeeper(BookKeeper bookKeeper);
        Task<BookKeeper> bookKeeperExists(int bookKeeperid);
        Task<BookKeeper> GetbookKeeperByID(int id);
        Task<List<BookKeeperViewModel>> GettBooksByUser(string userid);
        Task RemovebookKeeper(BookKeeper bookKeeper);
        Task UpdatebookKeeperByID(BookKeeper bookKeeper);
    }
}