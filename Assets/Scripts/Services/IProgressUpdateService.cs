using Interfaces;

namespace Services
{
    public interface IProgressUpdateService
    {
        void Register(IProgressWriter progressWriter);
        void UpdateProgressAndSave();
    }
}