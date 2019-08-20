using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * 1.4 版本 新的开始
 * 优化更新:
 *      增加面板模块,开始界面和结束界面等.
 *      优化初始化流程和伤害判定流程
 *      增加多种NPC控制器,区分NPC的操作.
 * 平衡性更新:
 *      给牧师增加了一个攻击技能 可以在低难度打输出.
 *      调整了队友和BOSS的属性
 * 
 * 1.5版本 真正的第一场战斗
 * 架构更新:
 *      重新优化面板模块,使用对象池.
 *      重新从底层优化面板模块,使用数据绑定模块
 *      重新整理游戏对象,技能,控制器之间的关系
 * 优化更新:
 *      合并场景,现在没有那么多场景了.场景抽象为Canvas
 *      添加被攻击跳数字,这个需要做成模块,待更新
 *      优化BOSS 的技能说明
 * 游戏性更新:
 *      游戏机制 BUFF 通过BUFF实现
 *      爆击 造成二倍效果
 *      取消了闪避属性.现在BOSS1的3技能默认对两个单位造成伤害
 *      技能 真言术: 盾 增加护盾,可以抵挡伤害
 *      技能 神圣之光:瞬发+hot
 *      技能 嗜血: 群体增加强度
 *      队友有了攻速和爆击机制,进行了属性调整
 *      新的BOSS 真正的一关
 * 
 * 1.6版本 队友
 * TODO:
 *      主题:队友,选择不同的队友可以有不同的游戏体验 存档功能(1.5版本已经加入测试版,1.6正式实装)
 *      BUFF 和Skill 取消递增id设定,改为唯一ID设定,然后把带ID的都创建到一起.
 *      
 * 
 * 未来更新 进化 自定义自己的角色
 * 未来更新 多目标
 * 
 * 
 * TODO: 为了一定那啥考虑,还是把StartScene变成一个面板吧. 跨场景的练习已经到位了,多场景会严重增加调试成本.
 */
namespace HealerSimulator
{
    //引擎无关,游戏核心部分,暂时不考虑场景管理
    public class GameMode
    {
        #region singlton
        private static readonly GameMode instance = new GameMode();
        public static GameMode Instance
        {
            get
            {
                return instance;
            }
        }

        private GameMode()
        {
            Clear();
        }
        #endregion

        /// <summary>
        /// 当GameMode中的数据产生变化的时候进行通知. 然后他们去处理自己的事情
        /// </summary>
        public List<Action> OnChangeEvent { get; set; } = new List<Action>();

        /// <summary>
        /// 每帧触发一次的事件
        /// </summary>
        public Action UpdateEvent;

        /// <summary>
        /// 每秒触发一次的事件
        /// </summary>
        public Action UpdatePerSecendEvent;

        /// <summary>
        /// 只有当正在战斗中控制器才会进行运作
        /// </summary>
        public bool InBattle = false;

        public float BattleTime = 0f;

        public Character Boss;

        public Character Player;

        public Character FocusCharacter;

        public List<Character> TeamCharacters;


        public void PropChanged()
        {
            TeamAlive = new List<Character>();
            TeamDead = new List<Character>();
            TeamDic = new Dictionary<TeamDuty, List<Character>>();
            TeamDic.Add(TeamDuty.Healer, new List<Character>());
            TeamDic.Add(TeamDuty.MeleeDPS, new List<Character>());
            TeamDic.Add(TeamDuty.RangeDPS, new List<Character>());
            TeamDic.Add(TeamDuty.Tank, new List<Character>());

            foreach (var v in TeamCharacters)
            {
                if (v.IsAlive)
                {
                    TeamAlive.Add(v);
                    TeamDic[v.Duty].Add(v);
                }
                else
                {
                    TeamDead.Add(v);
                }
            }

            foreach (var v in OnChangeEvent)
            {
                v.Invoke();
            }
        }
        //由PropChanged维护
        public List<Character> TeamAlive;
        public List<Character> TeamDead;
        public Dictionary<TeamDuty, List<Character>> TeamDic;






        public int DifficultyLevel;

        public string LevelName;

        public void Clear()
        {
            Skada.Instance.Clear();

            InBattle = false;
            BattleTime = 0f;
            TeamCharacters = new List<Character>();
            TeamDead = new List<Character>();
            Boss = null;
            Player = null;
            FocusCharacter = null;
            UpdateEvent = null;
            UpdatePerSecendEvent = null;

        }

        public void InitGame(int diff,int level)
        {
            LevelName = "第"+level.ToString()+"关";
            DifficultyLevel = diff;
            
            //创建小队
            TeamCharacters = new List<Character>();

            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Mage,diff));
            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Tank, diff));
            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Saber, diff));
            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Warrior, diff));
            
            //创建玩家
            Character c = PlayerController.CreatePlayer(diff);
            TeamCharacters.Add(c);

            //加满血
            foreach (var v in TeamCharacters)
            {
                v.HP = v.MaxHP;
            }

            if(level == 1)
            {
                //创建BOSS
                Boss = BossController.CreateBoss1(diff).c;
            }
            else if (level == 2)
            {
                //创建BOSS
                Boss = BossController.CreateBoss2(diff).c;
            }


            //创建游戏控制器
            new GameContrtoller();

            //初始化game设定
            Player = c;
            FocusCharacter = Player;

            PropChanged();
        }

    }
}
