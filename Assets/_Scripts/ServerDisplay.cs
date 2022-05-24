using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;

public class ServerDisplay : MonoBehaviour {
    private Server server;

    [Header("UI Display")] 
    public CustomInputField serverNameInput;
    
    public void Setup(Server server) {
        this.server = server;
    }

    private void Update() {
        if (server != null) {
            serverNameInput.inputText.text = "Server Name: " + server.ReturnServerName();
        }
    }

    public void RunServer() {
        server.RunServer();
    }

    public void DeleteServer() {
        server.DeleteServer();
    }

    public void RenameServer() {
        server.RenameServer();
    }

    public void Info() {
        server.Info();
    }

    public void OpenPath() {
        server.OpenPath();
    }
}
