namespace MiniIT.GAME.UI
{
    using AudioModule;
    using AudioModule.Extensions;
    using Doozy.Runtime.UIManager;
    using Doozy.Runtime.UIManager.Components;
    using UnityEngine;

    public class DoozyAudioButton : MonoBehaviour
    {
        [SerializeField] private AudioID id;
        
        private UIButton button;

        private UIButton Button => button ??= GetComponent<UIButton>();

        private void OnEnable()
        {
            Button.OnSelectionStateChangedCallback.AddListener(OnClick);
        }

        private void OnDisable()
        {
            Button.OnSelectionStateChangedCallback.RemoveListener(OnClick);
        }

        private void OnClick(UISelectionState selectionState)
        {
            if (selectionState == UISelectionState.Pressed)
            {
                id.PlayOneShot();
            }
        }
    }
}