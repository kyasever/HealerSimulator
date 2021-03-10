using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HealerSimulator
{
    /// <summary>
    /// 这个类负责结算一般意义的一个技能对某些目标的标准释放模型.
    /// 包含消耗结算,CD结算等等
    /// </summary>
    public static class SkillCaster
    {
        /// <summary>
        /// 发动无目标技能,结算ＣＤ和蓝耗
        /// </summary>
        /// <param name="s"></param>
        public static void CastToVoid(Skill s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 释放了 {1} ", s.Caster.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
            //输出结果
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 向目标发动单体技能
        /// </summary>
        public static void CastSingleSkill(Skill s, Character target)
        {
            if (target == null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //将Skill 转换为SkillInstance 进行下一步结算
            SkillCalculator.AttackSingle(s.CreateInstance(target));

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
        }

        /// <summary>
        /// 向目标群体发动技能
        /// </summary>
        public static void CastMultiSkill(Skill s, List<Character> targets)
        {
            if (targets.Count == 0)
            {
                return;
            }

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //将Skill 转换为SkillInstance 进行下一步结算
            foreach (var target in targets)
            {
                if(target.IsAlive)
                    SkillCalculator.AttackSingle(s.CreateInstance(target));
            }

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
        }
    }


    /// <summary>
    /// 这个类中的方法负责结算SkillInstance类型的Skill 
    /// 包含BUFF加成,减伤,击中结算
    /// </summary>
    public static class SkillCalculator
    {
        public static void AttackSingle(SkillInstance s)
        {
            //计算攻击者的BUFF增伤
            foreach(BUFF v in s.Caster.Buffs)
            {
                v.OnAttack(s);
            }

            //计算爆击翻倍
            if(s.Caster.CanCrit())
            {
                s.Value = s.Value * 2; 
            }

            //当造成的是伤害的时候 结算减伤
            if (s.Value < 0)
            {
                s.Value = (int)(s.Value * (1 - s.Target.Defense));
            }

            //计算被攻击者的BUFF减伤
            foreach(BUFF v in s.Target.Buffs)
            {
                v.OnBeHit(s);
            }

            //结算伤害
            s.Target.HP += s.Value;

            if (s.ID < 0)
            {
                return;
            }

            OutResult(s);

        }


        public static void OutResult(SkillInstance s)
        {
            Skill skill = FindSkill(s.ID);

            Skada.Instance.AddRecord(new SkadaRecord() { Accept = s.Target, Source = s.Caster, UseSkill = skill, Value = s.Value });
            if(skill.DebugOutLevel == Skill.DebugType.DebugLog)
            {
                StringBuilder sb = new StringBuilder();
                if (s.Value <= 0)
                {
                    sb.AppendFormat("{0} 释放了 {1} 对 {2} 造成了:{3}伤害", s.Caster.CharacterName, skill.skillName, s.Target.CharacterName, s.Value.ToString());
                }
                else
                {
                    sb.AppendFormat("{0} 释放了 {1} 对 {2} 造成了:{3}治疗", s.Caster.CharacterName, skill.skillName, s.Target.CharacterName, s.Value.ToString());
                }
                Debug.Log(sb.ToString());
            }
        }
    }


}
