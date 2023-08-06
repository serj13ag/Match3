using Interfaces;

namespace Services.Mono
{
    public interface IUpdateMonoService
    {
        void Init();
        void Register(IUpdatable updatable);
        void Remove(IUpdatable updatable);
    }
}