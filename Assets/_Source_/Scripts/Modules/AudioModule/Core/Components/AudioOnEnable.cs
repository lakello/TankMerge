using AudioModule.Extensions;
using UnityEngine;

namespace AudioModule
{
    public class AudioOnEnable : MonoBehaviour
    {
        [SerializeField] private AudioID _id;
        
        private void OnEnable()
        {
            _id.PlayOneShot();
        }
    }
}