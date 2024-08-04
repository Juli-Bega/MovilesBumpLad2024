package com.example.unityloggerbumplad;

import android.app.Activity;
import android.os.Environment;
import android.widget.Toast;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.FileWriter;

public class PluginInstance {

    private static Activity unityActivity;

    public static void receiveUnityActivity (Activity tActivity)
    {
        unityActivity = tActivity;
    }

    public void ScreenMSG(String msg)
    {
        Toast.makeText(unityActivity, msg, Toast.LENGTH_SHORT).show();
    }

    public void writeToFile(String newLog) {

        String fileDirectory = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOCUMENTS).getPath();
        String aux = newLog;
        try{
            FileWriter writer = new FileWriter(fileDirectory + "/Logger.txt");
            writer.write(newLog);
            writer.close();
        }
        catch(IOException e)
        {
            e.printStackTrace();
        }

    }
    public String readFile()
    {
        FileReader fr = null;
        String fileDirectory = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOCUMENTS).getPath();
        File logs = new File(fileDirectory + "/Logger.txt") ;
        try{
        fr = new FileReader(logs);
        return fileDirectory;
        //return fr.toString();
        } catch (FileNotFoundException e)
        {
            e.printStackTrace();
            return "Error to load File";
        }
    }
    public void deleteFile() {
        String fileDirectory = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOCUMENTS).getPath();
        File logs = new File(fileDirectory, "Logger.txt");
        logs.delete();
    }

}
