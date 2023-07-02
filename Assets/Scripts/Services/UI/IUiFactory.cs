using UI;
using UI.Windows;
using UnityEngine;

namespace Services.UI
{
    public interface IUiFactory
    {
        void CreateUiRootCanvas();
        MessageInGameWindow GetMessageWindow();
        MainMenu CreateMainMenu();
        SettingsWindow CreateSettingsWindow();
        LevelsWindow CreateLevelsWindow();
        LevelButton CreateLevelButton(Transform parentTransform);
        void Cleanup();
    }
}