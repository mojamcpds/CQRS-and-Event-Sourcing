using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace CQRS.Reporting
{
    public class ReportingUnitOfWork:IReportingUnitOfWork
    {
        ReportingDbContext _context = new ReportingDbContext();

        public ReportingUnitOfWork()
        {
            
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Save<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                _context.Set<T>().Add(entity);
            }
        }

        public void Update<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _context.Set<T>().Attach(entity);
                _context.Set<T>().Remove(entity);
            }
        }

        public T GetById<T>(Guid id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

    }
}
