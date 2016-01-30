using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

/*
 * CLASS EXEMPLE POUR FAIRE UN SCREEN
*/

namespace Acllacuna
{
    class TitleScene : Scene
    {
        Texture2D texture;


        public override void LoadContent(ContentManager Content, GraphicsDevice graph)
        {
            base.LoadContent(Content, graph);
            texture = Content.Load<Texture2D>("Graphics/CodingMadeEasyPlatformer/Hero1");

        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime,Game game)
        {
            base.Update(gameTime, game);
            var input = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();
            if (input.IsKeyDown(Keys.A))
            {
                SceneManager.Instance.AddScene(new SplashScreen());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0,0), Color.White);
        }
    }
}
