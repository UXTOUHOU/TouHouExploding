using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public abstract class Skill//:IID
    {
        public string name;
        public string description;
        public UseType useType = UseType.Active;
        public int typeID { get; set; }
        /// <summary>
        /// 持有该技能的卡牌
        /// </summary>
        public Unit master { get; set; }
        public Skill(Unit unit=null)
        {
            master = unit;
            SetAttribute();
        }
        /// <summary>
        /// 技能数值的模板，在这里声明新技能的参数
        /// </summary>
        public abstract void SetAttribute();
        public void Register(Unit unit)
        {
            master = unit;
        }
        public virtual void Reset()
        {
        }
        public virtual bool CanUse(InputUse inputUse = null)
        {
            return master != null;
        }

        /// <summary>
        /// 使用时效果的模板
        /// </summary>
        /// <param name="inputUse"></param>
        /// <returns></returns>
        public abstract bool Fuction(InputUse inputUse = null);
        /// <summary>
        /// 使用时调用的方法
        /// </summary>
        /// <param name="inputUse"></param>
        /// <returns></returns>
        public virtual bool Use(InputUse inputUse = null)
        {
            if (CanUse() == false) return false;

            if (Fuction(inputUse))
            {
                return true;
            }
            return false;
        }
        public class InputUse
        {

        }

        /// <summary>
        /// 发动类型
        /// </summary>
        public enum UseType
        {
            Active, Passive
        }
    }
        //主动——出牌阶段发动，需激活，一般限一次，blablablabla
        //被动——锁定技，blblablablabla
        //瞬间——每当你XXXXX时，你可以blablablabla
        //后援——出牌阶段发动，无需激活

    public abstract class ActiveSkill:Skill
    {
        public bool onlyOnce = true;
        public bool needActivation = true;

        private bool haveUsed = false;
        /// <summary>
        /// 手牌花费的B点
        /// </summary>
        public int cost = 0;
        public ActiveSkill(Unit unit)
            :base(unit)
        {
            useType = UseType.Active;
        }
        public override void Reset()
        {
            base.Reset();
            haveUsed =false;
        }
        public virtual bool HaveUsed()
        {
            return haveUsed;
        }
        public override bool CanUse(InputUse inputUse = null)
        {
            if (base.CanUse() == false) return false;
            if (master.Owner.bDot < cost) return false;
            if (haveUsed == true && onlyOnce == true) return false;
            return true;
        }
        /// <summary>
        /// 使用时调用的方法
        /// </summary>
        /// <param name="inputUse"></param>
        /// <returns></returns>
        public override bool Use(InputUse inputUse = null)
        {
            if (CanUse() == false) return false;

            if (needActivation == true)
            {
                if (master.ActionState.IsAction == false)
                {
                    if (master.Activate() == false)
                        return false;
                }
            }

            if (Fuction(inputUse))
            {
                haveUsed = true;
                master.Owner.bDot -= cost;
                return true;
            }
            return false;
        }
    }
    public abstract class PassiveSkill : Skill, NeedRespond//还没完成
    {
        public bool IsForce = true;
        public PassiveSkill(Unit unit)
            :base(unit)
        {
            useType = UseType.Passive;
        }

        /// <summary>
        /// 接口，读取要询问的事件信息，如询问者，询问内容等等，暂无用
        /// </summary>
        public ThingsToAsk thingsToAsk
        {
            get
            {
                return _thingsToAsk;
            }

            set
            {
                _thingsToAsk = value;
            }
        }
        private ThingsToAsk _thingsToAsk;

        public bool AddToNeedRespondLIst()
        {
            if (IsForce) return false;
            master.GameCore.needRespond = this;
            return true;
        }

        public bool InputRespond(ClientRespond clientRespond)
        {
            PassiveSkillClientRespond r = (PassiveSkillClientRespond)clientRespond;
            throw new NotImplementedException();
        }
        public class PassiveSkillClientRespond : ClientRespond
        {
            public bool result;
            public PassiveSkillClientRespond(bool result)
            {
                this.result = result;
            }
        }
    }
    public abstract class Effect
    {

    }
    class Art
    {

    }
    
}
