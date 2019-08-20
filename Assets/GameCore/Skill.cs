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
            SkillCaster.CastMultiSkill(s, game.TeamAlive);
        }

        private static void CastToBoss(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, game.Boss);
        }

        /// <summary>
        /// 神圣之光,直接治疗并添加一个HOT 持续治疗
        /// </summary>
        private static void CastHolyLight(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, game.FocusCharacter);
            HotBuff buff = new HotBuff(s.Caster, 18f, 3f, 100);
            game.FocusCharacter.Buffs.Add(buff);
        }

        private static void CastShield(Skill s, GameMode game)
        {
            SkillCaster.CastToVoid(s);
            ShieldBUFF buff = new ShieldBUFF(s.Caster, 10f, s.Power);
            game.FocusCharacter.Buffs.Add(buff);
        }

        private static void CastTeamBuff(Skill s, GameMode game)
        {
            SkillCaster.CastToVoid(s);
            foreach (Character v in game.TeamCharacters)
            {
                PTeamBuff buff = new PTeamBuff(s.Caster);
                v.Buffs.Add(buff);
            }
        }
        #endregion

        #region Player
        //神圣之光 瞬抬+hot
        public static Skill CreateSkillP1(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.Q,
                Caster = caster,
                Power = 600,
                CastingDefaultInterval = -1f,
                CDDefault = 12f,
                MPCost = 200,
                skillName = "神圣之光",
                skillDiscription = "瞬发,治疗600,并持续恢复",
            };
            s.CastSctipt += CastHolyLight;
            return s;
        }
        //治疗术
        public static Skill CreateSkillP2(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.W,
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
        //强效治疗
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
        //祷言
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
        //爆发群抬
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
        //攻击技能
        public static Skill CreateSkillP6(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.A,
                Caster = caster,
                CDDefault = -1f,
                Power = -500,
                CastingDefaultInterval = 2.5f,
                MPCost = -100,
                skillName = "圣光之箭",
                skillDiscription = "对BOSS造成伤害,并恢复少量蓝",
            };
            s.CastSctipt += CastToBoss;
            return s;
        }
        //真言术盾
        public static Skill CreateSkillP7(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.F,
                Caster = caster,
                Power = 600,
                CastingDefaultInterval = -1f,
                CDDefault = 15f,
                MPCost = 200,
                skillName = "真言术盾",
                skillDiscription = "瞬发,为目标添加护盾,持续10s,可以抵挡600伤害",
            };
            s.CastSctipt += CastShield;
            return s;
        }
        //嗜血
        public static Skill CreateSkillP8(Character caster)
        {
            Skill s = new Skill()
            {
                Key = KeyCode.G,
                Caster = caster,
                CastingDefaultInterval = -1f,
                CDDefault = 300f,
                MPCost = 300,
                skillName = "神圣强化",
                skillDiscription = "为全体友方单位添加神圣强化效果,攻击和治疗强度提升30%",
            };
            s.CastSctipt += CastTeamBuff;
            return s;
        }

        #endregion

        #region NPC

        public static Skill CreateSkillNPC1(Character caster, float cd)
        {
            Skill s = new Skill()
            {
                Caster = caster,
                Power = -1,
                CDDefault = cd,
                CDRelease = 0.1f,
                skillName = caster.CharacterName + "的攻击",
                skillDiscription = "造成伤害",
                DebugOutLevel = Skill.DebugType.None,
            };
            s.CastSctipt += CastToBoss;
            return s;
        }
        #endregion

        #region BOSS1
        public static Skill CreateSkillB1(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-30 * miuti),
                CDDefault = 2f,
                skillName = "地震",
                CDRelease = 1f,
                skillDiscription = "短间隔全体AOE",
                DebugOutLevel = Skill.DebugType.None,
            };
            s.CastSctipt += CastToTeam;
            return s;
        }

        public static Skill CreateSkillB2(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-1200 * miuti),
                CDDefault = 20f,
                skillName = "重击",
                skillDiscription = "对坦克造成大量伤害"

            };
            s.CDRelease = s.CD / 2;
            s.CastSctipt += CastToTank;
            return s;
        }

        private static void CastToTank(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, GetTank(game));
        }

        /// <summary>
        /// 获得一个坦克单位,如果没有,那么打第一个人
        /// </summary>
        private static Character GetTank(GameMode game)
        {
            foreach (Character v in game.TeamCharacters)
            {
                if (v.Duty == TeamDuty.Tank && v.IsAlive)
                {
                    return v;
                }
            }

            foreach (Character v in game.TeamCharacters)
            {
                if (v.IsAlive)
                {
                    return v;
                }
            }

            return null;
        }

        /// <summary>
        /// 随机向团队发射,命中取决于闪避
        /// </summary>
        /// <param name="s"></param>
        /// <param name="game"></param>
        private static void CastToTeamRandom2(Skill s, GameMode game)
        {
            List<Character> list = new List<Character>();
            foreach (Character c in game.TeamCharacters)
            {
                if (c.IsAlive)
                {
                    list.Add(c);
                }
            }
            //取随机两次
            List<Character> re = new List<Character>();
            int index = UnityEngine.Random.Range(0, list.Count - 1);
            re.Add(list[index]);

            list.RemoveAt(index);
            index = UnityEngine.Random.Range(0, list.Count - 1);
            re.Add(list[index]);

            SkillCaster.CastMultiSkill(s, re);
        }

        public static Skill CreateSkillB3(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-450 * miuti),
                CDDefault = 20f,
                skillName = "流火",
                skillDiscription = "随机点名造成大量伤害",

            };
            s.CDRelease = s.CD;
            s.CastSctipt += CastToTeamRandom2;
            return s;
        }
        #endregion

        #region BOSS2
        public static Skill CreateBOSS2_1(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-150 * miuti),
                CDDefault = 2f,
                skillName = "普通攻击",
                
                skillDiscription = "对坦克造成少量单体伤害",
                DebugOutLevel = Skill.DebugType.None,
            };
            s.CDRelease = s.CD;
            s.CastSctipt += CastToTank;
            return s;
        }

        public static Skill CreateBOSS2_2(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-2000 * miuti),
                CDDefault = 25f,
                skillName = "毁灭重击",
                skillDiscription = "对坦克造成毁灭性的打击",
            };
            s.CDRelease = s.CD/2;
            s.CastSctipt += CastToTank;
            return s;
        }

        private static void CastBOSS2_3(Skill s, GameMode game)
        {
            SkillCaster.CastToVoid(s);

            BoomDeBUFF debuff = new BoomDeBUFF(s.skillName, s.Caster, 10f, s.Power);

            if (game.TeamDic[TeamDuty.MeleeDPS].Count > 0)
            {
                List<Character> list = game.TeamDic[TeamDuty.MeleeDPS];
                list[UnityEngine.Random.Range(0, list.Count - 1)].Buffs.Add(debuff);
            }
            else if(game.TeamDic[TeamDuty.Tank].Count > 0)
            {
                List<Character> list = game.TeamDic[TeamDuty.Tank];
                list[UnityEngine.Random.Range(0, list.Count - 1)].Buffs.Add(debuff);
            }

            debuff = new BoomDeBUFF(s.skillName, s.Caster, 10f, s.Power);
            if (game.TeamDic[TeamDuty.RangeDPS].Count > 0)
            {
                List<Character> list = game.TeamDic[TeamDuty.RangeDPS];
                list[UnityEngine.Random.Range(0, list.Count - 1)].Buffs.Add(debuff);
            }
            else if(game.TeamDic[TeamDuty.Healer].Count > 0)
            {
                List<Character> list = game.TeamDic[TeamDuty.Healer];
                list[UnityEngine.Random.Range(0, list.Count - 1)].Buffs.Add(debuff);
            }
        }

        public static Skill CreateBOSS2_3(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-1200 * miuti),
                CDDefault = 35f,
                skillName = "幽灵火",
                skillDiscription = "随机点名1近1远,10s后造成大量伤害",
            };
            s.CDRelease = s.CD;
            s.CastSctipt += CastBOSS2_3;
            return s;
        }

        private static void CastBOSS2_4(Skill s, GameMode game)
        {
            s.Power = (int)(s.CustomInt * (1 + game.BattleTime / 10f * 0.1f));
            CastToTeam(s, game);
        }

        public static Skill CreateBOSS2_4(Character c, float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                CustomInt = (int)(-400 * miuti),
                CDDefault = 16f,
                skillName = "烈焰灼烧",
                skillDiscription = "AOE高额伤害,每秒伤害提高1%",
            };
            s.CDRelease = s.CD;
            s.CastSctipt += CastBOSS2_4;
            return s;
        }
        #endregion
    }

    /// <summary>
    /// 技能被打出去之后就变成了SkillInstace,这个技能只参与加成与结算.不参与cd等
    /// </summary>
    public class SkillInstance
    {
        public SkillInstance()
        {

        }

        public SkillInstance(Skill s, Character target)
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
            CastSctipt?.Invoke(this, GameMode.Instance);
        }

        public SkillInstance CreateInstance(Character target)
        {
            return new SkillInstance(this, target);
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


        /// <summary>
        /// 一个自定义的技能数据,用于搞事情
        /// </summary>
        public float CustomFloat = 1f;

        /// <summary>
        /// 一个自定义的技能数据,用于搞事情
        /// </summary>
        public int CustomInt = 1;
    }
}
