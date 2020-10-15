using DiUI.Models;
using UnityEngine;
using System.Collections.Generic;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

namespace DiUI
{
    public class Config
    {
        [UseConverter(typeof(ListConverter<DiChild>))]
        public List<DiChild> Children { get; set; } = new List<DiChild>
        {
            new DiChild
            {
                Name = "Solo",
                Enabled = true,
                Position = new Vector2(-40f, -55f),
            },
            new DiChild
            {
                Name = "Online",
                Enabled = true,
                Scale = new Vector2(1f, 0.95f),
                Position = new Vector2(-35f, -28.5f),
            },
            new DiChild
            {
                Name = "Help",
                Enabled = true,
                Position = new Vector2(35f, -67.5f),
            },
            new DiChild
            {
                Name = "Editor",
                Enabled = true,
                Position = new Vector2(22.5f, -67.5f),
            },
            new DiChild
            {
                Name = "Campaign",
                Enabled = true,
                Position = new Vector2(-10f, -55f),
            },
            new DiChild
            {
                Name = "Options",
                Enabled = true,
                Position = new Vector2(-38.5f, -67.5f),
            },
            new DiChild
            {
                Name = "Party",
                Enabled = true,
                Scale = new Vector2(1f, 0.95f),
                Position = new Vector2(30f, -55f),
            },
            new DiChild
            {
                Name = "Exit",
                Enabled = true,
                Position = new Vector2(47.5f, -67.5f),
            },
        };
    }
}