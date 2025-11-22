namespace UtilsModule.Execute
{
    using System;
    using Interfaces;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class GameObjectExecutorsHolder
    {
        [SerializeField] 
        [Required]
        [OnValueChanged("OnValueChanged")]
        private GameObject _obj;
        
        private IExecuteHolder[] _executeHolders;

        public GameObjectExecutorsHolder(GameObject obj)
        {
            _obj = obj;
            OnValueChanged();
        }

        public IExecuteHolder[] Holders => _executeHolders ??= _obj.GetComponentsInChildren<IExecuteHolder>();

        private void OnValueChanged()
        {
            if (_obj != null)
            {
                IExecuteHolder[] holders = _obj.GetComponentsInChildren<IExecuteHolder>();

                if (holders == null || holders.Length == 0)
                {
                    Debug.LogError("ExecuteHolder object is null or empty");
                    _obj = null;
                }
            }
        }
    }
}