using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace Acllacuna
{
	public class PhysicsUtils
	{
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
	}
}
