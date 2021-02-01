using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class KUIRoot : MonoBehaviour
{
    /// <summary>
    /// 根据子节点名称存储所有节点
    /// </summary>
    protected Dictionary<string, GameObject> nodes;

    /// <summary>
    /// 通过字符串获取GameObject 加入异常处理
    /// </summary>
    public GameObject Find(string name)
    {
        if (nodes.ContainsKey(name))
        {
            return nodes[name];
        }
        else
        {
            Log.Error("当前对象{0} 未找到节点:{1}", gameObject.name, name);
            return null;
        }
    }

    public T Find<T>(string name) where T : Component
    {
        if (nodes.ContainsKey(name))
        {
            T com = nodes[name].GetComponent<T>();
            return com;
        }
        else
        {
            Log.Error("当前对象{0} 未找到节点:{1}", gameObject.name, name);
            return null;
        }
    }

    public void SetText(string name, string value)
    {
        if (nodes.ContainsKey(name))
        {
            Text text = nodes[name].GetComponent<Text>();
            if (text != null)
            {
                text.text = value;
            }
        }
    }

    void Awake()
    {
        //递归地存储节点
        nodes = new Dictionary<string, GameObject>();
        nodes.Add(gameObject.name, gameObject);
        RecursiveNode(transform);
    }

    /// <summary>
    /// 递归地获得所有子节点
    /// </summary>
    /// <param name="trans">Trans.</param>
    protected void RecursiveNode(Transform trans)
    {
        AddNode(trans, "");
    }


    public RectTransform FindRecTrans(string name)
    {
        return Find(name)?.transform as RectTransform;
    }

    private void AddNode(Transform trans, string cur)
    {
        foreach (Transform child in trans)
        {
            string name;
            if (cur != "")
            {
                name = cur + "." + child.name;
            }
            else
            {
                name = child.name;
            }
            //避免重名添加
            if (nodes.ContainsKey(name))
            {
                Log.Warning("当前对象{0} 重复添加了节点:{1}", gameObject.name, name);
            }
            else
                nodes.Add(name, child.gameObject);
            if (child.childCount != 0)
            {
                AddNode(child, name);
            }
        }
    }


}
