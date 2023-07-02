using UI;
using UnityEngine;

namespace Services.UI
{
    public interface IUiFactory
    {
        void CreateUiRootCanvas();
        MessageWindow GetMessageWindow();
        MainMenu CreateMainMenu();
        SettingsWindow CreateSettingsWindow();
        LevelsWindow CreateLevelsWindow();
        LevelButton CreateLevelButton(Transform parentTransform);
        void Cleanup();
    }
}