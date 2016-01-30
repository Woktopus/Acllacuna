using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using System;

namespace Acllacuna
{
	public class PhysicsUtils
	{
		public static float FPS = 30f;

		public static AABB GetAABBFromBody(Body body)
		{
			Transform t;
			body.GetTransform(out t);

			AABB aabb;
			aabb.LowerBound = new Vector2(Settings.MaxFloat, Settings.MaxFloat);
			aabb.UpperBound = new Vector2(-Settings.MaxFloat, -Settings.MaxFloat);

			AABB tempAABB;

			int fixtureCount = body.FixtureList.Count;
			for (int i = 0; i < fixtureCount; ++i)
			{
				body.FixtureList[i].Shape.ComputeAABB(out tempAABB, ref t, 0);
				aabb.Combine(ref tempAABB);
			}

			return aabb;
		}

		public static float GetVerticalSpeedToReach(World world, float height)
		{
			if (height <= 0)
			{
				return 0;
			}

			float t = 1 / FPS;
			Vector2 gravityStep = world.Gravity * t * t;

			//ax² + bx + c = 0
			float a = 0.5f / gravityStep.Y;
			float b = 0.5f;
			float c = height;

			double solution1 = (-b - Math.Sqrt(Math.Abs(b * b - 4f * a * c))) / (2f * a);
			double solution2 = (-b + Math.Sqrt(Math.Abs(b * b - 4f * a * c))) / (2f * a);

			float verticalSpeed = (float)solution1;
			if (verticalSpeed < 0)
			{
				verticalSpeed = (float)solution2;
			}

			return verticalSpeed * FPS;
		}
	}
}
