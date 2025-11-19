using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UtilsModule.TransitionSystem
{
	public class Transition : MonoBehaviour
    {
        public TransitionSettings transitionSettings;
        
        public Transform transitionPanelIN;
        public Transform transitionPanelOUT;

        public CanvasScaler transitionCanvas;

        private CanvasGroup transitionIn;
        private CanvasGroup transitionOut;
        private SizeDeltaAnimator _animatorIn;
        private SizeDeltaAnimator _animatorOut;

        private void Awake()
        {
            transitionIn = Instantiate(transitionSettings.transitionIn, transitionPanelIN).AddComponent<CanvasGroup>();
            transitionIn.blocksRaycasts = transitionSettings.blockRaycasts;
            _animatorIn = transitionIn.GetComponentInChildren<SizeDeltaAnimator>();

            transitionOut = Instantiate(transitionSettings.transitionOut, transitionPanelOUT).AddComponent<CanvasGroup>();
            transitionOut.blocksRaycasts = transitionSettings.blockRaycasts;
            _animatorOut = transitionOut.GetComponentInChildren<SizeDeltaAnimator>();
        }

        private void Start()
        {
            Disable();
        }

        public async UniTask InPlay()
        {
            Play(transitionPanelIN, transitionIn);
            await _animatorIn.Animate();
        }

        public async UniTask OutPlay()
        {
            Play(transitionPanelOUT, transitionOut);
            await _animatorOut.Animate();
        }

        public void Disable()
        {
            transitionPanelIN.gameObject.SetActive(false);
            transitionPanelOUT.gameObject.SetActive(false);
        }

        public void Play(Transform panel, CanvasGroup group)
        {
            transitionCanvas.referenceResolution = transitionSettings.refrenceResolution;
            
            transitionPanelIN.gameObject.SetActive(false);
            transitionPanelOUT.gameObject.SetActive(false);

            panel.gameObject.SetActive(true);
        }
    }
}