namespace MiniIT.GAME.Entities
{
    using System;
    using AudioModule;
    using AudioModule.Extensions;
    using Battle;
    using MessageModule;
    using Particles;
    using PLAYER.Messages;
    using R3;
    using Settings;
    using UnityEngine;
    using UnityEngine.Pool;
    using UtilsModule.Extensions;

    public class Tank : MonoBehaviour
    {
        [SerializeField] private Attaker attaker;
        [SerializeField] private Health  health;
        [SerializeField] private float   takeDamageReward;
        [SerializeField] private float   deadReward;

        private CompositeDisposable disposable = null;
        private TankLevelConfig     tankLevelConfig;
        private Fraction            selfFraction;

        public event Action<Tank> Dead = delegate { };

        public Fraction SelfFraction => selfFraction;

        public Health Health => health;
        public Attaker Attaker => attaker;
        public ReactiveProperty<int> Level { get; } = new ReactiveProperty<int>(0);

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
            Level.Value = tankLevelConfig.Level;
            health.Init(config.HealthMultiplier);
            attaker.Init(config.DamageMultiplier);
        }

        public void Run(BattleData data)
        {
            health.Activate();
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
                ObjectPool<ParticleSystem> pool = EffectHolder.Instance.GetPool(EffectType.Merge);
                ParticleSystem particle = pool.Get();
                particle.transform.position = transform.position;
                pool.ReleaseParticle(particle).Forget();
                
                AudioID.Dead.PlayOneShot();

                Dead(this);
            }
        }
    }
}