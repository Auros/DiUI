using System;
using Zenject;
using UnityEngine;
using DiUI.Models;
using DiUI.Managers;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System.Linq;

namespace DiUI.UI
{
    [ViewDefinition("DiUI.Views.editor-controller.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\editor-controller.bsml")]
    internal class DiUIEditorController : BSMLAutomaticViewController
    {
        private Config _config;
        private EditModeManager _editModeManager;
        private MenuChildManager _menuChildManager;
        private MainMenuViewController _mainMenuViewController;
        private readonly List<DiButton> _initialCachedButtonData = new List<DiButton>();
        private readonly Stack<MoveAction> _movementActionStack = new Stack<MoveAction>();
        private readonly Stack<MoveAction> _movementActionQueue = new Stack<MoveAction>();
        internal event Action ExitEditRequested;

        [Inject]
        public void Construct(Config config, EditModeManager editModeManager, MenuChildManager menuChildManager, MainMenuViewController mainMenuViewController)
        {
            _config = config;
            _editModeManager = editModeManager;
            _menuChildManager = menuChildManager;
            _mainMenuViewController = mainMenuViewController;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            _editModeManager.ButtonGrabbed += Editor_ButtonGrabbed;
            _editModeManager.ButtonReleased += Editor_ButtonReleased;
            _movementActionStack.Clear();
            _movementActionQueue.Clear();
            _editModeManager.Enable();

            _initialCachedButtonData.Clear();
            _initialCachedButtonData.AddRange(_config.DiButtons.Copy());
        }

        private void Editor_ButtonGrabbed(GameObject button)
        {
            _movementActionQueue.Clear();
            _movementActionStack.Push(new MoveAction
            {
                subject = button,
                newLocalPosition = button.transform.localPosition,
                originalLocalPosition = button.transform.localPosition
            });
        }

        private void Editor_ButtonReleased(GameObject button)
        {
            var action = _movementActionStack.Peek();
            action.SetNewPosition(button.transform.localPosition);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            _editModeManager.Disable();
            _movementActionQueue.Clear();
            _movementActionStack.Clear();
            _editModeManager.ButtonGrabbed -= Editor_ButtonGrabbed;
            _editModeManager.ButtonReleased -= Editor_ButtonReleased;

            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        [UIAction("exit-edit-mode")]
        protected void ExitEditMode()
        {
            Reset();
            ExitEditRequested?.Invoke();
        }

        [UIAction("undo")]
        protected void Undo()
        {
            if (_movementActionStack.TryPop(out MoveAction action))
            {
                _movementActionQueue.Push(action);
                action.subject.transform.localPosition = action.originalLocalPosition;
            }
        }

        [UIAction("redo")]
        protected void Redo()
        {
            if (_movementActionQueue.TryPop(out MoveAction action))
            {
                _movementActionStack.Push(action);
                action.subject.transform.localPosition = action.newLocalPosition;
            }
        }

        [UIAction("reset")]
        protected void Reset()
        {
            _config.DiButtons = _initialCachedButtonData.ToList();
            _initialCachedButtonData.Clear();
            _menuChildManager.UpdateButtons(_mainMenuViewController.gameObject);
        }

        [UIAction("apply")]
        protected void Apply()
        {
            _initialCachedButtonData.Clear();
            _initialCachedButtonData.AddRange(_config.DiButtons.ToList());
            _menuChildManager.UpdateButtons(_mainMenuViewController.gameObject, true);
            ExitEditMode();
        }

        private class MoveAction
        {
            public GameObject subject;
            public Vector3 newLocalPosition;
            public Vector3 originalLocalPosition;

            public void SetNewPosition(Vector3 vec)
            {
                newLocalPosition = vec;
            }
        }
    }
}