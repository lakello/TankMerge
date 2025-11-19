using System;

namespace NaughtyAttributes.Core.DrawerAttributes_SpecialCase
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReorderableListAttribute : SpecialCaseDrawerAttribute
    {
    }
}
