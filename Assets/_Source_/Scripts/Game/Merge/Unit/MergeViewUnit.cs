namespace MiniIT.GAME.MERGE.UNIT
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Animations;

    public class MergeUnitView : MonoBehaviour
    {
        [SerializeField] private LookAtConstraint lookAtConstraint;
        [SerializeField] private TMP_Text         levelOutput;

        private void Awake()
        {
            lookAtConstraint.gameObject.SetActive(false);
            lookAtConstraint.worldUpObject = Camera.main.transform;
        }

        public void Enable(int level)
        {
            lookAtConstraint.gameObject.SetActive(true);
            levelOutput.text = level.ToString();
        }

        public void Disable()
        {
            lookAtConstraint.gameObject.SetActive(false);
        }
    }
}