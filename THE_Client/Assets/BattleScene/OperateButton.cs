using UnityEngine;
using System.Collections;

namespace BattleScene
{
	public class OperateButton : MonoBehaviour
	{
		public void OnClickButtonSkill()
		{
			BattleProcess.ChangeState(PlayerState.PS_SelectSkill);
		}

		public void OnClickButtonMove()
		{
			BattleProcess.ChangeState(PlayerState.PS_SelectMovePosition);
		}

		public void OnClickButtonAttack()
		{
			BattleProcess.ChangeState(PlayerState.PS_SelectAttackTarget);
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}