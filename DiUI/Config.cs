using System;
using DiUI.Models;
using IPA.Config.Stores;
using System.Collections.Generic;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DiUI
{
    internal class Config
    {
        internal event Action<Config> Reloaded;

        public virtual float EditorMoveSensitivity { get; set; } = 0.5f;

        [UseConverter(typeof(ListConverter<DiButton>))]
        public List<DiButton> DiButtons { get; set; } = new List<DiButton>();

        public virtual void Changed()
        {
            Reloaded?.Invoke(this);
        }
    }
}