using NaughtyAttributes.Core.DrawerAttributes;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    //[CreateAssetMenu(fileName = "TestScriptableObjectB", menuName = "LeadTools.NaughtyAttributes/TestScriptableObjectB")]
    public class _TestScriptableObjectB : ScriptableObject
    {
        [MinMaxSlider(0, 10)]
        public Vector2Int slider;
    }
}