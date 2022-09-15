using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneColumnPopulator : MonoBehaviour
{
    public bool sceneColumnActive;
    public SceneListScriptableObject sceneList;
    public SceneCells[] sceneCells;
    public GameObject[] scenePlacements;
    public List<SceneButton> activeSceneButtons;

    //Class allowing linking of next scene column to assosciated scene button
    [System.Serializable]
    public class SceneCells //This custom class contains the scene index to pull from the scene list, as well as the next scene column that should be set active when that cell's button is pressed
    {
        public int sceneIndex;
        public SceneColumnPopulator nextSceneColumn;
    }
    // Start is called before the first frame update
    void Start()
    {
        //This loop instantiates the appropriate scene button into it's matching preplaced position
        for(int i = 0; i < sceneCells.Length; i++)
        {
            Instantiate(sceneList.runScenes[sceneCells[i].sceneIndex].sceneButton, scenePlacements[i].transform.position, scenePlacements[i].transform.rotation, scenePlacements[i].transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When the scene button is pressed it calls this function, it sets the scene column marked in the appropriate scene cell active while also setting the current scene cell inactive
    public void PickNextSceneColumn(bool sceneRan)
    {
        for(int i = 0; i < activeSceneButtons.Count; i++)
        {
            if (activeSceneButtons[i].sceneRan)
            {
                sceneCells[i].nextSceneColumn.sceneColumnActive = true;
                sceneColumnActive = false;
            }
        }
    }
}
