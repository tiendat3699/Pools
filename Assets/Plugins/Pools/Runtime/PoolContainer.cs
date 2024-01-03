using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pools.Runtime
{
    [Serializable]
    public class PoolContainer<T> where T: Object
    {
        [SerializeField] private T _prefab;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _max = 100;
        [SerializeField] private bool _collectionCheck;
        [NonSerialized] public Pool<T> Pool;

        public void Create()
        {
            Pool = Pool<T>.CreatePrefabPool(_prefab, _defaultCapacity, _max, _collectionCheck);
        }
        
        public void Clear()
        {
            Pool.Clear();
        }
    }
}