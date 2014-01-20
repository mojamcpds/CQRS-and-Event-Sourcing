using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Reporting
{
    public interface IReportingUnitOfWork
    {
        IQueryable<T> Query<T>() where T : class;
        void Save<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        T GetById<T>(Guid id) where T : class;
        void SaveChanges();
    }
}
