using System.Collections.Generic;
using UnityEngine;

namespace HealerSimulator
{
    /// <summary>
    /// 这是一个boss 控制器,其难度由初始化参数设定
    /// </summary>
    public class BossController : Controller
    {
        public Skill CreateSkillB1(float miuti)
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
            s.CastSctipt += CastSkill1;
            return s;
        }

        public Skill CreateSkillB2(float miuti)
        {
            Skill s = new Skill()
            {
                Caster = c,
                Power = (int)(-30 * miuti),
                CDDefault = 2f,
                skillName = "重击",
                skillDiscription = "对坦克造成大量伤害"

            };
            s.CDRelease = s.CD / 2;
            s.CastSctipt += CastSkill2;
            return s;
        }

        public Skill CreateSkillB3(float miuti)
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
            s.CastSctipt += CastSkill3;
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
                CharacterName = "按部就班的便当王",
                MaxHP = hp
            };
            c.HP = c.MaxHP;

            float miuti = (float)System.Math.Sqrt(1 + difficultyLevel * 0.1);
            c.Speed = miuti;

            //添加并绑定Boss的技能
            c.SkillList = new List<Skill>();

            //添加Controller
            BossController controller = new BossController(c);

            c.SkillList.Add(controller.CreateSkillB1(miuti));
            c.SkillList.Add(controller.CreateSkillB2(miuti));
            c.SkillList.Add(controller.CreateSkillB3(miuti));

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
            SkillCaster.CastMultiSkill(s, game.TeamCharacters);
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
                if (c.IsAlive && c.CanHit(0f))
                {
                    list.Add(c);
                }
            }
            SkillCaster.CastMultiSkill(s, list);
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

            foreach (Character v in game.TeamCharacters)
            {
                if (v.IsAlive)
                {
                    return v;
                }
            }

            return null;
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
                if (s.CDRelease < 0)
                {
                    s.CastSctipt.Invoke(s, game);
                    s.CDRelease = s.CD;
                }
            }

        }
    }
}
