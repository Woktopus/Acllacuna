using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Acllacuna
{
	public class Camera
	{
		public Vector2 position { get; private set; }
		public float zoom { get; private set; }
		public float rotation { get; private set; }
		public int viewportWidth { get; set; }
		public int viewportHeight { get; set; }

		public Camera()
		{
			zoom = 1.0f;
		}

		public void SetZoom(float amount)
		{
			zoom += amount;
			if (zoom < 0.25f)
			{
				zoom = 0.25f;
			}
		}

		public Matrix TranslationMatrix
		{
			get
			{
				return Matrix.CreateTranslation(-(int)position.X, -(int)position.Y, 0)
					* Matrix.CreateRotationZ(rotation)
					* Matrix.CreateScale(new Vector3(zoom, zoom, 1))
					* Matrix.CreateTranslation(new Vector3(new Vector2(viewportWidth * 0.5f, viewportHeight * 0.5f), 0)
				);
			}
		}

		private Vector2 ClampedPosition(Vector2 position, Map map)
		{
			var cameraMax = new Vector2(
				map.width - (viewportWidth / zoom / 2),
				map.height - (viewportHeight / zoom / 2));

			return Vector2.Clamp(
				position,
			   new Vector2(viewportWidth / zoom / 2, viewportHeight / zoom / 2),
			   cameraMax);
		}

		public void CenterOn(Vector2 position)
		{
			this.position = ClampedPosition(position);
		}

		public Vector2 WorldToScreen(Vector2 worldPosition)
		{
			return Vector2.Transform(worldPosition, TranslationMatrix);
		}

		public Vector2 ScreenToWorld(Vector2 screenPosition)
		{
			return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
		}
	}
}
