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

            //如果检测到控制的人挂了,那么解除关系,清空BUFF
            if (!c.IsAlive)
            {
                c.Buffs.Clear();
                c = null;
                return;
            }

            //正在进入CD的技能进行相应结算
            foreach (Skill s in c.SkillList)
            {
                if (s.CDRelease > 0)
                {
                    s.CDRelease -= Time.deltaTime;
                }
            }

            List<BUFF> needRemove = null;

            //驱动BUFF的持续时间和跳的时间
            foreach (BUFF buff in c.Buffs)
            {
                bool flag = false;
                if (buff.needRemove)
                {
                    flag = true;
                    if (needRemove == null)
                    {
                        needRemove = new List<BUFF>();
                    }
                    needRemove.Add(buff);
                    buff.PropChanged();
                    continue;
                }

                //持续时间没了移除 小于0说明持续时间无限
                if (buff.DefaultTime > 0)
                {
                    flag = true;
                    buff.ReleaseTime -= Time.deltaTime;
                    if (buff.ReleaseTime < 0)
                    {
                        if (needRemove == null)
                        {
                            needRemove = new List<BUFF>();
                        }
                        needRemove.Add(buff);
                    }
                }
                //周期性效果生效,吃急速效果
                if (buff.DefaultHot > 0)
                {
                    flag = true;
                    buff.ReleaseHot -= Time.deltaTime;
                    if (buff.ReleaseHot < 0)
                    {
                        buff.OnHot();
                        buff.ReleaseHot = buff.DefaultHot / c.Speed;
                    }
                }
                //光环也没周期的就不更新了
                if (flag)
                {
                    buff.PropChanged();
                }
            }

            if (needRemove != null)
            {
                foreach (var v in needRemove)
                {
                    c.Buffs.Remove(v);
                }
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
                Speed = 1.2f,
                Crit = 0.2f,
                Defense = 0f,
                CharacterName = "完美的操纵者",
                Description = "玩家控制单位",
            };
            c.HP = c.MaxHP;
            c.MP = c.MaxMP;

            c.SkillList.Add(SkillBuilder.CreateSkillP1(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP2(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP3(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP4(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP5(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP6(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP7(c));
            c.SkillList.Add(SkillBuilder.CreateSkillP8(c));
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
            //推动施法进度条,当技能施法时间结束时,释放这个技能
            if (c.IsCasting)
            {
                c.CastingSkill.CastingRelease -= Time.deltaTime;
                if (c.CastingSkill.CastingRelease < 0)
                {
                    //技能出手 如果出手的时候已经死掉了,那么不能出手
                    if (game.FocusCharacter.IsAlive)
                    {
                        c.CastingSkill.CastScript.Invoke(c.CastingSkill, game);
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
                        s.CastScript.Invoke(s, game);
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


        public static Character CreateNPC(NPCType type, int diff)
        {
            //每增加一层难度,队友的血量上限增加3%
            float miutiHP = 1 + (diff * 0.02f);
            Character c = new Character();
            int dps = 100;
            switch (type)
            {
                case NPCType.Tank:
                    c.Duty = TeamDuty.Tank;
                    c.CharacterName = "平庸的坦克";
                    c.MaxHP = 3200;
                    c.Defense = 0.3f;
                    dps = 50;
                    break;
                case NPCType.Mage:
                    c.Duty = TeamDuty.RangeDPS;
                    c.CharacterName = "粗心的法师";
                    c.MaxHP = 1760;
                    c.Defense = 0f;
                    dps = 160;
                    break;
                case NPCType.Warrior:
                    c.Duty = TeamDuty.MeleeDPS;
                    c.CharacterName = "鲁莽的斗士";
                    c.MaxHP = 2500;
                    c.Defense = -0.1f;
                    dps = 170;
                    break;
                case NPCType.Saber:
                    c.Duty = TeamDuty.MeleeDPS;
                    c.CharacterName = "可靠的武士";
                    c.MaxHP = 1800;
                    c.Defense = 0f;
                    c.Crit = 0.3f;
                    dps = 200;
                    break;
                default:
                    break;
            }
            c.MaxHP = (int)(c.MaxHP * miutiHP);
            c.HP = c.MaxHP;
            new NPCController(c, dps);
            return c;
        }

        public int PerHit = 100;

        public float HitSpeed = 1f;



        public NPCController(Character c, int dps) : base(c)
        {
            PerHit = dps;
            s = SkillBuilder.CreateSkillNPC1(c, HitSpeed);
            c.SkillList.Add(s);
        }

        private Skill s;

        public override void Update()
        {
            if (!game.InBattle)
            {
                return;
            }

            //这个技能冷却好了就用. 输出有一定随机因素
            if (s.CDRelease < 0)
            {
                int damage = (int)Random.Range(PerHit * 0.5f, PerHit * 1.5f);
                s.Power = -damage;
                s.Cast();
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
            if (game.TeamCharacters.Count == game.TeamDead.Count || !game.Boss.IsAlive)
            {
                game.InBattle = false;
                Global.Instance.GameEndPanel.gameObject.SetActive(true);
            }
        }
    }
}
