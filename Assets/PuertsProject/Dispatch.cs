using UnityEngine;
using Puerts;

namespace ReduxPuerts
{
    /*
      按照redux的方式来做,通过比较节点的对象是否发生了更新来判断是否需要更新UI
      js调用dispatch的时候只是更新dispatch中保存的值,不会实际更新UI
      实际的更新完全由C#控制
      然后UI组件使用connect来进行连接
    */

    /// <summary>
    /// 触发一个事件
    /// action 是触发事件的名称
    /// msg 触发事件的参数,直接参数或者json对象
    /// </summary>
    /// <param name="msg"></param>
    public delegate void KEventDelegate(string action, string msg);

    public class Dispatch
    {
        // 分发的时候使用异步, 只有update的时候才会调用unity,其他时候都是js侧运行.

        // 绑定事件, js 将自己的实现绑定在这个事件上,并在js侧进行分发.
        public static event KEventDelegate KEvent;

        public static KUIRoot Root;

        /// <summary>
        /// 改变某个节点上的某个值 value是动态值
        /// </summary>
        public static void SetValue(string nodeName, string key, string value)
        {
            Connect connect = Root.Find<Connect>(nodeName);
            if (connect == null)
            {
                return;
            }
            connect.SetValue(key, value);
        }

        // AddObject 在某个节点上挂某个对象

        // 触发指定事件
        public static void Trigger(string action, string msg)
        {
            if (KEvent != null)
            {
                KEvent(action, msg);
            }
        }
    }
}