namespace MiniIT.GAME.Battle
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct BattleData
    {
        public float    MaxHealth;
        public Fraction SelfFraction;
        public Fraction TargetFraction;
    }
}