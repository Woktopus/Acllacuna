using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acllacuna
{
	class ParallaxingBackground
	{
		Texture2D texture;

		// An array of positions of the parallaxing background.
		Vector2[] positions;

		int speed;
		int screenHeight;
		int screenWidth;

		public void Initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight, int speed)
		{
			this.screenHeight = screenHeight;
			this.screenWidth = screenWidth;

			texture = content.Load<Texture2D>(texturePath);

			this.speed = speed;

			// If we divide the screen with the texture width then we can determine the number of tiles needed.
			// We add 1 to it so that we won't have a gap in the tiling.
			int numOfTiles = (int)(Math.Ceiling(screenWidth / (float)texture.Width) + 1);
			positions = new Vector2[numOfTiles];

			// Set the initial positions of the parallaxing background
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] = new Vector2(i * texture.Width, 0);
			}
		}

		public void Update(GameTime gameTime)
		{
			// Update the positions of the background
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i].X += speed;
			}

			for (int i = 0; i < positions.Length; i++)
			{
				if (speed <= 0)
				{
					// Check if the texture is out of view and then put that texture at the end of the screen.
					if (positions[i].X <= -texture.Width)
					{
						WrapTextureToLeft(i);
					}
				}
				else
				{
					if (positions[i].X >= texture.Width * (positions.Length - 1))
					{
						WrapTextureToRight(i);
					}
				}
			}
		}

		private void WrapTextureToLeft(int index)
		{
			// If the textures are scrolling to the left, when the tile wraps, it should be put at the
			// one pixel to the right of the tile before it.
			int prevTexture = index - 1;
			if (prevTexture < 0)
				prevTexture = positions.Length - 1;

			positions[index].X = positions[prevTexture].X + texture.Width;
		}

		private void WrapTextureToRight(int index)
		{
			// If the textures are scrolling to the right, when the tile wraps, it should be placed to the left
			// of the tile that comes after it.
			int nextTexture = index + 1;
			if (nextTexture == positions.Length)
				nextTexture = 0;

			positions[index].X = positions[nextTexture].X - texture.Width;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				var rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y,
										   texture.Width,
										   screenHeight);
				spriteBatch.Draw(texture, rectBg, Color.White);
			}
		}
	}
}