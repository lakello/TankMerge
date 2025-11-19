using System.Collections.Generic;
using NaughtyAttributes.Core.DrawerAttributes;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    //[CreateAssetMenu(fileName = "TestScriptableObjectA", menuName = "LeadTools.NaughtyAttributes/TestScriptableObjectA")]
    public class _TestScriptableObjectA : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectB> listB;
    }
}