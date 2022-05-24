using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static void SaveServerData(Server server) {
        if (!Directory.Exists(Application.persistentDataPath + "/InternalData/Server")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/InternalData/Server");
        }
        
        var bf = new BinaryFormatter();
        var stream = new FileStream(Application.persistentDataPath + "/InternalData/Server/" + server.ReturnServerName() + ".ppmsc", FileMode.Create);
        bf.Serialize(stream, server);
        stream.Close();
    }

    public static List<Server> LoadServers() {
        if (!Directory.Exists(Application.persistentDataPath + "/InternalData/Server")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/InternalData/Server");
        }
        
        var servers = new List<Server>();
        var dir = new DirectoryInfo(Application.persistentDataPath + "/InternalData/Server");
        var info = dir.GetFiles("*.ppmsc");
        info.Select(f => f.FullName).ToArray();
        foreach (var f in info) 
        {
            servers.Add(ReturnServer(Application.persistentDataPath + "/InternalData/Server/" + f.Name));
        }
        return servers;
    }

    public static Server ReturnServer(string path) {
        var bf = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Open);
        var data = bf.Deserialize(stream) as Server;
        return data;
    }

    public static void SaveServerController(ServerController serverController) {
        if (!Directory.Exists(Application.persistentDataPath + "/InternalData")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/InternalData");
        }
        
        var bf = new BinaryFormatter();
        var stream = new FileStream(Application.persistentDataPath + "/InternalData/master.ppmsc", FileMode.Create);
        bf.Serialize(stream, new ServerControllerSaver(serverController));
        stream.Close();
    }

    public static ServerControllerSaver ReturnServerControllerSaver(ServerController serverController) {
        if (File.Exists(Application.persistentDataPath + "/InternalData/master.ppmsc")) {
            var bf = new BinaryFormatter();
            var stream = new FileStream(Application.persistentDataPath + "/InternalData/master.ppmsc", FileMode.Open);
            var data = bf.Deserialize(stream) as ServerControllerSaver;
            return data;
        }
        return new ServerControllerSaver(serverController);
    }
}

[Serializable]
public class ServerControllerSaver {
    public string javaJDKPath;
    public bool hasConfirmedJDKPath;

    public ServerControllerSaver(ServerController serverController) {
        javaJDKPath = serverController.javaJDKPath;
        hasConfirmedJDKPath = serverController.hasConfirmedJDKPath;
    }
}
