using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HealerSimulator
{
    public static class SkillCaster
    {

        /// <summary>
        /// 群体治疗
        /// </summary>
        public static void HealMiutiCharacter(Skill s, List<Character> targets)
        {
            if (targets == null || targets.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 释放了 {1} ", s.Caster.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            foreach (Character t in targets)
            {
                //给目标加血
                t.HP += s.Atk;
                sb.AppendFormat(" | 对{0}造成了:{1}治疗效果", t.CharacterName, s.Atk.ToString());
                //添加一条Skada记录
                Skada.Instance.AddRecord(new SkadaRecord() { Accept = t, Source = s.Caster, UseSkill = s, Value = s.Atk });
            }

            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }

            //输出记录
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 单体治疗
        /// </summary>
        public static void HealCharacter(Skill s, Character target)
        {
            if (target == null || !target.IsAlive)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 对 {1} 释放了 {2} ", s.Caster.CharacterName, target.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //给目标加血
            target.HP += s.Atk;
            sb.AppendFormat(" | 造成了:{0}治疗效果", s.Atk.ToString());

            //添加一条Skada记录
            Skada.Instance.AddRecord(new SkadaRecord() { Accept = target, Source = s.Caster, UseSkill = s, Value = s.Atk });

            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }

            //输出记录
            Debug.Log(sb.ToString());
        }

        public static int AttackSingle(Skill s, Character target)
        {
            //掉血
            int damage = s.Atk;
            damage = (int)(damage * (1 - target.Defense));
            target.HP -= damage;
            return damage;
        }

        //向目标群体发动攻击 一会改
        public static void CastAOESkill(Skill s, List<Character> targets)
        {
            if (targets.Count == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 释放了 {1} ", s.Caster.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            foreach (Character t in targets)
            {
                int damage = AttackSingle(s, t);
                sb.AppendFormat("对{0} | 造成了:{1}伤害", t.CharacterName, damage.ToString());
                Skada.Instance.AddRecord(new SkadaRecord() { Accept = t, Source = s.Caster, UseSkill = s, Value = -damage });
            }

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
        }

        //向目标发动单体攻击
        public static void CastSingleSkill(Skill s, Character target)
        {
            if (target == null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 释放了 {1} ", s.Caster.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //掉血
            int damage = AttackSingle(s, target);
            sb.AppendFormat("对{0} | 造成了:{1}伤害", target.CharacterName, damage.ToString());
            Skada.Instance.AddRecord(new SkadaRecord() { Accept = target, Source = s.Caster, UseSkill = s, Value = -damage });

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
            //输出结果
            Debug.Log(sb.ToString());
        }

    }

}
