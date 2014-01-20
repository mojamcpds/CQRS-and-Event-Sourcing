using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Reporting
{
    public interface IReadModelFacade
    {
        IEnumerable<T> GetAll<T>() where T : class;
        T GetById<T>(Guid id) where T : class;
    }
}
