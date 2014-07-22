using System;

namespace RRT2
{
	public class Character
	{
		public int health;
		public int max_health;
		public int attack;
		public Character (int pMaxHealth, int pAttack)
		{
			this.max_health = pMaxHealth;
			this.health = pMaxHealth;
			this.attack = pAttack;
		}
		
		public Character Copy()
		{
			Character newC = new Character(this.max_health, this.attack);
			newC.health = this.health;
			return newC;
		}
	}
	
}

