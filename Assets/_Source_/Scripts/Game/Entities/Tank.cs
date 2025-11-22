namespace MiniIT.GAME.Entities
{
    using System;
    using Battle;
    using MessageModule;
    using PLAYER.Messages;
    using R3;
    using Settings;
    using UnityEngine;
    using UtilsModule.Extensions;

    public class Tank : MonoBehaviour
    {
        [SerializeField] private Attaker attaker;
        [SerializeField] private Health  health;
        [SerializeField] private float   takeDamageReward;
        [SerializeField] private float   deadReward;

        private Fraction            selfFraction;
        private CompositeDisposable disposable = null;
        private TankLevelConfig     tankLevelConfig;

        public event Action<Tank> Dead = delegate { };

        public Fraction SelfFraction => selfFraction;

        public Health Health => health;
        public Attaker Attaker => attaker;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentElseThrow(out attaker);
            gameObject.GetComponentElseThrow(out health);
        }
#endif

        private void OnDisable()
        {
            disposable?.Dispose();
        }

        public void Init(TankLevelConfig config)
        {
            tankLevelConfig = config;
            health.Init(config.HealthMultiplier);
            attaker.Init(config.DamageMultiplier);
        }

        public void Run(BattleData data)
        {
            attaker.StartAttack(data);
            selfFraction = data.SelfFraction;

            disposable?.Dispose();
            disposable = new CompositeDisposable();
            health.Current
                .Subscribe(OnHealthChanged)
                .AddTo(disposable);
        }

        private void OnHealthChanged(float value)
        {
            if (health.IsMax == false)
            {
                new DamageTakedMessage
                {
                    SelfFraction = selfFraction,
                    IsAlive = health.IsAlive,
                    DeadReward = (int)(deadReward * tankLevelConfig.DeadRewardMultiplier),
                    TakeDamageReward = (int)(takeDamageReward * tankLevelConfig.TakeDamageRewardMultiplier),
                }.Publish();
            }

            if (value <= 0)
            {
                Dead(this);
            }
        }
    }
}