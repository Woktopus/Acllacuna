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
    public class Dagger
    {
        public Body body { get; set; }
        public Image image { get; set; }
        public bool isAtacking { get; set; }
        public Vector2 bodyPosition { get; set; }
        public DirectionEnum direc { get; set; }

        public Player player { get; set; }

        public Dagger()
        {

        }

        public void LoadContent(ContentManager Content, World world, Vector2 size, Vector2 position, string texturePath,bool isAttacking = false)
        {
            this.isAtacking = isAtacking;
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.BodyType = BodyType.Kinematic;
            body.Position = position;
            bodyPosition = position;
            body.FixtureList[0].UserData = 1500;

            Vector2 imagePosition = new Vector2(ConvertUnits.ToDisplayUnits(position.X), ConvertUnits.ToDisplayUnits(position.Y));
            image = new Image();
            image.LoadContent(Content, texturePath, Color.White, imagePosition);
            image.ScaleToMeters(size);
        }




        public void Update(GameTime gameTime)
        {
            if(direc == DirectionEnum.LEFT)
            {
                image.position = new Vector2(ConvertUnits.ToDisplayUnits(body.Position.X), ConvertUnits.ToDisplayUnits(body.Position.Y));

            }
            else
            {
                image.position = new Vector2(ConvertUnits.ToDisplayUnits(body.Position.X), ConvertUnits.ToDisplayUnits(body.Position.Y));
            }
            body.Position = bodyPosition;
            if (player.isAttacking)
            {
                body.IsSensor = false;
                //body.Enabled = true;       
            }
            else
            {
                body.IsSensor = true;
               //body.Enabled = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (player.isAttacking)
            {
                if (direc == DirectionEnum.LEFT)
                {
                    image.DrawFlipHorizontally(spriteBatch);
                }
                else
                {
                    image.Draw(spriteBatch);
                }
            }
        }
    }
}
