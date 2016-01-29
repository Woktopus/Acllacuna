using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acllacuna
{
	public class Text
	{
		private SpriteFont font { get; set; }
		private Vector2 origin { get; set; }

		// Can be modified without fucking up everything
		public string text { get; set; }
		public Color fontColor { get; set; }
		public float rotation { get; set; }
		public Vector2 scale { get; set; }
		public float alpha { get; set; }
		public Vector2 position { get; set; }

		public void LoadContent(ContentManager content, string fontPath, Color fontColor, string text, Vector2 position)
		{
			content = new ContentManager(content.ServiceProvider, "Content");

			if (fontPath != String.Empty)
			{
				font = content.Load<SpriteFont>(fontPath);
			}

			this.fontColor = fontColor;

			this.text = text;

			this.position = position;

			rotation = 0.0f;
			scale = new Vector2(1, 1);

			alpha = 1.0f;
		}

		public virtual void UnloadContent()
		{
			font = null;
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (font != null)
			{
				origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
				spriteBatch.DrawString(font, text, position, fontColor * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
			}
		}
	}
}
