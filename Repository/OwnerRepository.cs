﻿using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Helpers;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq.Dynamic.Core;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        private ISortHelper<Owner> _sortHelper;
        private IDataShaper<Owner> _dataShaper;
        public OwnerRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Owner> sortHelper,
            IDataShaper<Owner> dataShaper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }


        public IEnumerable<Owner> GetAllOwners()
        {
            return FindAll()
                .OrderBy(ow => ow.Name)
                .ToList();
        }

        public PagedList<ExpandoObject> GetOwners(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(o => o.BirthOfDay.Year >= ownerParameters.MinYearOfBirth &&
            o.BirthOfDay.Year <= ownerParameters.MaxYearOfBirth);
            //.OrderBy(no => no.Name);

            //SearchByName(ref owner, ownerParameters.Name);
            SearchByName(ref owners, ownerParameters.Name);

            /*var sortedOwners =*/ _sortHelper.ApplySort(owners, ownerParameters.OrderBy);

            var shapedOwners = _dataShaper.ShapeData(owners, ownerParameters.Fields);

            return PagedList<ExpandoObject>.ToPagedList(shapedOwners,
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
            //return PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
            //    ownerParameters.PageNumber,
            //    ownerParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;

            owners = owners.Where(o => o.Name.ToLower().Contains(ownerName.Trim().ToLower()));
        }

        public ExpandoObject GetOwnerById(Guid ownerId, string fields)
        {
            var owner = FindByCondition(owner => owner.Id.Equals(ownerId))
                .DefaultIfEmpty(new Owner())
                .FirstOrDefault();

            return _dataShaper.ShapeData(owner, fields);


        }

        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id.Equals(ownerId))
                .Include(ac => ac.Accounts)
                .FirstOrDefault();
        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        //private void ApplySort(ref IQueryable<Owner> owners, string orderByQueryString)
        //{
        //    if (!owners.Any())
        //        return;
        //    if (string.IsNullOrWhiteSpace(orderByQueryString))
        //    {
        //        owners = owners.OrderBy(x => x.Name);
        //        return;
        //    }
        //    var orderParams = orderByQueryString.Trim().Split(',');
        //    var propertyInfos = typeof(Owner).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    var orderQueryBuilder = new StringBuilder();
        //    foreach (var param in orderParams)
        //    {
        //        if (string.IsNullOrWhiteSpace(param))
        //            continue;
        //        var propertyFromQueryName = param.Split(" ")[0];
        //        var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
        //        if (objectProperty == null)
        //            continue;
        //        var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
        //        orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
        //    }
        //    var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        //    if (string.IsNullOrWhiteSpace(orderQuery))
        //    {
        //        owners = owners.OrderBy(x => x.Name);
        //        return;
        //    }
        //    owners = owners.OrderBy(orderQuery);

        //}
    }
}
