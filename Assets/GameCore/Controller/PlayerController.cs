using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HealerSimulator
{
    /// <summary>
    /// 玩家角色的控制类,操控都由这里做出.PlayerHUD只负责显示玩家状态
    /// 会有AIController 具有完整的操作权限,还有RobotController 只负责结算
    /// </summary>
    public class PlayerController
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
}