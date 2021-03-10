using System.Collections.Generic;
namespace HealerSimulator {
    // 只有近战-远程 区分,和站位区分
    public enum TeamDuty {
        Melee,
        Range
    }

    /// <summary>
    /// 角色类 
    /// </summary>
    public class Character {

        #region 角色基础属性
        /// <summary>
        /// 角色名字
        /// </summary>
        public string CharacterName = " ";

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description = "  ";

        /// <summary>
        /// 位置, 近战 | 远程
        /// </summary>
        public TeamDuty Duty = TeamDuty.Melee;

        /// <summary>
        /// 急速. 几倍速就是几
        /// </summary>
        public float Speed = 1.2f;

        /// <summary>
        /// 暴击 取值0-1, 1必定暴击
        /// </summary>
        public float Crit = 0.2f;

        /// <summary>
        /// 进行一次暴击判定
        /// </summary>
        public bool IsCrit => UnityEngine.Random.Range(0, 1f) < Crit;

        /// <summary>
        /// 默认减伤百分比
        /// </summary>
        public float Defense = 0f;


        #endregion

        #region 角色战斗属性

        private int hp = 1;
        public int HP {
            get => hp;
            set {
                if (!IsAlive) {
                    return;
                }
                if (hp > MaxHP) {
                    hp = MaxHP;
                } else if (hp <= 0) {
                    hp = 0;
                    IsAlive = false;
                } else {
                    hp = MaxHP;
                }
            }
        }

        /// <summary>
        /// 是否存活
        /// </summary>
        public bool IsAlive = true;

        public int MaxHP = 3000;

        private int mp = 0;

        public int MP {
            get => mp;
            set {
                mp = value;
                if (mp > MaxMP)
                    mp = MaxMP;
                else if (mp <= 0) {
                    mp = 0;
                }
            }
        }

        public int MaxMP = 3000;

        #endregion

        #region 状态属性
        /// <summary>
        /// 正在释放的法术
        /// </summary>
        public Skill CastingSkill = null;

        /// <summary>
        /// 是否正在施法
        /// </summary>
        public bool IsCasting => this.CastingSkill != null;

        /// <summary>
        /// 公共cd = 1.5s / 急速加成
        /// </summary>
        public float CommonInterval => 1.5f / Speed;

        /// <summary>
        /// 公cd剩余时间
        /// </summary>
        public float CommonTime = -1f;

        #endregion

        #region 子属性

        /// <summary>
        /// 持有的技能
        /// </summary>
        public List<Skill> SkillList = new List<Skill>();


        public BUFFContainer Buffs;
        #endregion

    }
}
