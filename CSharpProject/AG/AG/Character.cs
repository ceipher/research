using System;

namespace AG
{
	public class Character
	{
		public string name;
		public int health;
		public int maxHealth;
		public int attack;
		public Character (int pMaxHealth, int pAttack, string pName)
		{
			this.maxHealth = pMaxHealth;
			this.health = pMaxHealth;
			this.attack = pAttack;
			this.name = pName;
		}
		
		public Character Copy()
		{
			Character newC = new Character(maxHealth, attack, name);
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

