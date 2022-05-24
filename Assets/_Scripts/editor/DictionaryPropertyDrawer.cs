using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ServerDictionary))]
[CustomPropertyDrawer(typeof(DownloadDictionary))]
public class DictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{
    
}
