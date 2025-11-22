namespace UtilsModule.Other
{
    using UnityEngine;

    public class MonoContainer : MonoBehaviour
    {
        private readonly DContainer _container = new DContainer();

        public DContainer Container => _container;

        private void OnDestroy()
        {
            _container.Dispose();
        }
    }
}