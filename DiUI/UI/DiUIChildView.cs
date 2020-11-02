using HMUI;
using TMPro;
using Zenject;
using System.IO;
using System.Linq;
using UnityEngine;
using IPA.Utilities;
using DiUI.Managers;
using System.Threading;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine.UI;

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
        private ButtonSpriteSwap _spriteSwap;
        private CancellationTokenSource _cts;

        [Inject]
        protected MenuButtonCacher buttonCacher;

        [Inject]
        protected CachedMediaAsyncLoader mediaLoader;

        [UIParams]
        protected BSMLParserParams parserParams;

        [UIValue("sprite-list")]
        public List<object> spriteList = new List<object>();

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

        private string _normalImage;
        [UIValue("normal-image")]
        protected string NormalImage
        {
            get => _normalImage;
            set
            {
                _normalImage = value;
                NotifyPropertyChanged();
                LoadNormal(_normalImage);
            }
        }

        private string _selectedImage;
        [UIValue("selected-image")]
        protected string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                NotifyPropertyChanged();
                LoadSelected(_selectedImage);
            }
        }

        protected async void LoadNormal(string image)
        {
            var file = Path.Combine(UnityGame.UserDataPath, "Di", "UI", "Textures", image);
            var sprite = await mediaLoader.LoadSpriteAsync(file, _cts.Token);
            sprite.texture.wrapMode = TextureWrapMode.Clamp;
            if (_spriteSwap != null)
            {
                _spriteSwap.SetField("_normalStateSprite", sprite);
                var images = _spriteSwap.GetField<Image[], ButtonSpriteSwap>("_images");
                if (images.Length > 0)
                {
                    var imgs = images.ToList();
                    var bounds = (_spriteSwap.gameObject.transform as RectTransform).sizeDelta;
                    if (sprite.texture.width > sprite.texture.height)
                    {
                        imgs.ForEach(x => (x.rectTransform.parent.transform as RectTransform).sizeDelta = new Vector2(bounds.x, bounds.y * ((float)sprite.texture.width / sprite.texture.height)));
                    }
                    else if (sprite.texture.height > sprite.texture.width)
                    {
                        imgs.ForEach(x => (x.rectTransform.parent.transform as RectTransform).sizeDelta = new Vector2(bounds.x * ((float)sprite.texture.height / sprite.texture.width), bounds.y));
                    }
                }
                _button.gameObject.SetActive(!_button.gameObject.activeInHierarchy);
                _button.gameObject.SetActive(!_button.gameObject.activeInHierarchy);
            }
            
        }

        protected async void LoadSelected(string image)
        {
            var file = Path.Combine(UnityGame.UserDataPath, "Di", "UI", "Textures", image);
            var sprite = await mediaLoader.LoadSpriteAsync(file, _cts.Token);
            sprite.texture.wrapMode = TextureWrapMode.Clamp;
            if (_spriteSwap != null)
            {
                _spriteSwap.SetField("_highlightStateSprite", sprite);
                _button.gameObject.SetActive(!_button.gameObject.activeInHierarchy);
                _button.gameObject.SetActive(!_button.gameObject.activeInHierarchy);
            }
        }

        protected void Start()
        {
            var dir = new DirectoryInfo(Path.Combine(UnityGame.UserDataPath, "Di", "UI", "Textures"));
            if (!dir.Exists)
            {
                dir.Create();
            }
            var spriteNames = dir.GetFiles();
            spriteList.Add("Default");
            spriteList.AddRange(spriteNames.Select(x => x.Name as object));
        }

        internal void ShowSettingsForButton(GameObject button)
        {
            _gameObject = button;
            _button = button.GetComponent<NoTransitionsButton>();
            _spriteSwap = button.GetComponent<ButtonSpriteSwap>();
            _imageView = button.GetComponentInChildren<ImageView>();
            _textMesh = button.GetComponentInChildren<TextMeshProUGUI>(true);
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

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            _cts = new CancellationTokenSource();
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            _cts?.Cancel();
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }
    }
}