using HMUI;
using TMPro;
using DiUI.Models;
using System.Linq;
using UnityEngine;

namespace DiUI.Managers
{
    internal class MenuChildManager
    {
        private readonly Config _config;

        public MenuChildManager(Config config)
        {
            _config = config;
        }

        internal DiButton GetButton(string source, string name, GameObject gameObject)
        {
            var button = _config.DiButtons.FirstOrDefault(x => x.Source == source && x.Name == name);
            if (button == null)
            {
                button = new DiButton
                {
                    Name = name,
                    Source = source,
                    NormalImage = "Default",
                    SelectedImage = "Default",
                };
                if (gameObject != null)
                {
                    button.Rotation = (int)gameObject.transform.localRotation.eulerAngles.z;
                    button.Position = gameObject.transform.localPosition;
                    
                    var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                    {
                        button.HasText = true;
                        button.ShowText = text.isActiveAndEnabled;
                        button.TextPosition = text.gameObject.transform.localPosition;
                    }
                    var image = gameObject.GetComponentInChildren<ImageView>();
                    if (image != null)
                    {
                        button.Skew = image.skew;
                    }
                }
            }
            _config.DiButtons.Add(button);
            return button;
        } 
    }
}