using UnityEngine;
using SiraUtil.Converters;
using IPA.Config.Stores.Attributes;

namespace DiUI.Models
{
    public class DiButton
    {

        public virtual string Name { get; set; }
        public virtual int Rotation { get; set; }
        public virtual bool HasText { get; set; }
        public virtual bool ShowText { get; set; }
        public virtual string Source { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual string NormalImage { get; set; }
        public virtual string SelectedImage { get; set; }

        [UseConverter(typeof(Vector2Converter))]
        public virtual float Skew { get; set; }

        [UseConverter(typeof(Vector2Converter))]    
        public virtual Vector2 TextPosition { get; set; }
    }
}