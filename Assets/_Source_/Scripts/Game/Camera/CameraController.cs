namespace MiniIT.GAME.Camera
{
    using Sirenix.OdinInspector;
    using Unity.Cinemachine;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineClearShot clearShot;
        [SerializeField] private CinemachineCamera    mergeCamera;
        [SerializeField] private CinemachineCamera    fightCamera;

        [Button]
        public void OnFightChange(bool isFight)
        {
            if (isFight)
            {
                SwitchToCamera(fightCamera);
            }
            else
            {
                SwitchToCamera(mergeCamera);
            }
        }

        private void SwitchToCamera(CinemachineCamera targetCam)
        {
            if (targetCam == null)
            {
                return;
            }

            int activePriority = clearShot.Priority + 1;

            foreach (var cam in clearShot.ChildCameras)
            {
                cam.Priority = (cam == targetCam)
                    ? activePriority
                    : 0;
            }
        }
    }
}