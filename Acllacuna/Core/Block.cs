using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace Acllacuna
{
    public class Block
    {

        public Body body { get; set; }

        public Block()
        {
            
        }

        public void LoadContent(World world, Vector2 size, Vector2 position )
        {
            //bodytype static
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.BodyType = BodyType.Static;
            body.Position = position;
        }

        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}
