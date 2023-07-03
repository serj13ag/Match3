using System;
using Constants;
using Enums;
using UI;
using UI.Windows;
using UnityEngine;

namespace Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;
        private readonly IAssetProviderService _assetProviderService;

        private BackgroundBlocker _backgroundBlocker;

        public WindowService(IUiFactory uiFactory, IAssetProviderService assetProviderService)
        {
            _uiFactory = uiFactory;
            _assetProviderService = assetProviderService;
        }

        public void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback)
        {
            ShowMessage(onButtonClickCallback, AssetPaths.GoalIconSpritePath, $"score goal\n{scoreGoal}", "start");
        }

        public void ShowGameWinMessageWindow(Action onButtonClickCallback)
        {
            ShowMessage(onButtonClickCallback, AssetPaths.WinIconSpritePath, "you win!", "ok");
        }

        public void ShowGameOverMessageWindow(Action onButtonClickCallback)
        {
            ShowMessage(onButtonClickCallback, AssetPaths.LoseIconSpritePath, "you lose!", "ok");
        }

        public void ShowWindow(WindowType windowType)
        {
            ShowBackgroundBlocker();

            BaseWindow baseWindow = windowType switch
            {
                WindowType.Levels => _uiFactory.CreateLevelsWindow(),
                WindowType.Settings => _uiFactory.CreateSettingsWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowType), windowType, null),
            };

            baseWindow.Show();

            baseWindow.OnHided += OnWindowHided;
        }

        private void OnWindowHided(object sender, EventArgs e)
        {
            BaseWindow baseWindow = (BaseWindow)sender;
            baseWindow.OnHided -= OnWindowHided;

            _backgroundBlocker.Deactivate();
        }

        private void ShowMessage(Action onButtonClickCallback, string iconSpritePath, string message, string buttonText)
        {
            ShowBackgroundBlocker();

            MessageInGameWindow messageWindow = _uiFactory.GetMessageWindow();
            Sprite goalIcon = _assetProviderService.LoadSprite(iconSpritePath);
            messageWindow.ShowMessage(goalIcon, message, buttonText, onButtonClickCallback);

            messageWindow.OnHided += OnWindowHided;
        }

        private void ShowBackgroundBlocker()
        {
            if (_backgroundBlocker == null)
            {
                _backgroundBlocker = _uiFactory.CreateBackgroundBlocker();
            }

            _backgroundBlocker.Activate();
        }
    }
}