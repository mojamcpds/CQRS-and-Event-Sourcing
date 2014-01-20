using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Reporting
{
    public class ReportingRepository:IReportingRepository
    {
        protected IReportingUnitOfWork _uow;

        public ReportingRepository(IReportingUnitOfWork uow)
        {
            _uow = uow;
        }

        public T GetById<T>(Guid id) where T : class
        {
            return _uow.GetById<T>(id);
        }

        public void Add<T>(T item) where T : class
        {
            _uow.Save<T>(item);
            _uow.SaveChanges();
        }

        public void Modify<T>(T item) where T : class
        {
            _uow.Update<T>(item);
            _uow.SaveChanges();
        }

        public void Delete<T>(Guid id) where T : class
        {

            var item = this.GetById<T>(id);
            _uow.Delete<T>(item);
            _uow.SaveChanges();
        }


        public List<T> GetItems<T>() where T : class
        {
            return _uow.Query<T>().ToList();
        } 
    }
}
