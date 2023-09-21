using UI;
using UI.Windows;
using UnityEngine;

namespace Services.UI
{
    public interface IUiFactory : IService
    {
        void CreateUiRootCanvas();
        MessageInGameWindow GetMessageWindow();
        MainMenu CreateMainMenu();
        BackgroundBlocker CreateBackgroundBlocker();
        BaseWindow CreateShopWindow();
        SettingsWindow CreateSettingsWindow();
        PuzzleLevelsWindow CreatePuzzleLevelsWindow();
        PuzzleLevelButton CreateLevelButton(Transform parentTransform);
        void Cleanup();
    }
}