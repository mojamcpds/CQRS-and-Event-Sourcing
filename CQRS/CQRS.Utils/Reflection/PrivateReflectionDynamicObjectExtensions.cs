using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Utils.Reflection
{
    public static class PrivateReflectionDynamicObjectExtensions
    {
        public static dynamic AsDynamic(this object o)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }
    }
}
