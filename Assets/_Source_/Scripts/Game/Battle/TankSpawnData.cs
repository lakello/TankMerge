namespace MiniIT.GAME.BATTLE
{
    using System;
    using ENTITIES;
    using UnityEngine;

    public class TankSpawnData
    {
        public Transform             Point;
        public Tank                  Tank;
        public int                   Level;
        public Action<TankSpawnData> Removed;
    }
}