using UnityEngine;
using Puerts;

namespace ReduxPuerts
{
    // 一个Connect 类,应该尽可能把绑定的值都保存为public 变量,使其在inspector中可见
    // 并重载重写setValue方法, 在方法中决定值在UI上的具体映射形式
    // 如果有按钮,那么应该重载propEvent方法, 决定按钮操作在event中的表现形式
    // Connect不一定在根节点上,任何一个节点都可以挂.

    public abstract class Connect : MonoBehaviour
    {
        void Start()
        {

        }

        public abstract void SetValue(string key, string value);
    }
}
