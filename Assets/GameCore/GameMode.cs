using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * 1.4 版本 进度超过Console版本 
 * 增加面板模块,开始界面和结束界面等.
 * 优化初始化流程和伤害判定流程
 * 增加多种NPC控制器,区分NPC的操作.
 * 平衡性更新:
 * 给牧师增加了一个攻击技能 可以在低难度打输出.
 * 快速治疗治疗量提升450->600 增加5s cd 救赎祷言CD - 30s
 * 增加BOSS技能流火的攻击力450->600 牧师在10难度下增加50%闪避
 * 
 * 1.5版本 第二步
 * 架构更新:
 *      重新优化面板模块,使用对象池.
 *      重新从底层优化面板模块,使用数据绑定模块
 * 优化更新:
 *      合并场景
 *      添加被攻击跳数字功能
 *      添加场景描述文字
 *      优化BOSS 技能说明
 * 游戏性更新:
 *      1.5版本主题:队友,选择不同的队友可以有不同的游戏体验 存档功能
 *      游戏机制 BUFF 通过BUFF实现
 *      游戏机制 被动 通过定制控制器实现
 *      每次更新都要加入一个全新的BOSS
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

        public List<Character> DeadCharacters = new List<Character>();


        public int DifficultyLevel;

        public string LevelName;

        public void Clear()
        {
            Skada.Instance.Clear();

            InBattle = false;
            BattleTime = 0f;
            TeamCharacters = new List<Character>();
            DeadCharacters = new List<Character>();
            Boss = null;
            Player = null;
            FocusCharacter = null;
            UpdateEvent = null;
            UpdatePerSecendEvent = null;
        }

        public void InitGame(int difficultyLevel)
        {
            LevelName = "第一关";
            DifficultyLevel = difficultyLevel;
            
            //创建小队
            TeamCharacters = new List<Character>();

            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Mage));
            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Tank));
            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Saber));
            TeamCharacters.Add(NPCController.CreateNPC(NPCController.NPCType.Warrior));
            
            //创建玩家
            Character c = PlayerController.CreatePlayer(difficultyLevel);
            TeamCharacters.Add(c);

            //加满血
            foreach (var v in TeamCharacters)
            {
                v.HP = v.MaxHP;
            }

            //创建BOSS
            Boss = BossController.CreateBoss(difficultyLevel).c;

            //创建游戏控制器
            new GameContrtoller();

            //初始化game设定
            Player = c;
            FocusCharacter = Player;

            foreach(var v in OnChangeEvent)
            {
                v.Invoke();
            }
        }

    }
}
