using System;
using System.Collections.Generic;
using System.Linq;

namespace Fittify.DataModelRepositories.Helpers
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

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count(); // database call to get count
            List<T> items = null;
            try
            {
                items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ToList(); // database call to get entities
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
