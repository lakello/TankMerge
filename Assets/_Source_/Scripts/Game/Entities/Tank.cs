namespace MiniIT.GAME.Entities
{
    using System;
    using Battle;
    using R3;
    using UnityEngine;
    using UtilsModule.Extensions;

    public class Tank : MonoBehaviour
    {
        [SerializeField] private Attaker attaker;
        [SerializeField] private Health  health;
        
        private Fraction selfFraction;
        private CompositeDisposable disposable = null;
        
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

        private void OnEnable()
        {
            disposable = new CompositeDisposable();
            health.Current
                .Subscribe(OnHealthChanged)
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable?.Dispose();
        }

        public void Init(BattleData data)
        {
            health.Init(data.MaxHealth);
            attaker.StartAttack(data);
            selfFraction = data.SelfFraction;
        }

        private void OnHealthChanged(float value)
        {
            if (value <= 0)
            {
                Dead(this);
            }
        }
    }
}