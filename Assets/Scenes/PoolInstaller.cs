using Pools.Runtime;
using UnityEngine;

public class PoolInstaller : MonoBehaviour
{
    public PoolContainer<GameObject> PoolContainer;
    

    void Start()
    {
        //create
        PoolContainer.Create();
    }

    private void OnDestroy()
    {
        //clear
        PoolContainer.Clear();
    }
}
