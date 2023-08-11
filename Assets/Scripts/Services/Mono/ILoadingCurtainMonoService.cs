namespace Services.Mono
{
    public interface ILoadingCurtainMonoService : IService
    {
        void FadeOnInstantly();
        void FadeOffWithDelay();
    }
}