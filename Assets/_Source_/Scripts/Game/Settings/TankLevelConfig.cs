namespace MiniIT.GAME.Settings
{
    using System;

    [Serializable]
    public struct TankLevelConfig
    {
        public int Level;
        public float DeadRewardMultiplier;
        public float TakeDamageRewardMultiplier;
        public float DamageMultiplier;
        public float HealthMultiplier;
    }
}