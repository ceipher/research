using System;

namespace RRT2
{
	public class Character
	{
		public int health;
		public int max_health;
		public int attack;
		public Character (int maxHealth, int attack)
		{
			this.max_health = max_health;
			this.health = max_health;
			this.attack = attack;
		}
	}
}

