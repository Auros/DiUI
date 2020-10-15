using System;
using Zenject;
using UnityEngine.UI;
using IPA.Utilities;
using System.Threading.Tasks;
using System.Threading;
using SiraUtil;
using HMUI;
using UnityEngine;
using Polyglot;

namespace DiUI.Managers
{
    public class MainMenuParentee : IInitializable, IDisposable
    {
        private readonly Config _config;
        private readonly MainMenuViewController _mainMenuViewController;

        private Button _soloButton;
        private Button _quitButton;
        private Button _partyButton;
        private Button _optionsButton;
        private Button _campaignButton;
        private Button _howToPlayButton;
        private Button _multiplayerButton;
        private Button _beatmapEditorButton;

        public MainMenuParentee(Config config, MainMenuViewController mainMenuViewController)
        {
            _config = config;
            _mainMenuViewController = mainMenuViewController;
        }

        public async void Initialize()
        {
            _soloButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_soloButton");
            _quitButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_quitButton");
            _partyButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_partyButton");
            _optionsButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_optionsButton");
            _campaignButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_campaignButton");
            _howToPlayButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_howToPlayButton");
            _multiplayerButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_multiplayerButton");
            _beatmapEditorButton = _mainMenuViewController.GetField<Button, MainMenuViewController>("_beatmapEditorButton");

            _config.Reloaded += Config_Reloaded;

            await Utilities.PauseChamp;

            MoveButtons();
            //Dab();
        }

        private void Config_Reloaded(Config config)
        {
            Plugin.Log.Info("The config has reloaded!");
            MoveButtons();
        }

        private void MoveButtons()
        {
            foreach (var child in _config.Children)
            {
                if (child.Name == "Solo")
                {
                    _soloButton.transform.localScale = child.Scale;
                    _soloButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Exit")
                {
                    _quitButton.transform.localScale = child.Scale;
                    _quitButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Party")
                {
                    _partyButton.transform.localScale = child.Scale;
                    _partyButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Options")
                {
                    _optionsButton.transform.localScale = child.Scale;
                    _optionsButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Campaign")
                {
                    _campaignButton.transform.localScale = child.Scale;
                    _campaignButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Help")
                {
                    _howToPlayButton.transform.localScale = child.Scale;
                    _howToPlayButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Online")
                {
                    _multiplayerButton.transform.localScale = child.Scale;
                    _multiplayerButton.transform.localPosition = child.Position;
                }
                if (child.Name == "Editor")
                {
                    _beatmapEditorButton.transform.localScale = child.Scale;
                    _beatmapEditorButton.transform.localPosition = child.Position;
                }
            }
        }

        private void Dab()
        {
            var soloSwapper = _soloButton.GetComponent<ButtonSpriteSwap>();

            var soloNormal = soloSwapper.GetField<Sprite, ButtonSpriteSwap>("_normalStateSprite");

            Swap(_quitButton, soloNormal);
            Swap(_partyButton, soloNormal);
            Swap(_optionsButton, soloNormal);
            Swap(_campaignButton, soloNormal);
            Swap(_howToPlayButton, soloNormal);
            Swap(_multiplayerButton, soloNormal);
            Swap(_beatmapEditorButton, soloNormal);

        }

        private void Swap(Button btn, Sprite sprite)
        {
            var bss = btn.GetComponent<ButtonSpriteSwap>();
            bss.SetField("_normalStateSprite", sprite);
        }

        public void Dispose()
        {
            _config.Reloaded -= Config_Reloaded;
        }
    }
}