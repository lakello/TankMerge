namespace UtilsModule.Other
{
    using UnityEngine;

    public static class GlobalData
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Container = new DContainer();
        }

        public static DContainer Container { get; private set; }
    }
}