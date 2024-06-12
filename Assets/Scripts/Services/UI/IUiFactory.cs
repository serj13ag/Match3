using UI;
using UI.Windows;

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
        void Cleanup();
    }
}