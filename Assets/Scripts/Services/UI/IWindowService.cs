using System;
using Enums;

namespace Services.UI
{
    public interface IWindowService
    {
        void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback);
        void ShowGameWinMessageWindow(Action onButtonClickCallback);
        void ShowGameOverMessageWindow(Action onButtonClickCallback);

        void ShowWindow(WindowType windowType);
    }
}