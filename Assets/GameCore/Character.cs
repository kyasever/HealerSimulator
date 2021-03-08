using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HealerSimulator
{
    public enum TeamDuty
    {
        Tank,
        Healer,
        MeleeDPS,
        RangeDPS,
    }

    /// <summary>
    /// 角色类 
    /// </summary>
    public class Character : IData
    {
        public Character()
        {
            Buffs = new BUFFContainer(this);
        }

        /// <summary>
        /// 上一条记录,被攻击了添加记录并PropChange
        /// </summary>
        public SkadaRecord BehitRecord;

        public TeamDuty Duty = TeamDuty.MeleeDPS;

        /// <summary>
        /// 急速. 取值1+ 2则为2倍速,0.5则为0.5倍速
        /// </summary>
        public float Speed = 1.2f;

        /// <summary>
        /// 暴击 取值0-1
        /// </summary>
        public float Crit = 0.2f;

        /// <summary>
        /// 进行一次暴击判定
        /// </summary>
        public bool CanCrit()
        {
            return UnityEngine.Random.Range(0, 1f) < Crit;
        }

        /// <summary>
        /// 减伤百分比
        /// </summary>
        public float Defense = 0f;

        /// <summary>
        /// 正在释放的法术
        /// </summary>
        public Skill CastingSkill = null;

        /// <summary>
        /// 是否正在施法
        /// </summary>
        public bool IsCasting { get { return this.CastingSkill != null; } }

        /// <summary>
        /// 公共cd = 1.5s / 急速加成
        /// </summary>
        public float CommonInterval { get { return 1.5f / Speed; } }

        /// <summary>
        /// 公cd剩余时间
        /// </summary>
        public float CommonTime = -1f;

        /// <summary>
        /// 持有的技能
        /// </summary>
        public List<Skill> SkillList = new List<Skill>();

        /// <summary>
        /// 角色名字
        /// </summary>
        public string CharacterName = " ";

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description = "  ";

        private int hp = 1;
        /// <summary>
        /// 当前生命
        /// </summary>
        public int HP
        {
            get => hp;
            set
            {
                if (!IsAlive)
                {
                    hp = 0;
                    return;
                }

                hp = value;
                if (hp > MaxHP)
                    hp = MaxHP;
                else if (hp <= 0)
                {
                    hp = 0;
                    IsAlive = false;
                }
            }
        }

        public bool IsAlive = true;

        /// <summary>
        /// 当前最大生命
        /// </summary>
        public int MaxHP = 3000;

        private int mp = 0;
        //第二资源条
        public int MP
        {
            get => mp;
            set
            {
                mp = value;
                if (mp > MaxMP)
                    mp = MaxMP;
                else if (mp <= 0)
                {
                    mp = 0;
                }
            }
        }
        public int MaxMP = 3000;

        public BUFFContainer Buffs;
    }



}
