using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    public class CollectibleItem
    {
        public Body body { get; set; }
        public Image image { get; set; }

        public CollectibleItem()
        {
            image = new Image();

        }

    }
}
