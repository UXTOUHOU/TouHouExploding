using UnityEngine;
using System.Collections;

namespace BattleScene
{
	public class CardAttribute
	{
		public int ID;              //卡片编号;
		public int hp;              //HP
		public int motility;		//机动性
		public int attack;			//攻击力
		public int minAtkRange;		//攻击范围最小值
		public int maxAtkRange;     //攻击范围最大值

		public CardAttribute()
		{
			//Test
			ID = 1;
			hp = 10;
			attack = 3;
			motility = 3;
			minAtkRange = 0;
			maxAtkRange = 3;
			//
		}

		public CardAttribute(int cardID)
		{
			//从文件中读取
			throw new System.NotImplementedException();
		}
	}
}