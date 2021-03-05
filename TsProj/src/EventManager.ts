/*
  计划采用桥模式
  调用桥使用单桥调用
  桥 - 调用某个节点 设置某个值 为 value
  写一个基类,集成它,然后挂在UI节点上,作为 connect

  api - 参数
  回调使用广播 广播名 - json参数

  Puerts使用尝试
    js 调用 所有 C# 侧函数 均通过Dispatch类进行。
    Dispatch.SetValue 设置 某个节点， 某个字段， 为 某个值
        节点名通过KUIRoot进行管理，确保场景里的Object均可以在KUIRoot找到
        某个字段到某个值的映射，通过Connect管理，在节点上挂载Connect组件。 所有KUIRoot中存在的节点，均应该被挂载了Connect组件。
        Connect组件的kv字段应该public暴露，便于调试。
        Prefab上均应该挂载Connect组件，添加和删除都有对象池。并在KUIRoot上有操作。
        如果是C# 触发的事件，那么就通过全局事件通知js，js接收事件执行对应逻辑。
        一是为了解耦，二是为了隐藏，给它加一个node的前端。
*/

import { ReduxPuerts } from 'csharp';

console.log('event');

// 设置native桥, 在js侧进行diff, 如果值改变了,则调用桥更新C# 对应值
ReduxPuerts.Dispatch.SetValue('Canvas', 'hp', '100');

/** 将事件加入这个绑定中,接收来自native的事件 */
export const eventBinding : {[key:string]: Array<(msg: string)=>void>} = {
  update: [],
};

let MP = 2200;
eventBinding.update.push(() => {
  MP--;
  ReduxPuerts.Dispatch.SetValue('MPPanel', 'MP', String(MP));
});

// 接收native事件
const delegate = new ReduxPuerts.KEventDelegate((action, msg) => {
  if (eventBinding[action]) {
    eventBinding[action].forEach((func) => {
      func(msg);
    });
  }
});
ReduxPuerts.Dispatch.add_KEvent(delegate);

// 触发事件
// ReduxPuerts.Dispatch.Trigger('action','msg')
