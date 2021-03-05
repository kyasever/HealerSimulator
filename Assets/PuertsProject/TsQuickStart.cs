using UnityEngine;
using Puerts;
using ReduxPuerts;
//typescript工程在TsProj，在该目录执行npm run build即可编译并把js文件拷贝进unity工程

namespace PuertsTest
{
    public class TsQuickStart : MonoBehaviour
    {
        JsEnv jsEnv;

        void Start()
        {
            Dispatch.Root = Global.Instance.Root;
            jsEnv = new JsEnv();
            jsEnv.Eval("require('src/EventManager')");
        }

        void Update()
        {
            Dispatch.Trigger("update", "");
        }
        void OnDestroy()
        {
            jsEnv.Dispose();
        }
    }
}
