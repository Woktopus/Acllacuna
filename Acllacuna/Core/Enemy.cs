using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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

        public void LoadContent(World world, ContentManager content, Vector2 size, Vector2 position, string texturePath, Color textureColor)
        {
            body = BodyFactory.CreateRectangle(world, size.X, size.Y - (size.X / 2), 1f);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Position = position;
            body.Mass = 50;

            CircleShape circle = new CircleShape(size.X / 2, 1f);
            circle.Position = new Vector2(0, (size.Y - (size.X / 2)) / 2);

            Fixture feet = body.CreateFixture(circle);
            feet.UserData = (int)4;

            image.LoadContent(content, texturePath, textureColor, GetDrawPosition());

            this.size = size;
        }

        private Vector2 GetDrawPosition()
        {
            return ConvertUnits.ToDisplayUnits(body.Position + new Vector2(0, (size.Y / 2) / 2));
        }

    }
}
