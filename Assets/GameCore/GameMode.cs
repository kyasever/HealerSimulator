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
 * 给牧师增加了一个攻击技能 可以在低难度打输出.
 * 快速治疗治疗量提升450->600 增加5s cd 救赎祷言CD - 30s
 * 增加BOSS技能流火的攻击力450->600 牧师在10难度下增加50%闪避
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
        }
        #endregion

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

        public Action UpdateEvent;

        public Action UpdatePerSecendEvent;

        public int DifficultyLevel;

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
            DifficultyLevel = difficultyLevel;

            InBattle = true;
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
        }

    }
}
