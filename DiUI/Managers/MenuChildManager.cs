using HMUI;
using TMPro;
using DiUI.Models;
using System.Linq;
using UnityEngine;
using IPA.Utilities;
using System.Collections.Generic;

namespace DiUI.Managers
{
    internal class MenuChildManager
    {
        private readonly Config _config;

        public MenuChildManager(Config config)
        {
            _config = config;
        }

        internal DiButton GetButton(string source, string name, GameObject gameObject, bool forceReload = false)
        {
            var button = _config.DiButtons.FirstOrDefault(x => x.Source == source && x.Name == name);
            if (forceReload || button == null)
            {
                if (!forceReload)
                {
                    button = new DiButton
                    {
                        Name = name,
                        Source = source,
                        NormalImage = "Default",
                        SelectedImage = "Default",
                    };
                }
                if (gameObject != null)
                {
                    var mainButton = gameObject.GetComponentsInChildren<NoTransitionsButton>().FirstOrDefault(x => x.gameObject.name == name);
                    if (mainButton != null)
                    {
                        button.Rotation = (int)mainButton.transform.localRotation.eulerAngles.z;
                        button.Position = mainButton.transform.localPosition;

                        Plugin.Log.Info(name);
                        Plugin.Log.Info(mainButton.transform.localPosition.ToString());

                        var text = mainButton.GetComponentInChildren<TextMeshProUGUI>();
                        if (text != null)
                        {
                            button.HasText = true;
                            button.ShowText = text.isActiveAndEnabled;
                            button.TextPosition = text.gameObject.transform.localPosition;
                        }
                        var image = mainButton.GetComponentInChildren<ImageView>();
                        if (image != null)
                        {
                            button.Skew = image.skew;
                        }
                    }
                }
                _config.DiButtons.Add(button);
            }
            return button;
        }

        internal void UpdateButtons(GameObject source, bool forceReload = false)
        {
            var diButtons = new List<DiButton>();
            var buttons = source.GetComponentsInChildren<NoTransitionsButton>();
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    var diButton = GetButton(source.name, button.gameObject.name, source, forceReload);
                    diButtons.Add(diButton);

                    button.transform.localPosition = diButton.Position;
                    button.transform.localRotation = Quaternion.Euler(button.transform.localRotation.eulerAngles.x, button.transform.localRotation.eulerAngles.y, diButton.Rotation);
                    var text = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.transform.localPosition = diButton.TextPosition;
                        text.gameObject.SetActive(diButton.ShowText);
                    }
                    var image = button.GetComponentInChildren<ImageView>();
                    if (image != null)
                    {
                        image.SetField("_skew", diButton.Skew);
                        image.SetVerticesDirty();
                    }
                }
            }
            _config.Changed();
        }
    }
}