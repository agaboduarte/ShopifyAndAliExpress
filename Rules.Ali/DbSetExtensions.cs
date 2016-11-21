using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public static class DbSetExtensions
    {
        public static IQueryable<TEntity> Include<TEntity>(this DbSet<TEntity> dbSet, params string[] includes)
            where TEntity : class
        {
            var returnValue = (DbQuery<TEntity>) dbSet;

            if (includes != null && includes.Any())
            {
                includes = includes.Distinct(StringComparer.CurrentCulture).ToArray();

                foreach (var item in includes)
                {
                    returnValue = returnValue.Include(item);
                }
            }

            return returnValue;
        }
    }
}
