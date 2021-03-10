/* eslint-disable no-shadow */
export interface BUFF {
  id: number
  /** 永久持续 */
  still?: 'still' | 'not'
  /** still代表不可被驱散 */
  type?: 'positive' | 'negative';
  name?: string;
  description?: string;
  icon?: string;
  defaultTime?: number;
  defaultHot?: number;
  /** 扳机列表 */
  events?: [{
    /** 触发时机 */
    eventName: string;
    /** 触发某脚本的逻辑 */
    script: string;
  }]
  /** 自定义字段,脚本从这里取值 */
  customValue?: {[key:string]: number};
  [key:string]: any;
}

export enum TeamDuty {
  Melee,
  Range
}

export interface Character {
  characterName?: string;
  description?: string;
  duty?: TeamDuty;
  speed?: number;
  crit?: number;
  isCrit?: boolean;
  defense?: number;
  hp?: number;
  isAlive?: boolean;
  maxHP?: number;
  mp?: number;
  maxMP?: number;
  castingSkill?: Skill;
  isCasting?: boolean;
  commonInterval?: number;
  commonTime?: number;
  skillList?: Skill[];
  buffs?: BUFF[];
}

export interface Skill {
  id?: number;
  skillName?: string;
  /** 技能描述 */
  skillDescription?: string;
  /** mp消耗 */
  mpCost?: number;
  /** ap消耗 */
  apCost?: number;
  cd?: number;
  /** 施法时间 */
  castingInterval?: number;

  /** 技能释放执行的动作 */
  skillType?: 'self';

  customValue?: {[key:string]: number};
  [key:string]: any;
}
