using System.Collections.Generic;
using NaughtyAttributes.Core.DrawerAttributes;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    //[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "LeadTools.NaughtyAttributes/_NaughtyScriptableObject")]
    public class _NaughtyScriptableObject : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectA> listA;
    }
}
