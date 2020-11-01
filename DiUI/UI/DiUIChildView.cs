using HMUI;
using TMPro;
using UnityEngine;
using IPA.Utilities;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace DiUI.UI
{
    [ViewDefinition("DiUI.Views.child-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\child-view.bsml")]
    internal class DiUIChildView : BSMLAutomaticViewController
    {
        private ImageView _imageView;
        private GameObject _gameObject;
        private TextMeshProUGUI _textMesh;
        private NoTransitionsButton _button;

        [UIParams]
        protected BSMLParserParams parserParams;

        private string _editingChildTitle = "Editing Child";
        [UIValue("editing-child-title")]
        protected string EditingChildTitle
        {
            get => _editingChildTitle;
            set
            {
                _editingChildTitle = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("pos-x")]
        protected float PosX
        {
            get => _gameObject.transform.localPosition.x;
            set => _gameObject.transform.localPosition = new Vector2(value, _gameObject.transform.localPosition.y);
        }

        [UIValue("pos-y")]
        protected float PosY
        {
            get => _gameObject.transform.localPosition.y;
            set => _gameObject.transform.localPosition = new Vector2(_gameObject.transform.localPosition.x, value);
        }

        [UIValue("rot")]
        protected float Rot
        {
            get => _gameObject.transform.localRotation.eulerAngles.z;
            set => _gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, value));
        }

        [UIValue("text-pos-x")]
        protected float TextPosX
        {
            get => _textMesh != null ? _textMesh.gameObject.transform.localPosition.x : 0;
            set => _textMesh.gameObject.transform.localPosition = new Vector2(value, _textMesh.gameObject.transform.localPosition.y);
        }

        [UIValue("text-pos-y")]
        protected float TextPosY
        {
            get => _textMesh != null ? _textMesh.gameObject.transform.localPosition.y : 0;
            set => _textMesh.gameObject.transform.localPosition = new Vector2(_textMesh.gameObject.transform.localPosition.x, value);
        }

        [UIValue("text-visible")]
        protected bool TextVisible
        {
            get => _textMesh != null && _textMesh.gameObject.activeInHierarchy;
            set => _textMesh.gameObject.SetActive(value);
        }

        private bool _textOptions = false;
        [UIValue("text-options")]
        protected bool TextOptions
        {
            get => _textOptions;
            set
            {
                _textOptions = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("skew")]
        protected float Skew
        {
            get => _imageView.skew - 0.11f;
            set
            {
                _imageView.SetField("_skew", value + 0.11f);
                _imageView.SetAllDirty();
            }
        }

        private bool _show = true;
        [UIValue("show")]
        protected bool Show
        {
            get => _show;
            set
            {
                _show = value;
                NotifyPropertyChanged();
            }
        }

        internal void ShowSettingsForButton(GameObject button)
        {
            _gameObject = button;
            _button = button.GetComponent<NoTransitionsButton>();
            _imageView = button.GetComponentInChildren<ImageView>();
            _textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
            EditingChildTitle = $"Editing <color=#27cf9f>{button.name}</color>";
            
            parserParams?.EmitEvent("get");

            if (_textMesh != null)
            {
                TextOptions = true;
                parserParams?.EmitEvent("get-text");
            }
            else
            {
                TextOptions = false;
            }
        }
    }
}