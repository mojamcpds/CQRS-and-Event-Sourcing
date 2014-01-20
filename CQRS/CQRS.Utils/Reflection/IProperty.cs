using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Utils.Reflection
{
    public interface IProperty
    {
        string Name { get; }
        object GetValue(object obj, object[] index);
        void SetValue(object obj, object val, object[] index);
        Type PropertyType { get; }
    }
}
