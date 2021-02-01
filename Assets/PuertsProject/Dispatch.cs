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

    public class Dispatch
    {
        public static KUIRoot Root;
        /// <summary>
        /// 改变某个节点上的某个值
        /// </summary>
        public static void DispatchValue(string nodeName, string key, string value)
        {
            Root.Find<Connect>(nodeName).SetValue(key, value);
        }
    }
}