using System;
using System.Collections.Generic;
using UnityEngine;

namespace HealerSimulator
{
    public class SkillBuilder
    {
        #region 技能释放逻辑
        private static void CastToFocus(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, game.FocusCharacter);
        }

        private static void CastToTeam(Skill s, GameMode game)
        {
            SkillCaster.CastMultiSkill(s, game.TeamCharacters);
        }

        private static void CastToBoss(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, game.Boss);
        }
        #endregion

        #region Player
        public static Skill CreateSkillP1(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.Q,
                Caster = caster,
                Power = 300,
                CastingDefaultInterval = 2.5f,
                MPCost = 30,
                skillName = "治疗术",
                skillDiscription = "恢复300点HP",
            };
            s.CastSctipt += CastToFocus;
            return s;
        }

        public static Skill CreateSkillP2(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.W,
                Caster = caster,
                Power = 600,
                CastingDefaultInterval = -1f,
                CDDefault = 10f,
                MPCost = 200,
                skillName = "快速治疗",
                skillDiscription = "瞬发,治疗600,并持续恢复",
            };
            s.CastSctipt += CastToFocus;
            return s;
        }

        public static Skill CreateSkillP3(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.E,
                Caster = caster,
                Power = 600,
                CastingDefaultInterval = 3f,
                MPCost = 150,
                skillName = "强效治疗",
                skillDiscription = "强而有效的单体治疗",
            };
            s.CastSctipt += CastToFocus;
            return s;
        }

        public static Skill CreateSkillP4(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.R,
                Caster = caster,
                Power = 200,
                CastingDefaultInterval = 3.5f,
                MPCost = 150,
                skillName = "治疗祷言",
                skillDiscription = "有效的群体治疗",
            };
            s.CastSctipt += CastToTeam;
            return s;
        }

        public static Skill CreateSkillP5(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.T,
                Caster = caster,
                CDDefault = 60f,
                Power = 1000,
                CastingDefaultInterval = -1f,
                MPCost = 100,
                skillName = "救赎祷言",
                skillDiscription = "应急群体治疗",
            };
            s.CastSctipt += CastToTeam;
            return s;
        }

        public static Skill CreateSkillP6(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.F,
                Caster = caster,
                CDDefault = 0f,
                Power = -500,
                CastingDefaultInterval = 3f,
                MPCost = 0,
                skillName = "圣光之箭",
                skillDiscription = "对Boss造成中等的单体伤害",
            };
            s.CastSctipt += CastToBoss;
            return s;
        }
        #endregion

        #region NPC

        public static Skill CreateSkillNPC1(Character caster,float cd)
        {
            Skill s = new Skill()
            {
                Caster = caster,
                Power = -1,
                CDDefault = cd,
                CDRelease = 0.1f,
                skillName = caster.CharacterName + "的攻击",
                skillDiscription = "造成伤害",
            };
            s.CastSctipt += CastToBoss;
            return s;
        }
        #endregion
    }

    /// <summary>
    /// 技能被打出去之后就变成了SkillInstace,这个技能只参与加成与结算.不参与cd等
    /// </summary>
    public class SkillInstance
    {

        public SkillInstance(Skill s,Character target)
        {
            ID = s.ID;
            Value = s.Power;
            Caster = s.Caster;
            this.Target = target;
        }

        public int Value;

        public Character Caster;

        public Character Target;

        public int ID;
    }

    /// <summary>
    /// 技能类. 考虑使用继承进行扩展. 技能的释放是一个权限很大的函数,它可以获得游戏中的所有的数据并决定该干什么.
    /// </summary>
    public class Skill : IDataBinding
    {
        public enum DebugType
        {
            DebugLog = 1,
            None = 2,
        }
        /// <summary>
        /// 输出优先级,优先级低的向日志输出,优先级高的向Debug输出
        /// </summary>
        public DebugType DebugOutLevel = DebugType.DebugLog;

        public static int GlobalID = 0;
        public static List<Skill> ID_SKill = new List<Skill>();
        public int ID = 1;
        //每被创建一次 ID都会自增
        public Skill()
        {
            ID = GlobalID;
            ID_SKill.Add(this);
            GlobalID++;
        }

        public List<Action> OnChangeEvent { get; set; } = new List<Action>();

        public void PropChanged()
        {
            foreach (Action a in OnChangeEvent)
            {
                a.Invoke();
            }
        }

        public string skillName = "技能名";
        public string skillDiscription = "技能效果";

        /// <summary>
        /// 和伤害还是治疗无关,单纯的为正就是加血 为负就是减血
        /// </summary>
        public int Power = -100;

        /// <summary>
        /// 绑定按键
        /// </summary>
        public KeyCode Key;

        /// <summary>
        /// 这个技能的释放者
        /// </summary>
        public Character Caster;

        /// <summary>
        /// 技能被释放时执行的操作,通常使用静态函数进行托管,特定技能需要编写特定的静态函数进行处理
        /// 参数Skill    代表攻击者  
        /// </summary>
        public Action<Skill, GameMode> CastSctipt;

        /// <summary>
        /// 不太好... CastSctipt?.Invoke(this,GameMode.Instance);
        /// </summary>
        public void Cast()
        {
            CastSctipt?.Invoke(this,GameMode.Instance);
        }

        public SkillInstance CreateInstance(Character target)
        {
            return new SkillInstance(this,target);
        }

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
        public float CD => CDDefault / Caster.Speed;

        private float cdRelease = -1f;
        /// <summary>
        /// 剩余CD 小于0表示冷却好了
        /// </summary>
        public float CDRelease { get => cdRelease; set { cdRelease = value; PropChanged(); } }

        private float castingRelease = -1f;
        /// <summary>
        /// 剩余施法时间
        /// </summary>
        public float CastingRelease { get => castingRelease; set { castingRelease = value; PropChanged(); } }

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
        public bool IsInstant => (CastingDefaultInterval < 0);

        /// <summary>
        /// 实际施法时间
        /// </summary>
        public float CastingInterval => CastingDefaultInterval / Caster.Speed;
    }
}
