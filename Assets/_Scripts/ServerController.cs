using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerController : MonoBehaviour {
    private bool animationInProgress = false;
    
    [Header("JDK path")] 
    public string javaJDKPath = "Java";
    public bool hasConfirmedJDKPath = false;
    
    [Header("Dictionaries")]
    public ServerDictionary serverDictionary;
    public DownloadDictionary downloadDictionary;
   
    [Header("Prefabs")] 
    public GameObject serverButton;

    [Header("Transforms")] 
    public Transform serverSpawnArea;

    [Header("Input Fields")] 
    public CustomInputField serverName;
    public TextMeshProUGUI serverNamePlaceholder;
    public CustomInputField inputFieldJDKPath;
    public TextMeshProUGUI inputFieldJDKPathPlaceholder;
    public CustomDropdown versionDropdown;
    public Sprite dropdownIcon;

    [Header("MENUS")] 
    public GameObject jdkPathMenu;

    private void Start() {
        var data = SaveLoadManager.ReturnServerControllerSaver(this);
        javaJDKPath = data.javaJDKPath;
        hasConfirmedJDKPath = data.hasConfirmedJDKPath;
        
        foreach (var server in SaveLoadManager.LoadServers()) {
            serverDictionary[server.ReturnServerName()] = server;
        }
        
        versionDropdown.dropdownItems.Clear();
        foreach (var download in downloadDictionary) {
            versionDropdown.CreateNewItemFast(download.Key, dropdownIcon);
        }
        versionDropdown.SetupDropdown();
        
        var directory = Application.persistentDataPath + "/Servers/";
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        
        jdkPathMenu.SetActive(!hasConfirmedJDKPath);
        
        PopulateServerList();
    }

    public void OpenJDKPathMenu() {
        jdkPathMenu.SetActive(true);
    }

    public void SetJDKPath() {
        if (string.IsNullOrEmpty(inputFieldJDKPath.inputText.text)) {
            StartCoroutine(AnimateEmptyServerName("JDK PATH IS NULL!", "JAVA", inputFieldJDKPath.inputText.text, inputFieldJDKPath, inputFieldJDKPathPlaceholder));
            return;
        }
        jdkPathMenu.SetActive(false);
        hasConfirmedJDKPath = true;
        javaJDKPath = inputFieldJDKPath.inputText.text;
        SaveLoadManager.SaveServerController(this);
    }
    
    private void PopulateServerList() {
        for (var i = 1; i < serverSpawnArea.childCount; i++) {
            Destroy(serverSpawnArea.GetChild(i).gameObject);
        }

        foreach (var pair in serverDictionary) {
            var serverName = pair.Key;
            var server = pair.Value;
            var go = Instantiate(serverButton, serverSpawnArea);
            go.name = serverName;
            go.GetComponent<ServerDisplay>().Setup(server);
        }
    }

    public void AddNewServer() {
        if (animationInProgress) return;

        if (string.IsNullOrEmpty(this.serverName.inputText.text)) {
            StartCoroutine(AnimateEmptyServerName("SERVERNAME IS EMPTY!", "Server Name", this.serverName.inputText.text, this.serverName, serverNamePlaceholder));
            return;
        }

        if (serverDictionary.ContainsKey(this.serverName.inputText.text)) {
            StartCoroutine(AnimateEmptyServerName("SERVERNAME EXISTS!", "Server Name", this.serverName.inputText.text, this.serverName, serverNamePlaceholder));
            return;
        }

        var serverName = this.serverName.inputText.text;
        var serverVersion = versionDropdown.dropdownItems[versionDropdown.selectedItemIndex].itemName;
        var server = new Server(serverName, serverVersion, Application.persistentDataPath + "/Servers/" + serverName);
        server.CreateFolder();
        StartCoroutine(DownloadFile(ReturnDownloadURL(serverVersion), server.ReturnDownloadPath()));
        server.CreateEulaText();
        server.CreateRunBatFile(javaJDKPath);
        SaveLoadManager.SaveServerData(server);
        
        var go = Instantiate(serverButton, serverSpawnArea);
        go.name = server.ReturnServerName();
        go.GetComponent<ServerDisplay>().Setup(server);
        this.serverName.inputText.text = "";
    }
    
    private IEnumerator DownloadFile(string downloadURL, string filePath) {
        var www = new UnityWebRequest(downloadURL);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        var results = www.downloadHandler.data;
        File.WriteAllBytes(filePath, results);
    }

    private IEnumerator AnimateEmptyServerName(string message, string placeholdertext, string pastText, CustomInputField inputField, TextMeshProUGUI placeholder) {
        animationInProgress = true;
        inputField.inputText.text = message;
        yield return new WaitForSeconds(1f);
        inputField.inputText.text = pastText;
        animationInProgress = false;
    }

    private string ReturnDownloadURL(string key) {
        foreach (var data in downloadDictionary) {
            if (data.Key.Equals(key)) return data.Value;
        }
        return null;
    }
}
