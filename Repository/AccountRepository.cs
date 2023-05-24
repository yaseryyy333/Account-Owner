using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public PagedList<Account> GetAccountsForOwner(Guid ownerId, AccountParameters accountParameters)
        {
            return PagedList<Account>.ToPagedList(FindByCondition(a =>a.OwnerId == ownerId).OrderBy(on => on.AccountType),
                accountParameters.PageNumber,
                accountParameters.PageSize);
        }

        public IEnumerable<Account> AccountsByOwner(Guid ownerId)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
        }
    }
}
