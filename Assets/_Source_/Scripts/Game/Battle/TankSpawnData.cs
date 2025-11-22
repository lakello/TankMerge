namespace MiniIT.GAME.Battle
{
    using System;
    using Entities;
    using UnityEngine;

    public class TankSpawnData
    {
        public Transform             Point;
        public Tank                  Tank;
        public Fraction              Fraction;
        public int                   Level;
        public Action<TankSpawnData> Removed;
    }
}