namespace MiniIT.GAME.Battle
{
    using UnityEngine;

    public class Tower : MonoBehaviour
    {
        [SerializeField]                   private float rotateSpeed    = 5f;
        [SerializeField, Range(0.1f, 10f)] private float angleThreshold = 2f;

        public bool IsLook(Vector3 target)
        {
            Vector3 targetDirection = target - transform.position;
            targetDirection.y = 0f;

            if (targetDirection.sqrMagnitude == 0f)
            {
                return true;
            }

            Vector3 forward = transform.forward;
            forward.y = 0f;

            float angle = Vector3.Angle(forward, targetDirection);

            return angle <= angleThreshold;
        }

        public void Look(Vector3 target)
        {
            Vector3 targetDirection = target - transform.position;
            targetDirection.y = 0f;

            if (targetDirection.sqrMagnitude == 0f)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}