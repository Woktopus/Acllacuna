using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    public class Enemy
    {
        public Body body { get; set; }
        public Image image { get; set; }
        public Vector2 size { get; set; }
        public Vector2 bodyPosition { get; set; }

        public int contactsWithFloor { get; set; }

        public Enemy()
        {
            image = new Image();
            contactsWithFloor = 0;
        }

        public void LoadContent(World world, ContentManager content, Vector2 size, Vector2 position, string texturePath)
        {
            this.size = size;
        }

    }
}
