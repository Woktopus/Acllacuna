using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    class Button
    {
        Texture2D texture;
        Vector2 position;

        public Vector2 Position
        {
            set
            {
                position = value;
            }
        }
        Rectangle rectangleBouton;
        Color couleur = new Color(255,255,255,255);
        public Vector2 size;
        
        public Button(Texture2D texture,GraphicsDevice graphics)
        {
            this.texture = texture;
            size = new Vector2(texture.Width, texture.Height);
        }

        bool down;
        public bool isCliked;

        public void Update(GameTime gameTime)
        {
            var souris = ServiceHelper.Get<InputManagerService>().Mouse.GetState();
            var prevSouris = ServiceHelper.Get<InputManagerService>().Mouse.GetPrevState();
            isCliked = false;
            rectangleBouton = new Rectangle((int)position.X, (int)position.Y, (int)(size.X*0.4f), (int)(size.Y*0.4f));
            Rectangle rectangleSouris = new Rectangle(souris.X, souris.Y, 1, 1);
            if (rectangleSouris.Intersects(rectangleBouton))
            {
                if (couleur.A == 255) down = false;
                if (couleur.A == 0) down = true;
                if (down) couleur.A += 3; else couleur.A -= 3;
                if (souris.LeftButton == ButtonState.Pressed && prevSouris.LeftButton == ButtonState.Released) isCliked = true;
            }
            else if (couleur.A < 255)
            {
                couleur.A += 3;
                isCliked = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, rectangleBouton, couleur);
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), couleur, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0.0f);

        }
    }
}
