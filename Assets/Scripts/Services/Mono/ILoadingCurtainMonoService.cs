using System;

namespace Services.Mono
{
    public interface ILoadingCurtainMonoService : IService
    {
        void FadeOnInstantly();
        void FadeOffWithDelay(Action onFadeEndedCallback = null);
    }
}