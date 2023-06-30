using UI;
using UnityEngine;

namespace Services.UI
{
    public interface IUiFactory
    {
        void CreateUiRootCanvas();
        MessageWindow GetMessageWindow();
        MainMenu GetMainMenu();
        LevelsWindow GetLevelsWindow();
        LevelButton GetLevelButton(Transform parentTransform);
        void Cleanup();
    }
}