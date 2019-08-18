using HealerSimulator;
using UnityEngine;
/// <summary>
/// 这应该是面板类型的基类 可以自动绑定面板数据和触发更新
/// </summary>
public class DataBinding<T> : MonoBehaviour where T : class, IDataBinding
{
    /// <summary>
    /// 数据源 与这个UI组件绑定的数据源
    /// </summary>
    public T sourceData;

    /// <summary>
    /// 将一个数据源与这个UI组件进行绑定,如果数据源触发了变动事件,那么就会激活Refresh函数
    /// </summary>
    public virtual void Binding(T source)
    {
        if (sourceData == null)
        {
            sourceData = source;
            sourceData.OnChangeEvent.Add(Refresh);
            Refresh();
            return;
        }
        if (sourceData != source)
        {
            sourceData.OnChangeEvent.Remove(Refresh);
            sourceData = source;
            sourceData.OnChangeEvent.Add(Refresh);
            Refresh();
            return;
        }
    }

    /// <summary>
    /// 重载这个函数,检测到数据源发生了变化,刷新面板
    /// </summary>
    public virtual void Refresh()
    {

    }
}
