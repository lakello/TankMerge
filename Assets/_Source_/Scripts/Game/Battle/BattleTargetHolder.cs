namespace MiniIT.GAME.Battle
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using UnityEngine;
    using UtilsModule.Disposable;
    using UtilsModule.Execute;
    using UtilsModule.Execute.Interfaces;
    using UtilsModule.Other;
    using ZLinq;
    using Random = UnityEngine.Random;

    public class BattleTargetHolder : MonoBehaviour, IExecuteHolder
    {
        [SerializeField] private Transform[] enemySpawnPoints;
        [SerializeField] private Transform[] friendsSpawnPoints;

        private Dictionary<Fraction, TankSpawnData[]> targets;

        public event Action<Fraction> UnitAdded = delegate { };
        public event Action<Fraction> UnitRemoved = delegate { };

        public ExecuteMethod Method => ExecuteMethod.Awake;
        public int Priority { get; set; }

        public Executor GetExecutor()
        {
            return new ExecutorSync(Init);
        }

        private void Init()
        {
            GlobalData.Container.Register(this).DisposeOnExitScene(gameObject.scene.name);

            targets = new Dictionary<Fraction, TankSpawnData[]>()
            {
                [Fraction.Friends] = friendsSpawnPoints.AsValueEnumerable()
                    .Select(p => new TankSpawnData()
                    {
                        Point = p,
                    })
                    .ToArray(),
                [Fraction.Enemy] = enemySpawnPoints.AsValueEnumerable()
                    .Select(p => new TankSpawnData()
                    {
                        Point = p,
                    })
                    .ToArray(),
            };
        }

        public int GetEmptyCount(Fraction fraction)
        {
            return targets[fraction].AsValueEnumerable().Count(d => d.Tank == null);
        }

        public Health GetRandomTargetHealth(Fraction fraction)
        {
            if (targets.TryGetValue(fraction, out TankSpawnData[] spawnData))
            {
                TankSpawnData[] validSpawnData = spawnData.AsValueEnumerable().Where(d => d.Tank != null).ToArray();

                if (validSpawnData.Length == 0)
                {
                    return null;
                }
                
                int index = Random.Range(0, validSpawnData.Length);

                return validSpawnData[index].Tank.Health;
            }

            return null;
        }

        public bool TryAddUnit(Fraction fraction, Func<TankSpawnData> canAddCallback)
        {
            if (targets.TryGetValue(fraction, out TankSpawnData[] fractionSpawnData)
                && fractionSpawnData.AsValueEnumerable().All(s => s.Tank != null))
            {
                return false;
            }
            
            TankSpawnData data = canAddCallback();

            TankSpawnData tankSpawnData = fractionSpawnData.AsValueEnumerable().First(d => d.Tank == null);

            tankSpawnData.Level = data.Level;
            tankSpawnData.Removed = data.Removed;
            tankSpawnData.Tank = data.Tank;
            tankSpawnData.Tank.transform.position = tankSpawnData.Point.position;
            tankSpawnData.Tank.transform.rotation = tankSpawnData.Point.rotation;
            tankSpawnData.Tank.Dead += RemoveTank;

            UnitAdded(fraction);

            return true;
        }

        private void RemoveTank(Tank tank)
        {
            tank.Dead -= RemoveTank;
            tank.Attaker.StopAttack();

            if (targets.TryGetValue(tank.SelfFraction, out TankSpawnData[] fractionSpawnData))
            {
                TankSpawnData tankSpawnData = fractionSpawnData.AsValueEnumerable().First(d => d.Tank == tank);
                tankSpawnData.Removed?.Invoke(tankSpawnData);
                tankSpawnData.Tank = null;

                UnitRemoved(tank.SelfFraction);
            }
        }

        private void OnDisable()
        {
            foreach (TankSpawnData[] fractionData in targets.Values.AsValueEnumerable())
            {
                foreach (TankSpawnData data in fractionData)
                {
                    if (data != null && data.Tank != null)
                    {
                        data.Tank.Dead -= RemoveTank;
                    }
                }
            }
        }
    }
}