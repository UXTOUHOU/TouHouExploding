using UnityEngine;
using System.Collections;
using BattleScene;
using System;
using System.Collections.Generic;

namespace BattleScene
{
	public class UnitUIManager : MonoBehaviour
	{
		public static List<BattleScene.UnitUI> UnitList = new List<BattleScene.UnitUI>();

		public static bool HPChanged = false;
		public static bool GroupChanged = false;
		public static bool CellChanged = false;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (CellChanged)
				throw new NotImplementedException();
			if (HPChanged)
			{
				foreach (var it in UnitList)
				{
					it.UpdateTextHP();
				}
			}
			if (GroupChanged)
			{
				foreach (var it in UnitList)
				{
					it.UpdateGroup();
				}
			}
		}

		public static void AddUnitSprite(BattleScene.UnitUI unitSprite)
		{
			UnitList.Add(unitSprite);
		}

		public static void SetAllUnitAttributeVisible(bool visible)
		{
			throw new NotImplementedException();
		}
	}
}