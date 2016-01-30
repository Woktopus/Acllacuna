using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acllacuna
{
    class MenuScene : Scene
    {
        Button jouer;
        Button option;
        Button quitter;

        public override void LoadContent(ContentManager Content, GraphicsDevice graph)
        {
            jouer = new Button(Content.Load<Texture2D>("Graphics/Menu/jouer"),graph);
            jouer.Position = new Vector2(250, 50);

            option = new Button(Content.Load<Texture2D>("Graphics/Menu/option"), graph);
            option.Position = new Vector2(250, 200);

            quitter = new Button(Content.Load<Texture2D>("Graphics/Menu/quitter"), graph);
            quitter.Position = new Vector2(250, 350);

            base.LoadContent(Content, graph);
        }

        public override void Update(GameTime gameTime, Game game)
        {
            if (jouer.isCliked)
            {
                SceneManager.Instance.AddScene(new PhysicsScene());
            }
            else if (option.isCliked)
            {
                Console.WriteLine("option");
            }
            else if (quitter.isCliked)
            {
                game.Exit();
            }
            jouer.Update(gameTime);
            option.Update(gameTime);
            quitter.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            jouer.Draw(spriteBatch);
            option.Draw(spriteBatch);
            quitter.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
