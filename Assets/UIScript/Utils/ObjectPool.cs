using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject GamePrefeb;

    public int InitCount = 5;

    public List<GameObject> ObjPoolSleep = new List<GameObject>();

    public List<GameObject> ObjPoolActive = new List<GameObject>();

    public Transform PoolNode;

    private void Start()
    {
        GameObject obj = new GameObject("PoolNode");
        obj.AddComponent<RectTransform>();
        PoolNode = obj.transform;
        PoolNode.SetParent(Global.Instance.transform);

        for (int i = 0; i < InitCount; i++)
        {
            Create();
        }
    }

    private void Create()
    {
        GameObject obj = Instantiate(GamePrefeb, PoolNode.transform);
        ObjPoolSleep.Add(obj);
        obj.SetActive(false);
    }

    /// <summary>
    /// 获取一个激活的对象
    /// </summary>
    public GameObject GetInstantiate()
    {
        if (ObjPoolSleep.Count == 0)
        {
            Create();
        }
        //取出对象
        GameObject obj = ObjPoolSleep[ObjPoolSleep.Count - 1];
        ObjPoolSleep.RemoveAt(ObjPoolSleep.Count - 1);


        obj.SetActive(true);
        obj.transform.SetParent(transform);

        ObjPoolActive.Add(obj);


        return obj;
    }

    /// <summary>
    /// 还回一个对象
    /// </summary>
    public void ReturnInstantiate()
    {
        //取出对象
        GameObject obj = ObjPoolActive[ObjPoolActive.Count - 1];
        ObjPoolActive.RemoveAt(ObjPoolActive.Count - 1);

        obj.SetActive(false);
        obj.transform.SetParent(PoolNode);

        ObjPoolSleep.Add(obj);
    }

    /// <summary>
    /// 还回一个对象
    /// </summary>
    public void ReturnInstantiate(GameObject obj)
    {
        ObjPoolActive.Remove(obj);

        obj.SetActive(false);
        obj.transform.SetParent(PoolNode);

        ObjPoolSleep.Add(obj);
    }

    /// <summary>
    /// 获取一定数量的实例对象
    /// </summary>
    /// <param name="needCount"></param>
    /// <returns></returns>
    public List<GameObject> GetInstantiate(int needCount)
    {   
        int count = ObjPoolActive.Count;
        //如果需求更多,那么进行添加
        if (needCount > count)
        {
            for (int i = 0; i < needCount - count; i++)
            {
                GetInstantiate();
            }
        }
        if (needCount < count)
        {
            for (int i = 0; i < count - needCount; i++)
            {
                ReturnInstantiate();
            }
        }
        return ObjPoolActive;
    }


}
