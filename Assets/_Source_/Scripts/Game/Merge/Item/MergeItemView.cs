namespace miniit.GAME.MERGE.ITEM
{
	using TMPro;
	using UnityEngine;
	using UnityEngine.Animations;

	public class MergeItemView : MonoBehaviour
	{
		[SerializeField]
		private LookAtConstraint lookAtConstraint;
		[SerializeField]
		private TMP_Text levelOutput;
		[SerializeField]
		private MergeItem mergeItem;

		private void Awake()
		{
			lookAtConstraint.gameObject.SetActive(false);
			lookAtConstraint.worldUpObject = Camera.main.transform;
		}

		private void OnEnable()
		{
			lookAtConstraint.gameObject.SetActive(true);
			levelOutput.text = mergeItem.CurrentLevel.ToString();
		}

		private void OnDisable()
		{
			lookAtConstraint.gameObject.SetActive(false);
		}
	}
}