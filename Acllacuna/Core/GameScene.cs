using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Acllacuna
{
    class GameScene : Scene
    {

        Texture2D background_main;
        Rectangle background_main_rect;

        ParallaxingBackground background_parallax_1;
        ParallaxingBackground background_parallax_2;

        Animation testAnimation;

        public void Initialize()
        {
            background_main_rect = new Rectangle(0, 0, graphicDevice.Viewport.Width, graphicDevice.Viewport.Height);

            // Parallaxes
            background_parallax_1 = new ParallaxingBackground();
            background_parallax_2 = new ParallaxingBackground();

            testAnimation = new Animation();
        }

        public override void LoadContent(ContentManager Content, GraphicsDevice graph)
        {
            base.LoadContent(Content, graph);
            this.Initialize();
            background_main = Content.Load<Texture2D>("Graphics/Tutorial/mainbackground");

            // Load parallaxes
            background_parallax_1.Initialize(Content, "Graphics/Tutorial/bgLayer1", graphicDevice.Viewport.Width, graphicDevice.Viewport.Height, -1);
            background_parallax_2.Initialize(Content, "Graphics/Tutorial/bgLayer2", graphicDevice.Viewport.Width, graphicDevice.Viewport.Height, -2);

            Vector2 testAnimation_position = new Vector2(graphicDevice.Viewport.Width / 2, graphicDevice.Viewport.Height / 2);
            
			testAnimation.LoadContent(
				Content,
				/*"Graphics/Tutorial/shipAnimation"*/"Graphics/CodingMadeEasyPlatformer/Hero1", Color.White,
				testAnimation_position,
				200, new Vector2(3, 4)
			);

        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update parallaxes
            background_parallax_1.Update(gameTime);
            background_parallax_2.Update(gameTime);

            var ks = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q))
            {
                testAnimation.isActive = true;
            }
            else if (ks.IsKeyDown(Keys.W))
            {
                testAnimation.isActive = false;
            }

            if (ks.IsKeyDown(Keys.E))
            {
                testAnimation.SetSpeed(50);
            }
            else if (ks.IsKeyDown(Keys.R))
            {
                testAnimation.SetSpeed(200);
            }

            if (ks.IsKeyDown(Keys.T))
            {
                testAnimation.rotation = 0.6f;
            }
            else if (ks.IsKeyDown(Keys.Y))
            {
                testAnimation.rotation = 0.0f;
            }

            if (ks.IsKeyDown(Keys.U))
            {
				testAnimation.scale = new Vector2(4, 4);
            }
            else if (ks.IsKeyDown(Keys.I))
            {
				testAnimation.scale = new Vector2(1, 1);
            }

            if (ks.IsKeyDown(Keys.O))
            {
                testAnimation.alpha = 0.6f;
            }
            else if (ks.IsKeyDown(Keys.P))
            {
                testAnimation.alpha = 1.0f;
            }

            if (ks.IsKeyDown(Keys.Down))
            {
                testAnimation.SelectAnimation(0);
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                testAnimation.SelectAnimation(1);
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                testAnimation.SelectAnimation(2);
            }
            else if (ks.IsKeyDown(Keys.Up))
            {
                testAnimation.SelectAnimation(3);
            }

            testAnimation.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.Draw(background_main, background_main_rect, Color.White);

            // Draw parallaxes
            background_parallax_1.Draw(spriteBatch);
            background_parallax_2.Draw(spriteBatch);

            testAnimation.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
