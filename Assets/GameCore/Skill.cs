using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HealerSimulator
{
    public class PlayerSKill
    {
        private static void CastSingle(Skill s, GameMode game)
        {
            SkillCaster.HealCharacter(s, game.FocusCharacter);
        }

        private static void CastAOE(Skill s, GameMode game)
        {
            SkillCaster.HealMiutiCharacter(s, game.TeamCharacters);
        }

        private static void CastAttackBoss(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, game.Boss);
        }

        public static Skill CreateSkillP1(Character caster)
        {
            Skill s = new Skill()
            {
                Key =  KeyCode.Q,
                Caster = caster,
                Atk = 300,
                CastingDefaultInterval = 2.5f,
                MPCost = 30,
                skillName = "治疗术",
                skillDiscription = "恢复300点HP",
            };
            s.OnCastEvent += CastSingle;
            return s;
        }

        public static Skill CreateSkillP2(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.W,
                Caster = caster,
                Atk = 600,
                CastingDefaultInterval = -1f,
                CDDefault = 10f,
                MPCost = 200,
                skillName = "快速治疗",
                skillDiscription = "瞬发,治疗600,并持续恢复",
            };
            s.OnCastEvent += CastSingle;
            return s;
        }

        public static Skill CreateSkillP3(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.E,
                Caster = caster,
                Atk = 600,
                CastingDefaultInterval = 3f,
                MPCost = 150,
                skillName = "强效治疗",
                skillDiscription = "强而有效的单体治疗",
            };
            s.OnCastEvent += CastSingle;
            return s;
        }

        public static Skill CreateSkillP4(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.R,
                Caster = caster,
                Atk = 200,
                CastingDefaultInterval = 3.5f,
                MPCost = 150,
                skillName = "治疗祷言",
                skillDiscription = "有效的群体治疗",
            };
            s.OnCastEvent += CastAOE;
            return s;
        }

        public static Skill CreateSkillP5(Character caster)
        {
            Skill s = new Skill()
            {
                Key =  KeyCode.T,
                Caster = caster,
                CDDefault = 60f,
                Atk = 1000,
                CastingDefaultInterval = -1f,
                MPCost = 100,
                skillName = "救赎祷言",
                skillDiscription = "应急群体治疗",
            };
            s.OnCastEvent += CastAOE;
            return s;
        }

        public static Skill CreateSkillP6(Character caster)
        {
            Skill s = new Skill()
            {
                Key =  KeyCode.F,
                Caster = caster,
                CDDefault = 0f,
                Atk = 500,
                CastingDefaultInterval = 3f,
                MPCost = 0,
                skillName = "圣光之箭",
                skillDiscription = "对Boss造成中等的单体伤害",
            };
            s.OnCastEvent += CastAttackBoss;
            return s;
        }
    }

    /// <summary>
    /// 技能类. 考虑使用继承进行扩展. 技能的释放是一个权限很大的函数,它可以获得游戏中的所有的数据并决定该干什么.
    /// </summary>
    public class Skill
    {
        public string skillName = "技能名";
        public string skillDiscription = "技能效果";

        public int Atk = 100;

        //最终攻击,在攻击判定的第一步,就是将最终攻击同步为攻击,然后计算加成.最后出手
        public int FinalATK = 100;

        //技能的绑定按键
        public KeyCode Key;

        //这个技能的释放者
        public Character Caster;

        /// <summary>
        /// 技能被释放时执行的操作,通常使用静态函数进行托管,特定技能需要编写特定的静态函数进行处理
        /// 参数Skill    代表攻击者  
        /// </summary>
        public Action<Skill,GameMode> OnCastEvent;

        /// <summary>
        /// 蓝耗
        /// </summary>
        public int MPCost = 10;

        /// <summary>
        /// 默认cd
        /// </summary>
        public float CDDefault = -1f;

        /// <summary>
        /// 实际CD
        /// </summary>
        public float CD { get => CDDefault / Caster.Speed; }

        /// <summary>
        /// 剩余CD 小于0表示冷却好了
        /// </summary>
        public float CDRelease = -1;

        /// <summary>
        /// 剩余施法时间
        /// </summary>
        public float CastingRelease = -1f;

        /// <summary>
        /// 默认施法时间,负数表示为瞬发技能
        /// </summary>
        public float CastingDefaultInterval = 1.5f;

        /// <summary>
        /// 判断是否满足这个技能的使用条件
        /// </summary>
        public bool CanCast
        {
            get
            {
                if (Caster.MP < MPCost)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 判断是否是瞬发技能
        /// </summary>
        public bool IsInstant { get { return (CastingDefaultInterval < 0); } }

        /// <summary>
        /// 实际施法时间
        /// </summary>
        public float CastingInterval { get => CastingDefaultInterval / Caster.Speed; }
    }
}
