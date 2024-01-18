using UnityEngine;

namespace Pools.Runtime
{
    internal static class Utilities
    {
        public static GameObject GameObject(this Object obj)
        {
            if (obj is GameObject)
            {
                return (GameObject)obj;
            }
            else if (obj is Component component)
            {
                return component.gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}