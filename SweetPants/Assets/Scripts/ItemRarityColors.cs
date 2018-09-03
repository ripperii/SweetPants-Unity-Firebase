using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ItemRarityColors
    {
        public static Dictionary<string, Color> colors = new Dictionary<string, Color>()
        {
            { "common", new Color(0.75f, 0.75f, 0.75f) },
            { "uncommon", Color.green },
            { "rare", Color.blue },
            { "epic", Color.magenta },
            { "legendary", Color.yellow }

        };
    }
}
