using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class PlugInInit : MonoBehaviour
{
    public TextMeshProUGUI Logs;
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    // Start is called before the first frame update
    void Start()
    {
        IniciarPlugIn("com.example.unityloggerbumplad.PluginInstance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IniciarPlugIn(string pluginName)
    { 
        unityClass = new AndroidJavaClass("com.example.unityloggerbumplad");
        unityActivity = unityClass.CallStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(pluginName);
        if (_pluginInstance != null) 
        {
            Debug.Log("Plugin Instance error");
        }

        _pluginInstance.CallStatic("recieveUnityActivity", unityActivity);
    }

    public void SendLog(string log) 
    {
        if (_pluginInstance != null) 
        {
            _pluginInstance.Call("writeToFile", log);
        }
    }

    public string ReadLog()
    {
        if (_pluginInstance != null)
        {
            string LoggerLogs = _pluginInstance.Call<string>("writeToFile");

            return LoggerLogs;
        }
        else { return "Error en lectura del Logger"; }
    }


    public void ShowLogs()
    {
        Logs.text = ReadLog();
    }

}
