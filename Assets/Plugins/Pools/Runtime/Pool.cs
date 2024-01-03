using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pools.Runtime
{
    [System.Serializable]
    public class Pool<T> where T: Object
    {
        [SerializeField] private T _source;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _max = 100;
        [SerializeField] private bool _collectionCheck;
        private ObjectPool<T> _pool;
        public int CountAll => _pool.CountAll;
        public int CountActive => _pool.CountInactive;
        public int CountInactive => _pool.CountInactive;
        private static readonly Dictionary<T, Pool<T>> _prefabLookUp = new ();
        private static readonly Dictionary<T, Pool<T>> _instanceLookUp = new ();
        
        public Pool()
        {
            _pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, _collectionCheck, _defaultCapacity, _max);
        }

        public Pool(T prefab, int defaultCapacity = 10, int max = 100, bool collectionCheck = false)
        {
            _source = prefab;
            _defaultCapacity = defaultCapacity;
            _max = max;
            _collectionCheck = collectionCheck;
            _pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, _collectionCheck, _defaultCapacity, _max);
        }

        public void Init()
        {
            _pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, _collectionCheck, _defaultCapacity, _max);
        }

        public static Pool<T> CreatePrefabPool(T prefab, int defaultCapacity = 10, int max = 100, bool collectionCheck = false)
        {
            if(!_prefabLookUp.TryGetValue(prefab, out var pool))
            {
                pool = new Pool<T>(prefab, defaultCapacity,max,collectionCheck);
                _prefabLookUp.Add(prefab, pool);
                return pool;
            }
            
            Debug.LogWarning($"Prefab Pool {prefab.name} already create");
            return null;
        }
        
        public static Pool<T> GetPool(T prefab)
        {
            if (_prefabLookUp.TryGetValue(prefab, out var pool)) return pool;
            if (_instanceLookUp.TryGetValue(prefab, out pool)) return pool;
            return null;
        }

        public T Get()
        {
            return _pool.Get();
        }
        
        public T Get(Vector3 position, Quaternion rotation)
        {
            var obj = Get();
            obj.GameObject().transform.SetPositionAndRotation(position, rotation);
            return obj;
        }
        

        public void Release(T obj)
        {
            _pool.Release(obj);
        }

        public void Clear()
        {
            _pool.Clear();
            if (_prefabLookUp.TryGetValue(_source, out var pool))
            { 
                _prefabLookUp.Remove(_source);
            }
            
            foreach(var item in _instanceLookUp.Where(kvp => kvp.Value == this).ToList())
            {
                _instanceLookUp.Remove(item.Key);
            }
        }

        private void ActionOnDestroy(T obj)
        {
            if(obj == null) return;
            _instanceLookUp.Remove(obj);
            Object.Destroy(obj);
        }

        private void ActionOnRelease(T obj)
        {
            if (obj is IPoolable iPool)
            {
                iPool.OnRelease();
            }
            obj.GameObject().SetActive(false);
        }

        private void ActionOnGet(T obj)
        {
            obj.GameObject().SetActive(true);
            if (obj is IPoolable iPool)
            {
                iPool.OnGet();
            }
        }

        private T CreateFunc()
        {
            if (_source != null)
            {
                var obj = Object.Instantiate(_source);
                _instanceLookUp.Add(obj, this);
                return obj;
            }

            Debug.LogWarning("Source cannot be null");
            return null;
        }
    }
}