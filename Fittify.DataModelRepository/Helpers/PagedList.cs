using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Helpers
{
    public class PagedList<T> : List<T>, IPagedList
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        /// <summary>
        /// CreateAsync a pagedList of an LinqToEntity query
        /// </summary>
        /// <param name="items">List of entities returned from context</param>
        /// <param name="count">Total count of available entities in context (equals SQL count)</param>
        /// <param name="pageNumber">The sequence of entities being queried</param>
        /// <param name="pageSize">The "number of entites per sequence" being queried</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            PagedList<T> pagedList = null;

            await Task.Run(() =>
            {
                var count = source.Count(); // database call to get count
                var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ToList(); // database call to get entities
                pagedList = new PagedList<T>(items, count, pageNumber, pageSize);
            });

            return pagedList;
        }
    }
}
