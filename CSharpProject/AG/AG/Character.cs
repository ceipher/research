using System;
using System.Collections.Generic;
namespace AG
{
	public class Character
	{
		public string name;
		public int health;
		public int maxHealth;
		public int mana = 50;
		public int maxMana = 50;
		public int attack;
		public DEBUFF debuff = DEBUFF.NONE;
		public int debuffLeft = 0;
		public List<Magic> magics = new List<Magic>();

		public Character (int pMaxHealth, int pAttack, string pName)
		{
			this.maxHealth = pMaxHealth;
			this.health = pMaxHealth;
			this.attack = pAttack;
			this.name = pName;
		}

		public Character (int pMaxHealth, int pAttack, string pName, List<Magic> pMagics)
		{
			this.maxHealth = pMaxHealth;
			this.health = pMaxHealth;
			this.attack = pAttack;
			this.name = pName;
			magics.AddRange (pMagics);
		}

		public Character Copy()
		{
			Character newC = new Character(maxHealth, attack, name, magics);
			newC.health = this.health;
			newC.mana = this.mana;
			newC.debuff = debuff;
			newC.debuffLeft = debuffLeft;
			return newC;
		}

		public bool isNormal()
		{
			return debuff != DEBUFF.SLEEPING && health > 0;
		}
	}
	
}

