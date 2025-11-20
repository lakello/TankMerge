namespace miniit.GAME.MERGE.ITEM
{
	using Entities;
	using Settings;
	using TMPro;
	using UnityEngine;
	using UnityEngine.Animations;
	using UtilsModule.Other;

	public class MergeItemView : MonoBehaviour
	{
		[SerializeField]
		private LookAtConstraint lookAtConstraint;
		[SerializeField]
		private TMP_Text levelOutput;
		[SerializeField]
		private MergeItem mergeItem;
		[SerializeField]
		private Transform viewContainer;

		private Item currentItem;
		
		private void Awake()
		{
			lookAtConstraint.gameObject.SetActive(false);
			lookAtConstraint.worldUpObject = Camera.main.transform;
		}

		private void OnEnable()
		{
			lookAtConstraint.gameObject.SetActive(true);
			levelOutput.text = mergeItem.CurrentLevel.ToString();

			currentItem = DI.Resolve<ItemsConfig>().GetItemByLevel(mergeItem.CurrentLevel);
			currentItem.transform.SetParent(viewContainer);
			currentItem.transform.localPosition = Vector3.zero;
			currentItem.transform.localRotation = Quaternion.identity;
		}

		private void OnDisable()
		{
			DI.Resolve<ItemsConfig>().ReleaseItem(currentItem, mergeItem.CurrentLevel);
			lookAtConstraint.gameObject.SetActive(false);
		}
	}
}