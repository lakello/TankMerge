using System;

namespace NaughtyAttributes.Core.DrawerAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExpandableAttribute : DrawerAttribute
    {
    }
}
