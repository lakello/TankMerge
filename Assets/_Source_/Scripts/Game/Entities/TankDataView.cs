namespace MiniIT.GAME.ENTITIES
{
    using Doozy.Runtime.UIManager.Components;
    using R3;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Animations;

    public class TankDataView : MonoBehaviour
    {
        [SerializeField] private LookAtConstraint lookAtConstraint;
        [SerializeField] private TMP_Text         levelOutput;
        [SerializeField] private UISlider         healthOutput;
        [SerializeField] private Tank             tank;

        private CompositeDisposable disposable = null;

        private void Awake()
        {
            lookAtConstraint.gameObject.SetActive(false);
            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = Camera.main.transform;
            source.weight = 1;
            lookAtConstraint.AddSource(source);
        }

        private void OnEnable()
        {
            lookAtConstraint.gameObject.SetActive(true);
            lookAtConstraint.constraintActive = true;
            levelOutput.text = tank.Level.ToString();

            disposable = new CompositeDisposable();

            tank.Health.Current
                .Subscribe(_ => OnHealthChanged())
                .AddTo(disposable);

            tank.Health.IsActive
                .Subscribe(OnHealthActiveChanged)
                .AddTo(disposable);

            tank.Level
                .Subscribe(v => levelOutput.text = v.ToString())
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            lookAtConstraint.constraintActive = false;
            lookAtConstraint.gameObject.SetActive(false);
            disposable?.Dispose();
        }

        private void OnHealthChanged()
        {
            healthOutput.value = tank.Health.Current.Value / tank.Health.Max;
        }

        private void OnHealthActiveChanged(bool isActive)
        {
            healthOutput.gameObject.SetActive(isActive);
        }
    }
}