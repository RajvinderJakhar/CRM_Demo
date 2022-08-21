using Microsoft.EntityFrameworkCore;
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
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            var _result = _context.Set<T>();
            //Dispose();
            return _result;
        }

        public T Add(T item)
        {
            _context.Set<T>().Add(item);
            CommitChanges();
            //Dispose();
            return item;
        }

        public T Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            CommitChanges();
            //Dispose();
            return item;
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var item = GetAll(predicate).FirstOrDefault();
            if (item == null) return;
            _context.Set<T>().Remove(item);
            CommitChanges();
            //Dispose();
        }

        public void DeleteAll(Expression<Func<T, bool>> predicate)
        {
            var items = GetAll(predicate);
            foreach (var item in items)
            {
                _context.Set<T>().Remove(item);
            }
            CommitChanges();
            //Dispose();
        }

        private void CommitChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
