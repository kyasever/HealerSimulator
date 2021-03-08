using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Dictionary<string, BUFF> Buffs = new Dictionary<string, BUFF>();

        public int Count { get { return Buffs.Count; } }

        /// <summary>
        /// 添加一个Buff
        /// </summary>
        public void Add(BUFF buff)
        {
            buff.Target = c;
            if (Buffs.ContainsKey(buff.Name))
            {
                //已经到了最大叠加层数,只刷新时间
                if (buff.Num == buff.MaxNum)
                {
                    Buffs[buff.Name].ReleaseTime = buff.DefaultTime;
                }
                else if (buff.Num < buff.MaxNum)
                {
                    buff.Num++;
                    buff.OnNumChanged();
                }
            }
            else
            {
                Buffs.Add(buff.Name, buff);
                buff.ReleaseTime = buff.DefaultTime;
                buff.OnAdd();
            }
        }

        /// <summary>
        /// 移除一个BUFF
        /// </summary>
        public void Remove(BUFF buff)
        {
            if (Buffs.ContainsKey(buff.Name))
            {
                Buffs.Remove(buff.Name);
                buff.OnRemove();
            }
        }

        /// <summary>
        /// 死的时候清除所有BUFF 不进行别的操作
        /// </summary>
        public void Clear()
        {
            Buffs.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var kv in Buffs)
            {
                yield return kv.Value;
            }
        }
    }

    public class PTeamBuff : BUFF
    {
        public PTeamBuff(Character caster) : base(caster)
        {
            Name = "神圣强化";
            Description = "增加30%强度等级";
            DefaultTime = 20f;
        }

        public override void OnAttack(SkillInstance s)
        {
            s.Value = (int)(s.Value * 1.3f);
        }
    }

    public class HotBuff : BUFF
    {
        Skill skill;

        public HotBuff(Character caster, float releaseTime, float hotTime, int power) : base(caster)
        {
            Name = "持续治疗";
            Description = "持续回复生命";
            DefaultTime = releaseTime;
            DefaultHot = hotTime;
            ReleaseHot = 1f;

            skill = new Skill()
            {
                Caster = Caster,
                Power = power,
                skillName = "HOT",
            };
        }

        public override void OnHot()
        {
            SkillCaster.CastSingleSkill(skill, Target);
        }
        public override void OnRemove()
        {
            SkillCaster.CastSingleSkill(skill, Target);
        }
    }

    /// <summary>
    /// 结束之后造成伤害
    /// </summary>
    public class BoomDeBUFF : BUFF
    {
        Skill skill;
        public BoomDeBUFF(string name, Character caster, float releaseTime, int power) : base(caster)
        {
            Name = name;
            Description = "结束后造成伤害";
            IsPositive = false;
            DefaultTime = releaseTime;

            skill = new Skill()
            {
                Caster = Caster,
                Power = power,
                skillName = "Boom",
            };
        }

        public override void OnRemove()
        {
            SkillCaster.CastSingleSkill(skill, Target);
        }
    }

    public class ShieldBUFF : BUFF
    {
        int power;

        public ShieldBUFF(Character caster, float releaseTime, int power) : base(caster)
        {
            Name = "护盾护盾";
            Description = "抵抗伤害";
            this.power = power;
            DefaultTime = releaseTime;
        }

        public override void OnBeHit(SkillInstance s)
        {
            if (s.Value < -power)
            {
                Debug.Log("抵挡了:" + power.ToString());
                s.Value -= power;
                power = 0;
            }
            else if (s.Value < 0)
            {
                Debug.Log("抵挡了:" + s.Value);
                power += s.Value;
                s.Value = 0;
            }
            if (power <= 0)
            {
                needRemove = true;
            }
        }
    }

    public class BUFF : IData
    {
        public BUFF(Character caster)
        {
            Caster = caster;
        }

        /// <summary>
        /// 将这个位置放置为true,则会被系统移除掉.自己在判定中不能移除自己
        /// </summary>
        public bool needRemove = false;

        /// <summary>
        /// 是否是增益BUFF,决定一部分显示问题
        /// </summary>
        public bool IsPositive = true;

        /// <summary>
        /// BUFF 的施法者
        /// </summary>
        public Character Caster;

        /// <summary>
        /// BUFF 被加给了谁
        /// </summary>
        public Character Target;

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
        public virtual void OnAttack(SkillInstance s)
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
        public virtual void OnBeHit(SkillInstance s)
        {

        }

        /// <summary>
        /// 当消失的时候触发
        /// </summary>
        public virtual void OnRemove()
        {

        }

    }
}