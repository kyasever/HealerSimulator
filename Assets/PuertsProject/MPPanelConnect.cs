using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReduxPuerts
{
    public class MPPanelConnect : Connect
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public string MP = "2200";
        public override void SetValue(string key, string value)
        {
            // Log.Debug($"{key} : {value}");
            if (key == "MP")
            {
                this.MP = value;
                GetComponent<MPPanel>().leftLabel.text = value;
            }
        }
    }
}