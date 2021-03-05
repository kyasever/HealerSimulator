// 思考一下到底要什么,要回合制还是即时
// 要什么字段 不要什么字段
/**
 * 低成长性,不做rpg. 成长就是成长一些技能,不要成长属性.
 * 给队友挂hot 给敌人挂dot
 *
 * 千面影
 * 应该是先有玩法后有技能
 * 队伍人数应该在5人锁定
 * boss加小怪也控制在5人以内
 * 5人应该至少有一个坦克. 区分近战位和远程位
 *
 * npc会有风格 更容易受到伤害或者更倾向于保命
 *
 * 黑暗愈合 对队友 造成伤害, 并添加持续恢复的hot
 * 凋零虹吸 敌人, 持续造成伤害,并恢复自己的生命
 * 狂热 增加一个队友的急速, 但是会持续损失生命
 * 黑暗球  3层充能 造成治疗或者伤害
 * 涌泉
 *
 * 比如一个斩杀战士
 * 攻击 +10怒
 * 重击 消耗20怒
 * 斩杀 35怒 大量伤害, 20%以下使用
 *
 * 法师
 * 能量射线 大量耗蓝
 */

export interface Character {
   hp: number
   hpMax: number
   mp: number
   mpMax: number
   speed : number
   crit : number
 }