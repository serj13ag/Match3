using UI;

namespace Services.UI
{
    public interface IUiFactory
    {
        void CreateUiRootCanvas();
        MessageWindow GetMessageWindow();
    }
}