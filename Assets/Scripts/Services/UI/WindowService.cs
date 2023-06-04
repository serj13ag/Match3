using System;
using UI;
using UnityEngine;

namespace Services.UI
{
    public class WindowService
    {
        private const string GoalIconSpritePath = "Images/GoalIcon";
        private const string WinIconSpritePath = "Images/WinIcon";
        private const string LoseIconSpritePath = "Images/LoseIcon";

        private readonly UiFactory _uiFactory;
        private readonly AssetProviderService _assetProviderService;

        public WindowService(UiFactory uiFactory, AssetProviderService assetProviderService)
        {
            _uiFactory = uiFactory;
            _assetProviderService = assetProviderService;
        }

        public void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback)
        {
            MessageWindow messageWindow = _uiFactory.GetMessageWindow();
            Sprite goalIcon = _assetProviderService.LoadSprite(GoalIconSpritePath);
            messageWindow.ShowMessage(goalIcon, $"score goal\n{scoreGoal}", "start", onButtonClickCallback);
        }

        public void ShowGameWinMessageWindow(Action onButtonClickCallback)
        {
            MessageWindow messageWindow = _uiFactory.GetMessageWindow();
            Sprite winIcon = _assetProviderService.LoadSprite(WinIconSpritePath);
            messageWindow.ShowMessage(winIcon, "you win!", "ok", onButtonClickCallback);
        }

        public void ShowGameOverMessageWindow(Action onButtonClickCallback)
        {
            MessageWindow messageWindow = _uiFactory.GetMessageWindow();
            Sprite loseIcon = _assetProviderService.LoadSprite(LoseIconSpritePath);
            messageWindow.ShowMessage(loseIcon, "you lose!", "ok", onButtonClickCallback);
        }
    }
}