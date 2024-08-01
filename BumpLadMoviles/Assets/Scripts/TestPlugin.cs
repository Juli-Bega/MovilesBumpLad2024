using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TestPlugin : MonoBehaviour
{
    const string PACK_NAME = "com.example.squarelogger";
    const string LOGGER_CLASS_NAME = "SquareManager";

    class AlertViewCallback : AndroidJavaProxy
    {
        private System.Action<int> alertHandler;

        public AlertViewCallback(System.Action<int>alertHandlerIn) : base (PACK_NAME + "." + LOGGER_CLASS_NAME + "$AlertViewCallback")
        {
            alertHandler = alertHandlerIn;
        }
        public void onButtonTapped(int index)
        {
            Debug.Log("Button tapped: " + index);
            if (alertHandler != null)
            {
                alertHandler(index);
            }
        }
    }

    static AndroidJavaClass SLoggerClass = null;
    static AndroidJavaObject SLoggerInstance = null;

    static public TestPlugin instanceSquareLoggerImpl;
    public static TestPlugin GetInstance()
    {
        return instanceSquareLoggerImpl;
    }

    public static Action<string> UpdateLogText;

    public void Start()
    {
        SendLog("Hola");
    }
    private void Awake()
    {
        if (instanceSquareLoggerImpl != null)
        {
            Destroy(gameObject);
            return;
        }
        instanceSquareLoggerImpl = this;
        DontDestroyOnLoad(gameObject);
    }
    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (SLoggerClass == null)
            {
                /*SLoggerClass = new AndroidJavaClass(PACK_NAME + "." + LOGGER_CLASS_NAME);
                AndroidJavaClass unityJava = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                //AndroidJavaObject activity = unityJava.GetStatic<AndroidJavaObject>("currentActivity");
                Debug.Log("activity: " + activity);
                SLoggerClass.CallStatic("receiveUnityActivity", activity);*/
            }
            return SLoggerClass;
        }
    }

    static public AndroidJavaObject PluginInstance
    {
        get
        {
            if (SLoggerInstance == null)
            {
                SLoggerInstance = PluginClass.CallStatic<AndroidJavaObject>("GetInstance");
            }
            return SLoggerInstance;
        }
    }

    public void SendLog(string log)
    {
        PluginInstance.Call("SendLogs", log);
    }

    public void WriteFile(string a)
    {
        PluginInstance.CallStatic("WriteFile", a);
    }

    public string ReadFile(string a)
    {
        return PluginInstance.CallStatic<string>("ReadFile", a);
    }

    public string GetLogs()
    {
        return PluginInstance.Call<string>("GetAllLogs");
    }
    public GameObject Logs;
    public void ReadLogs()
    {
        Logs = GameObject.FindGameObjectWithTag("Logger");
        Logs.GetComponent<TextMeshProUGUI>().text = GetLogs();
    }

    public void ShowAlertDialog(string[] strings, System.Action<int>handler = null)
    {
        if (strings.Length < 3)
        {
            Debug.LogError("AlertView requires at least 3 strings");
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("ShowAlertView", new object[] { strings, new AlertViewCallback(handler) });
        else
            Debug.LogWarning("AlertView not supported on this platform");
    }

    public void ShowAlert()
    {
        ShowAlertDialog(new string[] { "Atencion!", "Esta seguro que quiere borrar los registros del juego?", "No", "Si" }, (int obj) =>
        {
            if (obj == -2)
            {
                PluginInstance.CallStatic("ClearFile", " ");
                UpdateLogText?.Invoke(PluginInstance.CallStatic<string>("ReadFile", ""));
            }
            Debug.Log("Local Handler called: " + obj);
        });
    }

    private void OnDestroy()
    {
        if (instanceSquareLoggerImpl == this)
            instanceSquareLoggerImpl = null;
    }
}

