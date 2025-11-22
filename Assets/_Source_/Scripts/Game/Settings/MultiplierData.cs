namespace MiniIT.GAME.Settings
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MultiplierData
    {
        [SerializeField] private Vector2        range;
        [SerializeField] private AnimationCurve curve;

        public float Get(int level, Vector2 levelRange)
        {
            float levelF = level;

            float t = levelRange.x == levelRange.y
                ? 0f
                : Mathf.InverseLerp(levelRange.x, levelRange.y, levelF);

            t = Mathf.Clamp01(t);

            float curveValue = curve.Evaluate(t);

            return Mathf.Lerp(range.x, range.y, curveValue);
        }
    }
}