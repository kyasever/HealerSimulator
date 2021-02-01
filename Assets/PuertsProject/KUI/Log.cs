using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 日志类封装 日志级别 尽可能避免大量刷屏
/// Error 严重错误,影响正常运行. 对接Debug.LogError
/// Warning 使用方式错误,但是不影响运行. 对接Debug.LogWarning
/// Info 运行信息 对接Debug.Log
/// Debug 调试信息 对接Debug.Log
/// </summary>
public static class Log
{

    /// <summary>
    /// Error 严重错误,影响正常运行. 对接Debug.LogError
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="objs"></param>
    public static void Error(string msg, params object[] objs)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[FrameError:]");
        sb.AppendFormat(msg, objs);
        UnityEngine.Debug.LogError(sb.ToString());
    }

    /// <summary>
    /// Warning 使用方式错误,但是不影响运行. 对接Debug.LogWarning
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="objs"></param>
    public static void Warning(string msg, params object[] objs)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[FrameWarning:]");
        sb.AppendFormat(msg, objs);
        UnityEngine.Debug.LogWarning(sb.ToString());
    }

    /// <summary>
    /// Info 运行信息 对接Debug.Log
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="objs"></param>
    public static void Info(string msg, params object[] objs)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[FrameInfo:]");
        sb.AppendFormat(msg, objs);
        UnityEngine.Debug.Log(sb.ToString());
    }

    /// <summary>
    /// Debug 调试信息 对接Debug.Log
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="objs"></param>
    public static void Debug(string msg, params object[] objs)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[Debug:]");
        sb.AppendFormat(msg, objs);
        UnityEngine.Debug.Log(sb.ToString());
    }

}