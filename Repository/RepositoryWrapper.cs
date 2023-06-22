using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ISortHelper<Owner> _ownerSortHelper;
        private ISortHelper<Account> _accountSortHelper;
        private RepositoryContext _repositoryContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        private IDataShaper<Owner> _ownerDataShaper;
        private IDataShaper<Account> _accountDataShaper;
        public IOwnerRepository Owner
        {
            get
            {
                if(_owner == null)
                {
                    return _owner = new OwnerRepository(_repositoryContext, _ownerSortHelper, _ownerDataShaper);
                }
                return _owner;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if(_account == null)
                {
                    _account = new AccountRepository(_repositoryContext, _accountSortHelper, _accountDataShaper);
                }
                return _account;
            }
        }


        public RepositoryWrapper(RepositoryContext repositoryContext,
            ISortHelper<Owner> ownerSortHelper,
            ISortHelper<Account> accountSortHelper)
        {
            _repositoryContext = repositoryContext;
            _ownerSortHelper = ownerSortHelper;
            _accountSortHelper = accountSortHelper;
        }
        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
