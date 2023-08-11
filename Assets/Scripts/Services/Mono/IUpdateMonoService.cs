using Interfaces;

namespace Services.Mono
{
    public interface IUpdateMonoService : IService
    {
        void Init();
        void Register(IUpdatable updatable);
        void Remove(IUpdatable updatable);
    }
}