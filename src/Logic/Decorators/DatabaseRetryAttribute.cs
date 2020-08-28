using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DatabaseRetryAttribute : Attribute
    {
        public DatabaseRetryAttribute()
        {

        }
    }
}
