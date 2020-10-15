using UnityEngine;
using SiraUtil.Converters;
using IPA.Config.Stores.Attributes;

namespace DiUI.Models
{
    public class DiChild
    {
        public virtual bool Enabled { get; set; } = false;

        public virtual string Name { get; set; }

        [UseConverter(typeof(Vector2Converter))]
        public virtual Vector2 Position { get; set; }

        [UseConverter(typeof(Vector2Converter))]
        public virtual Vector2 Scale { get; set; } = new Vector2(1f, 1f);
    }
}