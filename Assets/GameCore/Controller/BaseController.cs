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
}
