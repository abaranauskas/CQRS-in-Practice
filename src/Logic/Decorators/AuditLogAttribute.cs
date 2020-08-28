using System;

namespace Logic.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AuditLogAttribute : Attribute
    {
        public AuditLogAttribute()
        {

        }
    }
}
