using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        PagedList<Account> GetAccountsForOwner(Guid ownerId, AccountParameters accountParameters);
        IEnumerable<Account> AccountsByOwner(Guid ownerId);
    }
}
