import { BUFF, Character, Skill } from './Interface';

const base: Character = {
  hp: 2000,
  maxHP: 2000,
  mp: 2000,
  maxMP: 2000,
  speed: 1,
  crit: 0.1,
};

const tank : Character = {
  ...base,
  hp: 3000,
  maxHP: 3000,

};

const baseBuff : Partial<BUFF> = {
  still: 'not',
  type: 'positive',
  icon: 'default',
  defaultTime: -1,
  defaultHot: -1,
};

const baseSkill : Skill = {
  mpCost: 0,
  apCost: 0,
  castingInterval: -1,
  cd: -1,
};

const onBehit = (caster: Character, target: Character, skill: Skill) => {
};

const openShield : BUFF = {
  ...baseBuff,
  id: 101,
  still: 'still',
  name: '格挡',
  description: '受到的攻击伤害有{chance}的几率降低{defense},成功格挡时增加怒气{addAP}',
  events: [{
    eventName: 'Behit',
    script: 'openShield',
  }],
  customValue: {
    chance: 0.8,
    defense: 0.5,
    addAP: 10,
  },
};

const superShield : Skill = {
  ...baseSkill,
  id: 1001,
  skillName: '盾墙',
  skillDescription: '开启盾墙,冷却180s,持续15s,减伤50%',
  cd: 180,
  action: [{
    script: 'addBUFFToSelf',
  }],
  customValue: {
    buffID: 101,
  },
};

const superShieldBuff : BUFF = {
  ...baseBuff,
  id: 101,
  type: 'positive',
  name: '盾墙',
  description: '收到的伤害降低{defense},持续{defaultTime}',
  defaultTime: 15,
  events: [{
    eventName: 'Behit',
    script: 'defenseAllDamage',
  }],
  customValue: {
    defense: 0.5,
  },
};
