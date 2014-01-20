using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Reporting
{
    public class ReadModelFacade
    {
        protected readonly IReportingRepository _repo;

        public ReadModelFacade(IReportingRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<T> GetAll<T>() where T:class
        {
            return _repo.GetItems<T>();
        }

        public T GetById<T>(Guid id) where T : class
        {
            return _repo.GetById<T>(id);
        }
    }
}
