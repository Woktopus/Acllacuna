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
    public class CollectibleItem
    {

        private static int NextId = 0;

        public int id { get; set; }
        public Body body { get; set; }
        public Image image { get; set; }
        public  CollectibleItemType type { get; set; }
        public Vector2 bodyPosition { get; set; }


        public CollectibleItem()
        {
            this.id = NextId;
            NextId++;
        }


        public void LoadContent(World world, Vector2 size, Vector2 position, ContentManager Content, string texturePath)
        {
            //Initialisation du body
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.Position = position;
            bodyPosition = position;
            body.FixtureList[0].UserData = (int)(500+id);

            //Initialisation de l'image
            Vector2 imagePosition = new Vector2(ConvertUnits.ToDisplayUnits(position.X), ConvertUnits.ToDisplayUnits(position.Y));
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
