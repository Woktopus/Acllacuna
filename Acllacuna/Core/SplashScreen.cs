using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Acllacuna
{
    class SplashScreen : Scene
    {
        Texture2D texture;
        float time = 0f;


        public override void LoadContent(ContentManager Content, GraphicsDevice graph)
        {
            base.LoadContent(Content, graph);
            texture = Content.Load<Texture2D>("Graphics/Splash/woktopus");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 3f)
            {
                SceneManager.Instance.AddScene(new GameScene());
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0.0f);
        }
    }
}
