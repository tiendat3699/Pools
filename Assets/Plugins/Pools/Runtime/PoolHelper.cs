using Unity.VisualScripting;
using UnityEngine;

namespace Pools.Runtime
{
    public static class PoolHelper
    {
        public static Pool<T> GetPool<T>(this T prefab) where T : Object
        {
            var pool = Pool<T>.GetPool(prefab);
            return pool;
        }

        public static T Get<T>(this T prefab) where T : Object
        {
            var obj = prefab.GetPool().Get();
            return obj;
        }
        
        public static T Get<T>(this T prefab, Transform parent, bool worldPositionStays = true) where T : Object
        {
            var obj = prefab.GetPool().Get();
            obj.GameObject().transform.SetParent(parent, worldPositionStays);
            return obj;
        }
        
        public static T Get<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            var obj = prefab.GetPool().Get();
            obj.GameObject().transform.SetPositionAndRotation(position, rotation);
            return obj;
        }
        
        public static T Get<T>(this T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            var obj = prefab.GetPool().Get();
            obj.GameObject().transform.SetPositionAndRotation(position, rotation);
            obj.GameObject().transform.SetParent(parent);
            return obj;
        }

        public static void Release<T>(this T instance) where T : Object
        {
            instance.GetPool().Release(instance);
        }
    }
}