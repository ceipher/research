using System;

namespace AG
{
	
	public enum ACTION_TYPE {ATTACK, MAGIC, POTION, NONE};	
	public enum DEBUFF {NONE, SLEEPING};

	public class Magic 
	{
		public static Magic Sleep = new Magic ("Sleep", 0, 50, DEBUFF.SLEEPING, 8); 

		public String name;
		public int damage;
		public int manaCost;
		public DEBUFF debuff;
		public int effectiveRounds;
		public Magic(String pName, int pDamage, int pManaCost, DEBUFF pDebuff, int pEffectiveRounds)
		{
			name = pName;
			damage = pDamage;
			manaCost = pManaCost;
			debuff = pDebuff;
			effectiveRounds = pEffectiveRounds;
		}
		
	}

	public class Action
	{
		public ACTION_TYPE type;
		public Character source;
		public Character target;
		public Magic magic;
				
		public Action (ACTION_TYPE actionType,
		               Character actionSource, Character actionTarget)
		{
			this.type = actionType;
			this.source = actionSource;
			this.target = actionTarget;
		}

		public Action (ACTION_TYPE actionType, Magic actionMagic,
		               Character actionSource, Character actionTarget)
		{
			this.type = actionType;
			this.source = actionSource;
			this.target = actionTarget;
			this.magic = actionMagic;
		}

		public static Action NoAction(Character c)
		{
			return new Action (ACTION_TYPE.NONE, c, c);
		}

		
		public override string ToString ()
		{
			if (type == ACTION_TYPE.NONE) 
			{
				return source.name + " does NOTHING";
			} else {
				return source.name + " does "+ type.ToString() + " on " + target.name;
			}
		}
	}


}


