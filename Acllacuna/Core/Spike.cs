using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    public class Spike
    {

        public Body body { get; set; }
        public Image image { get; set; }
        public Vector2 ImagePosition { get; set; }

        public Spike()
        {

        }

        public void LoadContent(ContentManager Content, World world, Vector2 size, Vector2 position, string texturePath)
        {
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.BodyType = BodyType.Static;
            body.Position = position;
            body.FixtureList[0].UserData = 2000;
            body.IsSensor = true;

            Vector2 imagePosition = new Vector2(ConvertUnits.ToDisplayUnits(position.X), ConvertUnits.ToDisplayUnits(position.Y));
            image = new Image();
            image.LoadContent(Content, texturePath, Color.White, imagePosition);
            image.ScaleToMeters(new Vector2(2,2));
            this.ImagePosition = imagePosition;

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }
    }
}
