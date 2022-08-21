using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Utility
{
    /// <summary>
    /// Genric repository for CRUD
    /// </summary>
    /// <author>RAJVINDER JAKHAR</author>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> : IDisposable
    {
        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        T Add(T item);

        T Update(T item);

        void Delete(Expression<Func<T, bool>> predicate);

        void DeleteAll(Expression<Func<T, bool>> predicate);
    }
}
