
using System;
namespace AG
{
	public class SleepUseCases
	{		
		static int H;
		static int A;
		static int minHealth;
		static int maxHealth;
		static int healthStepSize;
		static int minAttack;
		static int maxAttack;
		static int attackStepSize;

		public static void Case1() // 2 attacks required
		{
			H = 12;
			A = 8;
			minHealth = H/4;
			maxHealth = H*3;
			healthStepSize = H/4;
			minAttack = A/4;
			maxAttack = A*3;
			attackStepSize = A/4;
		}

		public static void Case2() // 3 attacks required
		{
			H = 20;
			A = 8;
			minHealth = H/4;
			maxHealth = H*3;
			healthStepSize = H/4;
			minAttack = A/4;
			maxAttack = A*3;
			attackStepSize = A/4;
		}

	}
}

