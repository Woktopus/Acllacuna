using System;

namespace Acllacuna.Core
{
	public class Enemy : Player
	{
		private static int NextID = 0;

		private int id;

		public Enemy() : base()
		{
			id = NextID;
			NextID++;
		}
	}
}
