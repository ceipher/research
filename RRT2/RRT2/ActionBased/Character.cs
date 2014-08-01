using System;

namespace RRT2
{
	public class Character
	{
		public int health;
		public int maxHealth;
		public int attack;
		public Character (int pMaxHealth, int pAttack)
		{
			this.maxHealth = pMaxHealth;
			this.health = pMaxHealth;
			this.attack = pAttack;
		}
		
		public Character Copy()
		{
			Character newC = new Character(this.maxHealth, this.attack);
			newC.health = this.health;
			return newC;
		}
		
		public void castAction(Action action)
		{
			Character target = action.target;
			switch(action.type)
			{
				case ACTION_TYPE.NONE:
					break;
				
				case ACTION_TYPE.POTION:					
					target.health += Global.POTION_HEAL;
					if (target.health > target.maxHealth) target.health = target.maxHealth;
					break;
				
				case ACTION_TYPE.ATTACK:
					target.health -= this.attack;
					if (target.health < 0) target.health = 0;
					break;
			}
		}
		
	}
	
}

