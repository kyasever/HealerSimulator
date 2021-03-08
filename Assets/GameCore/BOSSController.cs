using System.Collections.Generic;
using UnityEngine;

namespace HealerSimulator
{
    /// <summary>
    /// 这是一个boss 控制器,其难度由初始化参数设定
    /// </summary>
    public class BossController : Controller
    {
        /// <summary>
        /// 返回创建好的BOSS 和BOSS的技能伤害倍率
        /// </summary>
        public static Character CreateBossNormal(int diff, int hpMax, string name, out float m)
        {
            //创建并调整Boss的属性 HP每10层翻一倍
            int hp = (int)(hpMax * (1 + diff / 10f));
            Character c = new Character
            {
                CharacterName = "按部就班的便当王",
                MaxHP = hp,
                Crit = 0f,//BOSS可别爆击了
            };
            c.HP = c.MaxHP;

            float miuti = (float)System.Math.Sqrt(1 + diff * 0.1);
            //如果难度倍率超过4(急速倍率超过2) 那么急速锁定2 伤害倍率无限提高
            if (miuti > 2)
            {
                miuti = (1 + diff * 0.1f) / 2;
                c.Speed = 2;
            }
            else
            {
                c.Speed = miuti;
            }

            m = miuti;
            return c;
        }

        /// <summary>
        /// 由对应的控制器类负责创建对应的角色实例
        /// 好像又回到了wa的结构
        /// </summary>
        /// <returns></returns>
        public static BossController CreateBoss1(int difficultyLevel)
        {
            Character c = CreateBossNormal(difficultyLevel, 25000, "按部就班的便当王", out float miuti);

            //添加并绑定Boss的技能
            c.SkillList = new List<Skill>();

            //添加Controller
            BossController controller = new BossController(c);

            c.SkillList.Add(SkillBuilder.CreateSkillB1(c, miuti));
            c.SkillList.Add(SkillBuilder.CreateSkillB2(c, miuti));
            c.SkillList.Add(SkillBuilder.CreateSkillB3(c, miuti));

            return controller;
        }

        /// <summary>
        /// 由对应的控制器类负责创建对应的角色实例
        /// 好像又回到了wa的结构
        /// </summary>
        /// <returns></returns>
        public static BossController CreateBoss2(int difficultyLevel)
        {
            Character c = CreateBossNormal(difficultyLevel, 20000, "有狂暴的二号王", out float miuti);

            //添加并绑定Boss的技能
            c.SkillList = new List<Skill>();

            //添加Controller
            BossController controller = new BossController(c);

            c.SkillList.Add(SkillBuilder.CreateBOSS2_1(c, miuti));
            c.SkillList.Add(SkillBuilder.CreateBOSS2_2(c, miuti));
            c.SkillList.Add(SkillBuilder.CreateBOSS2_3(c, miuti));
            c.SkillList.Add(SkillBuilder.CreateBOSS2_4(c, miuti));

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
                    s.CastScript.Invoke(s, game);
                    s.CDRelease = s.CD;
                }
            }
        }
    }
}
