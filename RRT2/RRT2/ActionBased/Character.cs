using System;

namespace RRT2
{
	public class Character
	{
		public int health;
		public int maxHealth;
		public int attack;
		public int potionLeft;
		public Character (int pMaxHealth, int pAttack, int pPotions)
		{
			this.maxHealth = pMaxHealth;
			this.health = pMaxHealth;
			this.attack = pAttack;
			this.potionLeft = pPotions;
		}
		
		public Character Copy()
		{
			Character newC = new Character(this.maxHealth, this.attack, this.potionLeft);
			newC.health = this.health;
			return newC;
		}
	}
	
}

