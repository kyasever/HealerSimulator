using System.Collections;
using System.Text;
using UnityEngine;

/// <summary>
/// 这个类保存着一些全局方法,方便自己使用...请不要介意
/// </summary>
public static class Utils
{
    public static string GetNString(int hp, int maxhp)
    {
        StringBuilder sb = new StringBuilder().Append(hp.ToString()).Append("/").Append(maxhp.ToString());
        return sb.ToString();
    }

    public static string GetNString(float v, float max)
    {
        StringBuilder sb = new StringBuilder().Append(v.ToString("F2")).Append("/").Append(max.ToString("F2"));
        return sb.ToString();
    }

    /// <summary>
    /// 在transform的子节点下,按照路径名字寻找相应子节点上的组件,如果没找到会报错. 
    /// 不能用来find gameobject
    /// </summary>
    public static T GetComponentByPath<T>(string Path, Transform transform)
    {
        try
        {
            T obj = transform.Find(Path).GetComponent<T>();
            return obj;
        }
        catch
        {
            Debug.LogError("_K 路径检索失败:根节点" + transform.ToString() + " 目标节点:" + Path);
            T obj = transform.Find(Path).GetComponent<T>();
            return obj;
        }
    }

    /// <summary>
    /// 使用的时候注意使用协程调用,直接调不行,渐变
    /// StartConasjf(Fade)
    /// </summary>
    public static IEnumerator Fade(CanvasGroup group, float alpha, float duration)
    { 
        float time = 0.0f;
        float originalAlpha = group.alpha;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
    }

    /// <summary>
    /// 使用的时候注意使用协程调用,直接调不行,渐变
    /// StartConasjf(Fade)
    /// </summary>
    public static IEnumerator FadeAndUp(CanvasGroup group, float alpha, float duration,float upPosition,ObjectPool sourcePool)
    {
        float time = 0.0f;
        float originalAlpha = 1f;
        while (time < duration)
        {
            time += Time.deltaTime;

            if(time > duration)
            {
                sourcePool.ReturnInstantiate(group.gameObject);
            }
            group.transform.localPosition = new Vector3(0, Mathf.Lerp(0, upPosition, time / duration),0);
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
    }

    public static bool FloatEqual(float a , float b)
    {
        return  Mathf.Abs(a-b) < 0.00001f;
    }


}