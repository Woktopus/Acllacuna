using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Collision;
using FarseerPhysics;

namespace Acllacuna
{
	public class Image
	{
		protected Texture2D texture { get; set; }
		protected Rectangle sourceRect { get; set; }
		private Vector2 origin { get; set; }

		// Can be modified without fucking up everything
		public Color textureColor { get; set; }
		public float rotation { get; set; }
		public Vector2 scale { get; set; }
		public float alpha { get; set; }
		public Vector2 position { get; set; }

		public Rectangle SourceRect
		{
			get { return sourceRect; }
		}

		public void LoadContent(ContentManager content, string texturePath, Color textureColor, Vector2 position)
		{
			content = new ContentManager(content.ServiceProvider, "Content");

			if (texturePath != String.Empty)
			{
				texture = content.Load<Texture2D>(texturePath);
				sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
			}

			this.textureColor = textureColor;

			this.position = position;

			rotation = 0.0f;
			scale = new Vector2(1, 1);

			alpha = 1.0f;
		}

		public virtual void UnloadContent()
		{
			texture = null;
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (texture != null)
			{
				origin = new Vector2(-(sourceRect.Width * scale.X) / 2, -(sourceRect.Height * scale.Y) / 2);
				spriteBatch.Draw(texture, position + origin, sourceRect, textureColor * alpha, rotation, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
			}
		}

        public void DrawFlipVertically(SpriteBatch spriteBatch)
        {
            if (texture != null)
			{
                origin = new Vector2(-(sourceRect.Width * scale.X) / 2, -(sourceRect.Height * scale.Y) / 2);
				spriteBatch.Draw(texture, position + origin, sourceRect, textureColor * alpha, rotation, Vector2.Zero, scale, SpriteEffects.FlipVertically, 0.0f);
			}
        }


        public void DrawFlipHorizontally(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                origin = new Vector2(-(sourceRect.Width * scale.X) / 2, -(sourceRect.Height * scale.Y) / 2);
                spriteBatch.Draw(texture, position + origin, sourceRect, textureColor * alpha, rotation, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0.0f);
            }
        }

		public void ScaleToAABB(AABB aabb)
		{
			this.scale = new Vector2(
				ConvertUnits.ToDisplayUnits(aabb.Width) / sourceRect.Width,
				ConvertUnits.ToDisplayUnits(aabb.Height) / sourceRect.Height
			);
		}

        public void ScaleToMeters(Vector2 sizeInMeters)
        {
            this.scale = new Vector2(
                ConvertUnits.ToDisplayUnits(sizeInMeters.X) / sourceRect.Width,
                ConvertUnits.ToDisplayUnits(sizeInMeters.Y) / sourceRect.Height
            );
        }
	}
}
