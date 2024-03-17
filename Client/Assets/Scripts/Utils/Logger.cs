using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using TMPro;
using UnityEngine;
#endif

public static class Logger
{
#if UNITY_EDITOR
    public static string DebugTextLogPath = Application.dataPath + "\\Log.txt";
#else
    public static string DebugTextLogPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt";
#endif
    private static DateTime lastRecordTime;
    public static TimeSpan savePeriod;
    public static int messagesCountMax;

#if UNITY_ENGINE
    static TextMeshProUGUI Console;
#else

#endif
    static Logger()
    {
        DebugTextLogPath = "D:\\Developing\\Client_Server_Example\\ClientLog.txt";
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        messagesCountMax = 20;
        savePeriod = new TimeSpan(0, 0, 40);
        //DebugTextLogPath = (AppDomain.CurrentDomain.BaseDirectory + "\\" + config.saveLogFileName);
        Log("log started");
        Write();

    }
    static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        Write();
    }

    private readonly static List<string> strs = new List<string>();
    public static void Log(string[] lines)
    {
        for (int n = 0; n < lines.Length; n++)
        {
            Log(lines[n]);
        }
    }
    public static void Log(string message)
    {
        strs.Add(DateTime.Now.ToString());
        strs.Add(message);
#if UNITY_ENGINE
        Debug.Log(message);
#elif DEBUG || RELEASE
#endif
        if (lastRecordTime + savePeriod > DateTime.Now)
        {
            Write();
            lastRecordTime = DateTime.Now;
        }
    }
    private static void Write()
    {
#if UNITY_ENGINE
        if (Console == null)
        {
            Console = GameObject.Find("DebugLogger")?.GetComponent<TextMeshProUGUI>();
            GameObject.DontDestroyOnLoad(Console);
        }
        if (Console != null)
        {
            if (strs.Count > messagesCountMax) strs.RemoveAt(0);
            Console.text = "";
            for (int n = 0; n < strs.Count; n++)
            {
                Console.text += Environment.NewLine + strs[n];
            }
        }
#endif
#if UNITY_ENGINE || DEBUG || RELEASE
        using (StreamWriter sw = File.AppendText(DebugTextLogPath))
        {
            //ConsoLoger.Info("Writelog:"+ DebugTextLogPath);
            sw.WriteLine(DateTime.Now);
            if (strs.Count > messagesCountMax) strs.RemoveAt(0);
            for (int n = 0; n < strs.Count; n++)
            {
                sw.WriteLine(strs[n]);
            }
        }
        /*for (int n = 0; n < strs.Count; n++)
        {
            ConsoLoger.Info(strs[n]);
        }*/
        strs.Clear();
        lastRecordTime = DateTime.Now + savePeriod;
#endif
    }
}
