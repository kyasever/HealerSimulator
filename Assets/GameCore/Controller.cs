using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HealerSimulator
{
    /// <summary>
    /// 控制类基类,具有一些api
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Characer和其Skill是由纯数据组成的,Controller负责操作它们的数据
        /// </summary>
        public Character c;
        protected GameMode game;

        /// <summary>
        /// 需要手动调用这个函数来给Controller指明其负责的对象
        /// </summary>
        public Controller(Character c)
        {
            this.c = c;
            game = GameMode.Instance;
            //将自己的Update托管给gamemode,然后场景从gameMode取数据进行更新
            game.UpdateEvent += DefaultUpdate;
            game.UpdatePerSecendEvent += DefaultTickPerSecond;
        }

        protected void DefaultTickPerSecond()
        {
            if (c == null || !c.IsAlive)
            {
                return;
            }
            TickPerSecond();
        }
        public virtual void TickPerSecond()
        {

        }


        protected void DefaultUpdate()
        {
            if (c == null)
            {
                return;
            }

            //如果检测到控制的人挂了,那么解除关系
            if (!c.IsAlive)
            {
                game.DeadCharacters.Add(c);
                c = null;
                return;
            }
            //驱动每帧事件
            Update();
        }

        public virtual void Update()
        {

        }

    }

    /// <summary>
    /// 玩家角色的控制类,操控都由这里做出.PlayerHUD只负责显示玩家状态
    /// 会有AIController 具有完整的操作权限,还有RobotController 只负责结算
    /// </summary>
    public class PlayerController : Controller
    {
        /// <summary>
        /// 使用静态方法创建一个标准玩家控制的角色,这个角色的职业是Paladin
        /// </summary>
        /// <returns></returns>
        public static Character CreatePlayer(int difficultyLevel)
        {
            Character c = new Character()
            {
                //人物的基础属性
                Stama = 46,
                Speed = 1.2f,
                Crit = 0.2f,
                Inte = 55,
                Master = 0f,
                Defense = 0f,
                MaxAP = 0,
                CharacterName = "完美的操纵者",
                Description = "玩家控制单位",
            };
            c.HP = c.MaxHP;
            c.MP = c.MaxMP;
            c.AP = c.MaxAP;
            c.Evasion = 1.5f - difficultyLevel * 0.1f;

            c.SkillList.Add(PlayerSKill.CreateSkillP1(c));
            c.SkillList.Add(PlayerSKill.CreateSkillP2(c));
            c.SkillList.Add(PlayerSKill.CreateSkillP3(c));
            c.SkillList.Add(PlayerSKill.CreateSkillP4(c));
            c.SkillList.Add(PlayerSKill.CreateSkillP5(c));
            c.SkillList.Add(PlayerSKill.CreateSkillP6(c));
            new PlayerController(c);
            return c;
        }

        public PlayerController(Character c) : base(c)
        {

        }

        /// <summary>
        /// 每秒触发一次,结算每秒的操作
        /// </summary>
        public override void TickPerSecond()
        {
            c.MP += 20;
        }

        /// <summary>
        /// 每帧结算一次,结算CD和施法操作等
        /// </summary>
        public override void Update()
        {
            //减少CD
            foreach (Skill s in c.SkillList)
            {
                if (s.CDRelease > 0)
                {
                    s.CDRelease -= Time.deltaTime;
                }
            }

            //推动施法进度条,当技能施法时间结束时,释放这个技能
            if (c.IsCasting)
            {
                c.CastingSkill.CastingRelease -= Time.deltaTime;
                if (c.CastingSkill.CastingRelease < 0)
                {
                    //技能出手 如果出手的时候已经死掉了,那么不能出手
                    if (game.FocusCharacter.IsAlive)
                    {
                        c.CastingSkill.OnCastEvent.Invoke(c.CastingSkill, game);
                    }

                    c.CastingSkill = null;
                }
                //空格键打断当前施法
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    c.CastingSkill = null;
                }
            }

            //如果公cd> 0 那么处理公cd且不能释放别的技能
            if (c.CommonTime > 0f)
            {
                c.CommonTime -= Time.deltaTime;
                return;
            }

            //按下对应的按键后执行对应的操作
            foreach (Skill s in c.SkillList)
            {
                //按下对应按键且可以释放(蓝耗,技能,目标等没问题)
                if (Input.GetKeyDown(s.Key) && s.CanCast)
                {
                    //瞬发技能,直接释放,并进入公cd
                    if (s.IsInstant)
                    {
                        //打断当前读条技能
                        if (c.IsCasting)
                        {
                            c.CastingSkill = null;
                        }
                        s.OnCastEvent.Invoke(s, game);
                        c.CommonTime = c.CommonInterval;
                    }
                    //读条技能,开始读条
                    else
                    {
                        //读条技能不能打断读条技能
                        if (c.IsCasting)
                        {
                            return;
                        }
                        //进入公cd
                        c.CommonTime = c.CommonInterval;
                        //开始读条
                        c.CastingSkill = s;
                        s.CastingRelease = s.CastingInterval;
                    }
                }
            }
        }
    }


    public class NPCController : Controller
    {
        public enum NPCType
        {
            Tank,
            Mage,
            Warrior,
            Saber,
        }


        public static Character CreateNPC(NPCType type)
        {
            Character c = new Character();
            int dps = 100;
            switch (type)
            {
                case NPCType.Tank:
                    c.Duty = TeamDuty.Tank;
                    c.CharacterName = "平庸的坦克";
                    c.Evasion = 0.2f;
                    c.MaxHP = 3200;
                    c.Defense = 0.3f;
                    dps = 50;
                    break;
                case NPCType.Mage:
                    c.Duty = TeamDuty.RangeDPS;
                    c.CharacterName = "粗心的法师";
                    c.MaxHP = 1760;
                    c.Evasion = 0.7f;
                    c.Defense = 0f;
                    dps = 150;
                    break;
                case NPCType.Warrior:
                    c.Duty = TeamDuty.MeleeDPS;
                    c.CharacterName = "鲁莽的斗士";
                    c.MaxHP = 2500;
                    c.Evasion = 0.2f;
                    c.Defense = -0.1f;
                    dps = 150;
                    break;
                case NPCType.Saber:
                    c.Duty = TeamDuty.MeleeDPS;
                    c.CharacterName = "可靠的武士";
                    c.MaxHP = 1800;
                    c.Evasion = 0.8f;
                    c.Defense = 0f;
                    dps = 200;
                    break;
                default:
                    break;
            }
            new NPCController(c, dps);
            return c;
        }

        public int DPS = 100;

        public NPCController(Character c,int dps) : base(c)
        {
            DPS = dps;
        }

        public override void TickPerSecond()
        {
            if (c == null)
            {
                return;
            }

            if (!game.InBattle)
            {
                return;
            }


            int damage = (int)Random.Range(DPS * 0.5f,DPS * 1.5f);
            GameMode.Instance.Boss.HP -= damage;
            Skada.Instance.AddRecord(new SkadaRecord()
            {
                Accept = GameMode.Instance.Boss,
                Source = c,
                UseSkill = null,
                Value = -damage
            });
        }
    }

    /// <summary>
    /// 这是一个boss 控制器,其难度由初始化参数设定
    /// </summary>
    public class BossController : Controller
    {
        public static Skill CreateNormalHitSkill(Character caster, string name, int atk, float cd)
        {
            Skill s = new Skill()
            {
                Caster = caster,
                Atk = atk,
                CDDefault = cd,
                skillName = name,
            };
            return s;
        }

        /// <summary>
        /// 由对应的控制器类负责创建对应的角色实例
        /// 好像又回到了wa的结构
        /// </summary>
        /// <returns></returns>
        public static BossController CreateBoss(int difficultyLevel)
        {
            //创建并调整Boss的属性
            int hp = (int)(25000 * (1 + difficultyLevel / 10f));
            Character c = new Character
            {
                CharacterName = "BOSS",
                MaxHP = hp
            };
            c.HP = c.MaxHP;

            float miuti = (float)System.Math.Sqrt(1 + difficultyLevel * 0.1);
            c.Speed = miuti;

            //添加并绑定Boss的技能
            c.SkillList = new List<Skill>();
            Skill skill = CreateNormalHitSkill(c, "地震", (int)(30 * miuti), 2f);
            skill.CDRelease = 1f;
            skill.OnCastEvent += CastSkill1;
            c.SkillList.Add(skill);

            skill = CreateNormalHitSkill(c, "重击", (int)(1200 * miuti), 20f);
            skill.CDRelease = skill.CD /2;
            skill.OnCastEvent += CastSkill2;
            c.SkillList.Add(skill);

            skill = CreateNormalHitSkill(c, "流火", (int)(450 * miuti), 20f);
            skill.CDRelease = skill.CD;
            skill.OnCastEvent += CastSkill3;
            c.SkillList.Add(skill);

            //添加Controller
            BossController controller = new BossController(c);
            return controller;
        }

        /// <summary>
        /// 难度等级每增加10 boss的输出增加1倍
        /// </summary>
        /// <param name="c"></param>
        /// <param name="diffcultyLevel"></param>
        public BossController(Character c) : base(c)
        {
        }

        private static void CastSkill1(Skill s, GameMode game)
        {
            SkillCaster.CastAOESkill(s, game.TeamCharacters);
        }

        private static void CastSkill2(Skill s, GameMode game)
        {
            SkillCaster.CastSingleSkill(s, GetTank(game));
        }

        private static void CastSkill3(Skill s, GameMode game)
        {
            List<Character> list = new List<Character>();
            foreach (Character c in game.TeamCharacters)
            {
                if (c.CanHit(0f))
                {
                    list.Add(c);
                }
            }
            SkillCaster.CastAOESkill(s, list);
        }

        /// <summary>
        /// 获得一个坦克单位,如果没有,那么打第一个人
        /// </summary>
        private static Character GetTank(GameMode game)
        {
            foreach (Character v in game.TeamCharacters)
            {
                if (v.Duty == TeamDuty.Tank)
                {
                    return v;
                }
            }
            if (game.TeamCharacters.Count > 0)
            {
                return game.TeamCharacters[0];
            }
            else
            {
                return null;
            }
        }


        //卡cd释放技能
        public override void Update()
        {
            if (!game.InBattle)
            {
                return;
            }
            if (game.TeamCharacters.Count == 0)
            {
                return;
            }
            foreach (Skill s in c.SkillList)
            {
                s.CDRelease -= Time.deltaTime;
                if (s.CDRelease < 0)
                {
                    s.OnCastEvent.Invoke(s, game);
                    s.CDRelease = s.CD;
                }

            }

        }
    }

    /// <summary>
    /// 游戏控制器,判断游戏是否结束了
    /// </summary>
    public class GameContrtoller
    {
        private GameMode game;

        public GameContrtoller()
        {
            game = GameMode.Instance;
            GameMode.Instance.UpdateEvent += Update;
        }

        public void Update()
        {
            if (!game.InBattle)
            {
                return;
            }
            //全死光了
            if (game.TeamCharacters.Count == game.DeadCharacters.Count)
            {
                Debug.Log("游戏结束");
                game.InBattle = false;
                Global.Instance.GameEndPanel.gameObject.SetActive(true);
            }
            else if (!game.Boss.IsAlive)
            {
                Debug.Log("游戏胜利");
                game.InBattle = false;
                Global.Instance.GameEndPanel.gameObject.SetActive(true);
            }
        }
    }
}
