using System;
using DiUI.UI;
using Zenject;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

namespace DiUI.Managers
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private readonly MenuButton _menuButton;
        private readonly MainFlowCoordinator _mainFlowCoordinator;
        private readonly DiUIFlowCoordinator _diUIFlowCoordinator;

        public MenuButtonManager(MainFlowCoordinator mainFlowCoordinator, DiUIFlowCoordinator diUIFlowCoordinator)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _diUIFlowCoordinator = diUIFlowCoordinator;
            _menuButton = new MenuButton("DiUI", SummonFlowCoordinator);
        }

        public void Initialize()
        {
            MenuButtons.instance.RegisterButton(_menuButton);
        }

        private void SummonFlowCoordinator()
        {
            _mainFlowCoordinator.PresentFlowCoordinator(_diUIFlowCoordinator, animationDirection: HMUI.ViewController.AnimationDirection.Vertical);
        }

        public void Dispose()
        {
            if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
            {
                MenuButtons.instance.UnregisterButton(_menuButton);
            }   
        }
    }
}