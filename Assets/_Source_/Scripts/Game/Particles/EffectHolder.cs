namespace MiniIT.GAME.PARTICLES
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Pool;
    using UtilsModule.Singleton;
    using ZLinq;

    public class EffectHolder : SingletonMono<EffectHolder>
    {
        [SerializeField] private Data[] data;

        private Dictionary<EffectType, ObjectPool<ParticleSystem>> pools;
        private Transform                                          parent;

        [Serializable]
        private class Data
        {
            [HideLabel]
            [HorizontalGroup]
            public EffectType Type;
            [HideLabel]
            [HorizontalGroup]
            public ParticleSystem ParticlePrefab;
        }

        public ObjectPool<ParticleSystem> GetPool(EffectType type)
        {
            parent ??= new GameObject("Effects:POOL").transform;
            pools ??= data.AsValueEnumerable().ToDictionary(d => d.Type, d => new ObjectPool<ParticleSystem>(
                createFunc: () => Instantiate(d.ParticlePrefab, parent),
                actionOnGet: system => system.gameObject.SetActive(true),
                actionOnRelease: system => system.gameObject.SetActive(false)));

            return pools[type];
        }
    }
}