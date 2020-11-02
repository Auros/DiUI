using HMUI;
using Zenject;
using UnityEngine;
using IPA.Utilities;
using DiUI.Managers;
using Screen = HMUI.Screen;
using BeatSaberMarkupLanguage;
using System.Collections.Generic;
using DiUI.Models;

namespace DiUI.UI
{
    internal class DiUIFlowCoordinator : FlowCoordinator
    {
        private Screen _mainScreen;
        private DiUIChildView _childView;
        private DiUIManagerView _managerView;
        private EditModeManager _editModeManager;
        private DiUIEditorController _editorController;
        private DiUIInstructionsView _instructionsView;
        private MainFlowCoordinator _mainFlowCoordinator;
        private MainMenuViewController _mainMenuViewController;

        private readonly List<DiButton> _initialCachedButtonData = new List<DiButton>();

        [Inject]
        public void Construct(DiUIChildView childView, DiUIManagerView managerView, EditModeManager editModeManager, DiUIEditorController editorController, DiUIInstructionsView instructionsView, MainFlowCoordinator mainFlowCoordinator, MainMenuViewController mainMenuViewController)
        {
            _childView = childView;
            _managerView = managerView;
            _editModeManager = editModeManager;
            _editorController = editorController;
            _instructionsView = instructionsView;
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
            _editorController.ExitEditRequested += DeactivateEditMode;
            _editModeManager.ButtonReleased += ButtonWasReleased;
            _managerView.EditModeRequested += ActivateEditMode;
            _editModeManager.ButtonGrabbed += ButtonWasGrabbed;
        }

        private void ButtonWasReleased(GameObject button)
        {
            _childView.ShowSettingsForButton(button);
        }

        private void ButtonWasGrabbed(GameObject button)
        {
            _childView.ShowSettingsForButton(button);
            if (!_childView.isInViewControllerHierarchy)
            {
                SetRightScreenViewController(_childView, ViewController.AnimationType.In);
            }
            _childView.ShowSettingsForButton(button);
        }

        private void DeactivateEditMode()
        {
            showBackButton = true;
            ReplaceTopViewController(_managerView, ShowView, animationType: ViewController.AnimationType.Out);
            SetBottomScreenViewController(null, ViewController.AnimationType.Out);
            SetRightScreenViewController(null, ViewController.AnimationType.Out);
            SetLeftScreenViewController(null, ViewController.AnimationType.Out);
        }

        private void ActivateEditMode()
        {
            showBackButton = false;
            ReplaceTopViewController(_mainMenuViewController, ShowEdit, animationType: ViewController.AnimationType.In);
            SetBottomScreenViewController(_editorController, ViewController.AnimationType.In);
            SetLeftScreenViewController(_instructionsView, ViewController.AnimationType.In);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            _editorController.ExitEditRequested -= DeactivateEditMode;
            _editModeManager.ButtonReleased -= ButtonWasReleased;
            _managerView.EditModeRequested -= ActivateEditMode;
            _editModeManager.ButtonGrabbed -= ButtonWasGrabbed;
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            
            _mainFlowCoordinator.DismissFlowCoordinator(this, RepairMainMenu, animationDirection: ViewController.AnimationDirection.Vertical);
        }

        protected void RepairMainMenu()
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