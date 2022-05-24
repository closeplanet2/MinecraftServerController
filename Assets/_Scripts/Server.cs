using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Server {
    private string serverName;
    private string serverVersion;
    private string serverPath;
    
    public Server(string serverName, string serverVersion, string serverPath) {
        this.serverName = serverName;
        this.serverVersion = serverVersion;
        this.serverPath = serverPath;
    }

    public void CreateFolder() {
        if (!Directory.Exists(serverPath)) {
            Directory.CreateDirectory(serverPath);
        }
    }

    public void CreateEulaText() {
        var data =
            "#By changing the setting below to TRUE you are indicating your agreement to our EULA (https://account.mojang.com/documents/minecraft_eula).\n#Sun Apr 11 15:40:37 BST 2021\neula=true";
        File.WriteAllText(serverPath + "/eula.txt", data);
    }

    public void CreateRunBatFile(string javapath) {
        var tag = javapath.Equals("JAVA") ? "JAVA" : "\"" + javapath + "\"";
        var data = tag + " -Xmx4096M -Xms4096M -jar " + "\"" + serverPath + "/" + serverVersion + ".jar" + "\"" + "\nPAUSE";
        File.WriteAllText(serverPath + "/run.bat", data);
    }

    public string ReturnDownloadPath() {
        return serverPath + "/" + serverVersion + ".jar";
    }

    public string ReturnServerName() {
        return serverName;
    }
    
    public void RunServer() {
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = serverPath + "/run.bat";
        Process.Start(startInfo);
    }

    public void DeleteServer() {
        
    }

    public void RenameServer() {
        
    }

    public void Info() {
        
    }

    public void OpenPath() {
        Application.OpenURL(serverPath);
    }
}
