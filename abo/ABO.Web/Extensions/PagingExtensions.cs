using ABO.Core;
using MvcContrib.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Extensions
{
    public static class PagingExtensions
    {
        /// <summary>
        /// Converts from Core.IPagedList to MvcContrib.IPagination.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IPagination<TModel> ToMvcPaging<TEntity, TModel>(this IPagedList<TEntity> pagedList, IEnumerable<TModel> data)
        {
            return new CustomPagination<TModel>(data, pagedList.PageIndex, pagedList.PageSize, pagedList.TotalCount);
        }
    }
}