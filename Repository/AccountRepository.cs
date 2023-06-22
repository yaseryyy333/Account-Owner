using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Helpers;
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
        private ISortHelper<Account> _accountSortHelper;
        private IDataShaper<Account> _accountDataShaper;
        public AccountRepository(RepositoryContext repositoryContext,
            ISortHelper<Account> accountSortHelper,
            IDataShaper<Account> accountDataShaper
            ) 
            : base(repositoryContext)
        {
            _accountSortHelper = accountSortHelper;
            _accountDataShaper= accountDataShaper;
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
