/*
  计划采用桥模式
  调用桥使用单桥调用
  桥 - 调用某个节点 设置某个值 为 value
  写一个基类,集成它,然后挂在UI节点上,作为 connect
  

  api - 参数
  回调使用广播 广播名 - json参数
*/

import { ReduxPuerts } from 'csharp'

console.log('event')

ReduxPuerts.Dispatch.DispatchValue('Canvas','hp','100')
