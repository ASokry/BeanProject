using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "New Scene List", menuName = "Maps / New Scene List")]
public class SceneListScriptableObject : ScriptableObject
{
    //public int scenesDifficulty;
    public string SceneListName;
    public ListedScene[] runScenes;
    
    [System.Serializable]
    public class ListedScene
    {
        public string sceneName;
        public GameObject sceneButton;
        public string sceneType;
        //scene difficulty int is currently redundant here
        public int sceneDifficulty;
    }

    //public class SceneListData : SceneListScriptableObject { }
}
