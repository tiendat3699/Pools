namespace Pools.Runtime
{
    public interface IPoolable
    {
        void OnGet();
        void OnRelease();
    }
}