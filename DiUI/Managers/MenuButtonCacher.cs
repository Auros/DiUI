using HMUI;
using Zenject;
using UnityEngine;
using IPA.Utilities;
using System.Collections.Generic;

namespace DiUI.Managers
{
    internal class MenuButtonCacher : IInitializable
    {
        private readonly MenuChildManager _menuChildManager;
        private readonly MainMenuViewController _mainMenuViewController;
        private readonly Dictionary<string, SpritePair> _savedSpriteDict = new Dictionary<string, SpritePair>();

        internal MenuButtonCacher(MenuChildManager menuChildManager, MainMenuViewController mainMenuViewController)
        {
            _menuChildManager = menuChildManager;
            _mainMenuViewController = mainMenuViewController;
        }

        public async void Initialize()
        {
            await SiraUtil.Utilities.PauseChamp;
            var buttons = _mainMenuViewController.GetComponentsInChildren<NoTransitionsButton>();
            foreach (var button in buttons)
            {
                var buttonName = button.gameObject.name;
                var swapper = button.gameObject.GetComponent<ButtonSpriteSwap>();
                if (swapper != null)
                {
                    var spritePair = new SpritePair(swapper.GetField<Sprite, ButtonSpriteSwap>("_normalStateSprite"), swapper.GetField<Sprite, ButtonSpriteSwap>("_highlightStateSprite"));
                    _savedSpriteDict.Add(buttonName, spritePair);
                }
            }
            _menuChildManager.UpdateButtons(_mainMenuViewController.gameObject);
        }

        internal struct SpritePair
        {
            public Sprite normal;
            public Sprite hovered;

            public SpritePair(Sprite normal, Sprite hovered)
            {
                this.normal = normal;
                this.hovered = hovered;
            }
        }
    }
}