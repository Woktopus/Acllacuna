using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acllacuna
{
    public class Block
    {

        public Body body { get; set; }
        public Image image { get; set; }

        public Block()
        {
            
        }

        public void LoadContent(World world, Vector2 size, Vector2 position, ContentManager Content, string texturePath )
        {
            //Initialisation du body 
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.BodyType = BodyType.Static;
            body.Position = position;

			body.FixtureList[0].UserData = (int)2;

            //Initialisation de l'image du block
            Vector2 imagePosition = new Vector2(ConvertUnits.ToDisplayUnits(position.X),ConvertUnits.ToDisplayUnits(position.Y));
            image = new Image();
            image.LoadContent(Content, texturePath, Color.White, imagePosition);
            image.ScaleToMeters(size);
        }

        

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }
    }
}
