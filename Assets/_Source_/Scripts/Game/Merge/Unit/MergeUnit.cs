namespace MiniIT.GAME.MERGE
{
    using System;
    using ENTITIES;
    using SETTINGS;
    using UnityEngine;
    using UnityEngine.Pool;
    using UtilsModule.Execute;
    using UtilsModule.Execute.Interfaces;
    using UtilsModule.Other;

    public class MergeUnit : MonoBehaviour, IExecuteHolder
    {
        private const int START_LEVEL = 1;

        [SerializeField] private Transform grabPoint;
        [SerializeField] private Transform viewContainer;

        private Tank           currentTank;
        private TankDataHolder tankDataHolder;
        private DContainer     container;

        public static ObjectPool<MergeUnit> Pool { get; private set; }
        public static int MaxLevel { get; private set; }

        public int CurrentLevel { get; private set; }
        public Transform GrabPoint => grabPoint;
        public Tank CurrentTank => currentTank;
        public DContainer Container => container;

        public ExecuteMethod Method => ExecuteMethod.Awake;
        public int Priority { get; set; }

        public static void InitPool(MergeUnit prefab)
        {
            GameObject parent = new GameObject($"{nameof(MergeUnit)}:POOL");

            Pool = new ObjectPool<MergeUnit>(
                createFunc: () =>
                {
                    prefab.gameObject.SetActive(false);
                    MergeUnit unit = Instantiate(prefab, parent.transform);
                    prefab.gameObject.SetActive(true);
                    return unit;
                },
                actionOnRelease: item => item.gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            if (tankDataHolder == null)
            {
                return;
            }

            currentTank = tankDataHolder.GetTankByLevel(CurrentLevel);
            currentTank.transform.SetParent(viewContainer);
            currentTank.transform.localPosition = Vector3.zero;
            currentTank.transform.localRotation = Quaternion.identity;

            Container.Register(currentTank);
        }

        private void OnDisable()
        {
            Container.Release<Tank>();
            tankDataHolder.ReleaseTank(currentTank, CurrentLevel);
        }

        public Executor GetExecutor()
        {
            return new ExecutorSync(c =>
            {
                container = c;
                container.Register(this);
                tankDataHolder = GlobalData.Container.Resolve<TankDataHolder>();

                OnEnable();
            });
        }

        public void SetLevel(int level)
        {
            if (level < START_LEVEL)
            {
                throw new ArgumentException($"Level must be less than START_LEVEL:({START_LEVEL})");
            }

            CurrentLevel = level;

            if (MaxLevel < level)
            {
                MaxLevel = level;
            }
        }
    }
}