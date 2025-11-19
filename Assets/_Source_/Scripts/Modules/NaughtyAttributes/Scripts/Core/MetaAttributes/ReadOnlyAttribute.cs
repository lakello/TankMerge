using System;

namespace NaughtyAttributes.Core.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : MetaAttribute
    {

    }
}
