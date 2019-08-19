using System;
using System.Collections;
using System.Collections.Generic;

namespace HealerSimulator
{
    public class BUFFContainer : IEnumerable
    {
        private Character c;
        public BUFFContainer(Character character)
        {
            c = character;
        }

        /// <summary>
        /// BUFF状态栏
        /// </summary>
        public Dictionary<int, BUFF> Buffs = new Dictionary<int, BUFF>();

        /// <summary>
        /// 添加一个Buff
        /// </summary>
        public void Add(BUFF buff)
        {
            buff.Character = c;
            if (Buffs.ContainsKey(buff.ID))
            {
                if (buff.Num < buff.MaxNum)
                {
                    buff.Num++;
                    buff.OnNumChanged();
                }
            }
            else
            {
                Buffs.Add(buff.ID, buff);
                buff.ReleaseTime = buff.DefaultTime;
                buff.OnAdd();
            }
            c.PropChanged();
        }

        /// <summary>
        /// 移除一个BUFF
        /// </summary>
        public void Remove(BUFF buff)
        {
            if (Buffs.ContainsKey(buff.ID))
            {
                Buffs.Remove(buff.ID);
                buff.OnRemove();
            }
            c.PropChanged();
        }


         public IEnumerator GetEnumerator()
         {
            foreach(var kv in Buffs)
            {
                yield return kv.Value;
            }
         }
    }

    public class BUFF : IDataBinding
    {
        /// <summary>
        /// BUFF 属于的单位
        /// </summary>
        public Character Character;

        public List<Action> OnChangeEvent { get; set; } = new List<Action>();

        public void PropChanged()
        {
            if (OnChangeEvent.Count == 0)
            {
                return;
            }
            foreach (Action a in OnChangeEvent)
            {
                a.Invoke();
            }
        }


        public static int GlobalID = 1;

        public int ID = 1;
        //每被创建一次 ID都会自增
        public BUFF()
        {
            ID = GlobalID;
            GlobalID++;
        }


        public bool IsPositive = true;

        //显示层数
        public int Num = 1;

        public int MaxNum = 1;

        /// <summary>
        /// 当叠加层数发生变化时触发.通常和OnAdd不一起重载
        /// </summary>
        public virtual void OnNumChanged()
        {

        }

        public string Name = "斩杀";
        /// <summary>
        /// 显示小图标,不超过一个字
        /// </summary>
        public string Icon = "斩";


        /// <summary>
        /// 总时间 为-1说明是光环buff
        /// </summary>
        public float DefaultTime = -1f;
        /// <summary>
        /// 持续时间 
        /// </summary>
        public float ReleaseTime = 0f;

        /// <summary>
        /// 3秒一跳 负数则不触发跳 享受急速
        /// </summary>
        public float DefaultHot = -1f;
        public float ReleaseHot = 0f;

        public string Description = "斩杀,对生命值低于20%的单位伤害提高100%";

        /// <summary>
        /// 当被挂上的时触发
        /// </summary>
        public virtual void OnAdd()
        {

        }

        /// <summary>
        /// 当发动攻击时,检测自己身上的BUFF
        /// </summary>
        public void OnAttack(SkillInstance s)
        {

        }

        /// <summary>
        /// 当触发周期性效果时
        /// </summary>
        public virtual void OnHot()
        {

        }

        /// <summary>
        /// 当被攻击时,检测BUFF
        /// </summary>
        public void OnBeHit(SkillInstance s)
        {

        }

        /// <summary>
        /// 当消失的时候触发
        /// </summary>
        public void OnRemove()
        {

        }

    }
}