/*
  计划采用桥模式
  调用桥使用单桥调用
  桥 - 调用某个节点 设置某个值 为 value
  写一个基类,集成它,然后挂在UI节点上,作为 connect

  api - 参数
  回调使用广播 广播名 - json参数
*/
Object.defineProperty(exports, "__esModule", { value: true });
exports.eventBinding = void 0;
const csharp_1 = require("csharp");

console.log("event");
// 设置native桥, 在js侧进行diff, 如果值改变了,则调用桥更新C# 对应值
csharp_1.ReduxPuerts.Dispatch.SetValue("Canvas", "hp", "100");
/** 将事件加入这个绑定中,接收来自native的事件 */
exports.eventBinding = {
  update: [],
};
let MP = 2200;
exports.eventBinding.update.push(() => {
  MP--;
  csharp_1.ReduxPuerts.Dispatch.SetValue("MPPanel", "MP", String(MP));
});
// 接收native事件
const delegate = new csharp_1.ReduxPuerts.KEventDelegate((action, msg) => {
  if (exports.eventBinding[action]) {
    exports.eventBinding[action].forEach((func) => {
      func(msg);
    });
  }
});
csharp_1.ReduxPuerts.Dispatch.add_KEvent(delegate);
// 触发事件
// ReduxPuerts.Dispatch.Trigger('action','msg')
// # sourceMappingURL=EventManager.js.map
