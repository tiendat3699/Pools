using UnityEngine;

namespace Pools.Runtime
{
    public static class Utilities
    {
        public static GameObject GameObject(this Object obj)
        {
            if (obj is GameObject)
            {
                return (GameObject)obj;
            }
            
            if (obj is Component component)
            {
                return component.gameObject;
            }
            
            return null;
        }
    }
}