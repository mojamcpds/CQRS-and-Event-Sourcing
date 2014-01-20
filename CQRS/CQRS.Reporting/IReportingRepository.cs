using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Reporting
{
    public interface IReportingRepository
    {
        T GetById<T>(Guid id) where T : class;
        void Add<T>(T item) where T : class;
        void Delete<T>(Guid id) where T : class;
        List<T> GetItems<T>() where T : class;
        void Modify<T>(T item) where T : class;
    }
}
