using HMUI;
using System;
using Zenject;
using System.Linq;
using UnityEngine;
using VRUIControls;
using IPA.Utilities;

namespace DiUI.Managers
{
    internal class EditModeManager : IInitializable, ITickable, IDisposable
    {
        private Vector2 _relativeVector;
        private VRController _cachedVRController;

        private readonly VRPointer _vrPointer;
        private readonly VRInputModule _vrInputModule;
        private readonly IVRPlatformHelper _vrPlatformHelper;
        private readonly MainMenuViewController _mainMenuViewController;

        private GameObject _activeGameObject;

        public EditModeManager(VRInputModule vrInputModule, IVRPlatformHelper vrPlatformHelper, MainMenuViewController mainMenuViewController)
        {
            _vrInputModule = vrInputModule;
            _vrPlatformHelper = vrPlatformHelper;
            _mainMenuViewController = mainMenuViewController;

            _vrPointer = _vrInputModule.GetField<VRPointer, VRInputModule>("_vrPointer");
        }

        public void Initialize()
        {
            _vrInputModule.onProcessMousePressEvent += Press;
            _vrPlatformHelper.joystickWasNotCenteredThisFrameEvent += Scrolling;
        }

        private void Press(GameObject objectHit)
        {
            // Find The Actual Button GameObject
            _activeGameObject = objectHit.GetComponentInParent<NoTransitionsButton>()?.gameObject;
            _cachedVRController = _vrPointer.vrController;
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                _mainMenuViewController.GetComponentsInChildren<NoTransitionsButton>().ToList().ForEach(x => x.interactable = !x.interactable);
            }
            if (_activeGameObject != null)
            {
                if (_relativeVector != Vector2.zero)
                {
                    _activeGameObject.transform.localPosition +=  new Vector3(_relativeVector.x, _relativeVector.y, 0) * 3;
                    _relativeVector = Vector2.zero;
                }
                if ((_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.Unknown && Input.GetMouseButtonUp(0)) || (_vrPlatformHelper.currentXRDeviceModel != XRDeviceModel.Unknown && 0.5f > _cachedVRController.triggerValue) || _vrPointer.vrController != _cachedVRController)
                {
                    _relativeVector = Vector2.zero;
                    _activeGameObject = null;
                }
            }
        }

        public void Dispose()
        {
            _vrInputModule.onProcessMousePressEvent -= Press;
            _vrPlatformHelper.joystickWasNotCenteredThisFrameEvent -= Scrolling;
        }
    }
}