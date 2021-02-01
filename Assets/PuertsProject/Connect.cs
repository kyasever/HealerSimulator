using UnityEngine;
using Puerts;

namespace ReduxPuerts
{
    public class Connect : MonoBehaviour
    {
        void Start()
        {

        }

        public void SetValue(string key, string value)
        {
            Log.Info($"to set key {key} with {value}");
        }
    }
}
