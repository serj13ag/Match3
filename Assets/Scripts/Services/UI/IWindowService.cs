using System;
using Enums;

namespace Services.UI
{
    public interface IWindowService : IService
    {
        void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback);
        void ShowGameWinMessageWindow(Action onButtonClickCallback, bool showAd = false);
        void ShowGameOverMessageWindow(Action onButtonClickCallback);

        void ShowWindow(WindowType windowType);

        void Cleanup();
    }
}