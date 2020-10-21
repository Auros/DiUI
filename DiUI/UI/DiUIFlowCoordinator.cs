using BeatSaberMarkupLanguage;
using HMUI;
using IPA.Utilities;
using SiraUtil;
using Zenject;

namespace DiUI.UI
{
    internal class DiUIFlowCoordinator : FlowCoordinator
    {
        private Screen _mainScreen;
        private DiUIManagerView _managerView;
        private DiUIEditorController _editorController;
        private MainFlowCoordinator _mainFlowCoordinator;
        private MainMenuViewController _mainMenuViewController;

        [Inject]
        public void Construct(DiUIManagerView managerView, DiUIEditorController editorController, MainFlowCoordinator mainFlowCoordinator, MainMenuViewController mainMenuViewController)
        {
            _managerView = managerView;
            _editorController = editorController;
            _mainFlowCoordinator = mainFlowCoordinator;
            _mainMenuViewController = mainMenuViewController;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            ShowView();
            if (addedToHierarchy)
            {
                ProvideInitialViewControllers(_managerView);
            }
            _mainScreen = _mainMenuViewController.screen;
            _managerView.EditModeRequested += ActivateEditMode;
            _editorController.ExitEditRequested += DeactivateEditMode;
        }

        private void DeactivateEditMode()
        {
            showBackButton = true;
            ReplaceTopViewController(_managerView, ShowView, animationType: ViewController.AnimationType.Out);
            SetBottomScreenViewController(null, ViewController.AnimationType.None);
            SetRightScreenViewController(null, ViewController.AnimationType.None);
            SetLeftScreenViewController(null, ViewController.AnimationType.None);
        }

        private void ActivateEditMode()
        {
            showBackButton = false;
            ReplaceTopViewController(_mainMenuViewController, ShowEdit, animationType: ViewController.AnimationType.In);
            SetBottomScreenViewController(_editorController, ViewController.AnimationType.In);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            _editorController.ExitEditRequested -= DeactivateEditMode;
            _managerView.EditModeRequested -= ActivateEditMode;

            base.BackButtonWasPressed(topViewController);
            
            _mainFlowCoordinator.DismissFlowCoordinator(this, ReturnOwnershipOfMainMenu, animationDirection: ViewController.AnimationDirection.Vertical);
        }

        protected void ReturnOwnershipOfMainMenu()
        {
            _mainMenuViewController.SetField<ViewController, Screen>("_screen", _mainScreen);
        }

        protected void ShowView()
        {
            SetTitle("View Mode");
            showBackButton = true;
        }

        protected void ShowEdit()
        {
            SetTitle("Edit Mode");
            showBackButton = false;
        }
    }
}