﻿using HMUI;
using System;
using Zenject;
using System.Linq;
using UnityEngine;
using VRUIControls;
using IPA.Utilities;

namespace DiUI.Managers
{
    internal class EditModeManager : ITickable
    {
        private Vector2 _relativeVector;
        private VRController _cachedVRController;

        private readonly Config _config;
        private readonly VRPointer _vrPointer;
        private readonly VRInputModule _vrInputModule;
        private readonly IVRPlatformHelper _vrPlatformHelper;
        private readonly MainMenuViewController _mainMenuViewController;

        private GameObject _activeGameObject;

        internal event Action<GameObject> ButtonGrabbed;
        internal event Action<GameObject> ButtonReleased;

        private bool _enabled;
        internal bool Enabled
        {
            get => _enabled;
            private set
            {
                _enabled = value;
                ClearSelection();
            }
        }

        public EditModeManager(Config config, VRInputModule vrInputModule, IVRPlatformHelper vrPlatformHelper, MainMenuViewController mainMenuViewController)
        {
            _config = config;
            _vrInputModule = vrInputModule;
            _vrPlatformHelper = vrPlatformHelper;
            _mainMenuViewController = mainMenuViewController;

            _vrPointer = _vrInputModule.GetField<VRPointer, VRInputModule>("_vrPointer");
        }

        public void Enable()
        {
            _vrInputModule.onProcessMousePressEvent += Press;
            _vrPlatformHelper.joystickWasNotCenteredThisFrameEvent += Scrolling;
            _config.EditorMoveSensitivity += 0;
            Enabled = true;
        }

        private void Press(GameObject objectHit)
        {
            // Find The Actual Button GameObject
            
            var buttonRoot = objectHit.GetComponentInParent<NoTransitionsButton>()?.gameObject;
            if (buttonRoot != null && buttonRoot.transform.parent.name == "MainButtons")
            {
                _activeGameObject = buttonRoot;
                _cachedVRController = _vrPointer.vrController;
                ButtonGrabbed?.Invoke(_activeGameObject);
            }
        }

        private void Scrolling(Vector2 relativeVector)
        {
            if (_activeGameObject != null)
            {
                _relativeVector = relativeVector;
            }
        }

        public void Tick()
        {
            if (!Enabled)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                _mainMenuViewController.GetComponentsInChildren<NoTransitionsButton>().ToList().ForEach(x => x.interactable = !x.interactable);
            }
            if (_activeGameObject != null)
            {
                if (_relativeVector != Vector2.zero)
                {
                    _activeGameObject.transform.localPosition +=  new Vector3(_relativeVector.x, _relativeVector.y, 0) * _config.EditorMoveSensitivity;
                    _relativeVector = Vector2.zero;
                }
                if ((_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.Unknown && Input.GetMouseButtonUp(0)) || (_vrPlatformHelper.currentXRDeviceModel != XRDeviceModel.Unknown && 0.5f > _cachedVRController.triggerValue) || _vrPointer.vrController != _cachedVRController)
                {
                    ClearSelection();
                }
            }
        }

        private void ClearSelection()
        {
            if (_activeGameObject != null)
            {
                ButtonReleased?.Invoke(_activeGameObject);
            }
            _relativeVector = Vector2.zero;
            _activeGameObject = null;
        }

        public void Disable()
        {
            Enabled = false;
            _vrInputModule.onProcessMousePressEvent -= Press;
            _vrPlatformHelper.joystickWasNotCenteredThisFrameEvent -= Scrolling;
        }
    }
}