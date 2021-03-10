using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HealerSimulator
{
    /// <summary>
    /// 取消继承,分为base和自定义控制
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// 需要手动调用这个函数来给Controller指明其负责的对象
        /// </summary>
        public BaseController(Character c)
        {
            GameMode.Instance.Connect(Refresh, Lifecycle.BaseControllerUpdate);
        }

        public void Refresh(GameMode gameMode)
        {
            foreach (var c in gameMode.TeamCharacters)
            {
                RefreshCharacter(c);
            }
        }

        public void RefreshCharacter(Character c)
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
