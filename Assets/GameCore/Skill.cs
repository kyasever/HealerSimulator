namespace HealerSimulator
{
    public enum SkillID
    {
        冲击 = 1,
    }

    public enum SkillType
    {
        single,
        multi,
    }

    /// <summary>
    /// 技能类. 角色下的子对象,表示角色可以进行的行为
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// 这个技能所属的角色
        /// </summary>
        public Character Caster;

        /// <summary>
        /// ID 唯一编号,通常使用ID进行Skill标识
        /// </summary>
        public int ID = 1;

        /// <summary>
        /// 技能名,用于显示
        /// </summary>
        public string skillName = "技能名";

        /// <summary>
        /// 技能描述,用于展示
        /// </summary>
        public string skillDescription = "技能效果";

        /// <summary>
        /// 对生命值的影响, 负数代表是伤害,正数代表是治疗
        /// </summary>
        public float Power = -100;

        /// <summary>
        /// 技能释放的逻辑.
        /// </summary>
        public SkillType skillType;

        /// <summary>
        /// 法力消耗
        /// </summary>
        public int MPCost = 10;

        /// <summary>
        /// 默认cd, 负数表示无CD
        /// </summary>
        public float CDDefault = -1f;

        /// <summary>
        /// 实际CD
        /// </summary>
        public float CD => (CDDefault / Caster.Speed);

        /// <summary>
        /// 剩余CD, 负数表示冷却好了
        /// </summary>
        public float CDRelease = -1f;

        /// <summary>
        /// 默认施法时间,负数表示不需要施法,可以直接释放
        /// </summary>
        public float CastingDefaultInterval = 1.5f;

        /// <summary>
        /// 施法时间 负数表示不需要施法,可以直接释放
        /// </summary>
        public float CastingInterval => (CastingDefaultInterval / Caster.Speed);

        /// <summary>
        /// 是否是瞬发技能
        /// </summary>
        public bool IsInstant => (CastingDefaultInterval < 0);
    }
}
