using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Test
    {
        public tstForm Monitor;
        Core game;
        public Test(tstForm monitor)
        {
            Monitor = monitor;
            game = new Core();
        }
        public void 测试()
        {

            下一阶段();
            输出召唤池信息(true);
            到行动阶段();

            卡牌召唤(game.Characters[0],2,0);
            输出单位位置(game.Players[0].unit[0]);
            单位激活(game.Players[0].unit[0]);
            单位移动(game.Players[0].unit[0], 1, 2);
            输出单位位置(game.Players[0].unit[0]);
            到行动阶段();

            输出召唤池信息();
            卡牌召唤(game.Characters[0], 1, 10);
            单位激活(game.Players[1].unit[0]);
            单位移动(game.Players[1].unit[0], 1, 7);
            到行动阶段();

            卡牌召唤(game.Characters[0], 3, 1);
            单位激活(game.Players[0].unit[0]);
            单位移动(game.Players[0].unit[0], 1, 5);
            单位攻击(game.Players[0].unit[0], game.Players[1].unit[0]);
            到行动阶段();

            单位攻击(game.Players[1].unit[0], game.Players[0].unit[0]);
            到行动阶段();

            单位移动(game.Players[0].unit[1], 3, 4);
            单位攻击(game.Players[0].unit[0], game.Players[1].unit[0]);
            单位移动(game.Players[0].unit[0], 2, 7);
            到行动阶段(game.Players[0]);

            单位移动(game.Players[0].unit[1], 3, 7);
            单位移动(game.Players[0].unit[0], 2, 10);
            单位攻击基地(game.Players[0].unit[0], game.Players[1]);
            到行动阶段(game.Players[0]);

            单位移动(game.Players[0].unit[1], 3, 10);
            单位攻击基地(game.Players[0].unit[0], game.Players[1]);
            单位攻击基地(game.Players[0].unit[1], game.Players[1]);
            到行动阶段(game.Players[0]);

            单位攻击基地(game.Players[0].unit[0], game.Players[1]);
            单位攻击基地(game.Players[0].unit[1], game.Players[1]);
            到行动阶段(game.Players[0]);
        }
        //public class TestA
        //{
        //    public int a { get; set; }
        //    public static int TestValue(TestA aa)
        //    {
        //        aa.a = 4;
        //        return aa.a;
        //    }
        //    public static int OutTestValue(out TestA aa)
        //    {
        //        aa.a = 4;
        //        return aa.a;
        //    }
        //    public static int RefTestValue(ref TestA aa)
        //    {
        //        aa.a = 4;
        //        return aa.a;
        //    }
        //}

        public bool 结束战报()
        {
            if (game.NowProcess == Core.Process.RoomEnding)
            {
                if (game.GameEndReport.state == Core.EndReport.State.SomeoneWin)
                    Monitor.Log("该场赢家为:" + game.GameEndReport.Winner.name + "(ID:" + game.GameEndReport.Winner.id + ")");
                if (game.GameEndReport.state == Core.EndReport.State.Draw)
                    Monitor.Log("该场为平局！");
                return true;
            }
            return false;
        }
        public void 查询B点(Player player)
        {
            Monitor.Log("玩家:" + player.name + "(ID: " + player.id + ")" + "剩余B点" + player.bDot);
          
        }
        public void 卡牌召唤(Character character, int x, int y)
        {
            Unit u = game.NowPlayer.Call(character, new int[2] { x, y });
            if (u == null)
            {
                Monitor.Log("召唤卡牌(ID:" + character.id.ToString() + ")至(" + x.ToString() + "," + y.ToString() + ")。结果:False");
            }
            else
            {
                Monitor.Log("召唤卡牌(ID:" + character.id.ToString() + ")至(" + x.ToString() + "," + y.ToString() + ")。新单位ID:" + u.id.ToString());
            }
            
        }
        public void 单位移动(Unit unit, int x, int y)
        {
            Monitor.Log("单位(ID:" + unit.id.ToString() + ")尝试移动至(" + x.ToString() + "," + y.ToString() + ")。结果:" + unit.Move(x,y).ToString());
        }
        public void 单位攻击基地(Unit unit, Player beAttacked)
        {
            Monitor.Log("单位(ID:" + unit.id.ToString() + ")尝试攻击玩家"+beAttacked.name+"(ID:"+beAttacked.id+")的队伍(ID:"+beAttacked.atTeam.id+")基地。结果:" + unit.AttackBase(beAttacked));
            Monitor.Log("被攻击队伍(ID:" + beAttacked.atTeam.id + ")剩余血量:" + beAttacked.blood);
        }
        public void 单位攻击基地(Unit unit, Team beAttacked)
        {
            Monitor.Log("单位(ID:" + unit.id.ToString() + ")尝试攻击队伍(ID:"+beAttacked.id+")基地。结果:" + unit.AttackBase(beAttacked));
            Monitor.Log("被攻击队伍(ID:" + beAttacked.id + ")剩余血量:" + beAttacked.blood);
        }
        public void 单位攻击(Unit unit, Unit beAttacked)
        {
            Monitor.Log("单位(ID:" + unit.id.ToString() + ")尝试攻击(ID:" + beAttacked.id + ")。结果:" + unit.Attack(beAttacked).ToString());
            Monitor.Log("被攻击单位(ID:" + beAttacked.id + ")剩余血量:" + beAttacked.attribute.blood);
        }
        public void 单位激活(Unit unit)
        {
            Monitor.Log("单位(ID:" + unit.id.ToString() + ")激活。结果:" + unit.Activition().ToString());
        }
        public void 单位待机(Unit unit)
        {
            Monitor.Log("单位(ID:" + unit.id.ToString() + ")待机。结果:" + unit.Unactivition().ToString());
        }
        public void 到行动阶段(Player player=null)
        {
            if(game.NowProcess == Core.Process.RoomEnding)
            {
                Monitor.Log("游戏已经结束");
                结束战报();
                return;
            }
            下一阶段();
            while ((game.NowProcess != Core.Process.RoundAction) || (game.NowPlayer != player && player != null)) 下一阶段();
        }
        public void 下一阶段()
        {
            if (game.NowProcess == Core.Process.RoomEnding)
            {
                Monitor.Log("游戏已经结束");
                结束战报();
                return;
            }
            Core.Process p = game.NextStep();
            Monitor.Log(p.ToString());
            if (p == Core.Process.RoundStarting) Monitor.Log("控制权切换至玩家:" + game.NowPlayer.name + "(ID:" + game.NowPlayer.id + ")");
            if (p == Core.Process.RoundEnding) 查询B点(game.NowPlayer);
        }
        public void 输出单位位置(Unit unit)
        {
            Monitor.Log
                ("单位(ID:" + unit.id + ")所在坐标：" + 
                unit.at.locate[0].ToString() 
                + "," + 
                unit.at.locate[1].ToString());
        }
        public void 输出召唤池信息(bool 是否详细 = false)
        {
            Monitor.Log("输出召唤池相关信息：");
            if (是否详细 == true)
            {
            Monitor.Log("在卡池中的少女数：" + game.WaitingCharacters.Count.ToString());
            Monitor.Log("在召唤池中的少女数：" + game.Characters.Count.ToString());
            }
            int temp = 0;
            foreach (Character c in game.Characters)
            {
                Monitor.Log("第" + temp.ToString() + "号召唤位上的是【" + c.name + "】(ID:" + c.id + ")");
                if (是否详细)
                {
                Monitor.Log("血量/攻击/机动/射程：" + c.unitAttribute.blood.ToString() + "/" + c.unitAttribute.attack.ToString() + "/" + c.unitAttribute.mobility.ToString() + "/" + c.unitAttribute.mobility.ToString());
                if (c.unitAttribute.skill != null)
                {
                    foreach (Skill s in c.unitAttribute.skill)
                        Monitor.Log("[" + s.name + "]  " + s.description);
                }
                }
                temp++;
            }
        }
        public void 查询人物卡详细信息(int CID)
        {
            Monitor.Log("查询人物卡详细信息：");
            Character c = (Character) game.IDP.CID.GetObj(CID);
            Monitor.Log("【" + c.name + "】ID:" + c.id + "");
            Monitor.Log("简介：" + c.description);
            Monitor.Log("血量/攻击/机动/射程：" + c.unitAttribute.blood.ToString() + "/" + c.unitAttribute.attack.ToString() + "/" + c.unitAttribute.mobility.ToString() + "/" + c.unitAttribute.mobility.ToString());

            if (c.unitAttribute.skill != null)
            {
                foreach (Skill s in c.unitAttribute.skill)
                    Monitor.Log("[" + s.name + "】：" + s.description);
            }
            Monitor.Log(tstForm.特殊.单横);
        }
    }
}
