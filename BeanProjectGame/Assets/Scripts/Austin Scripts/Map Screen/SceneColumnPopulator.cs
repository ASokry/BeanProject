using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneColumnPopulator : MonoBehaviour
{
    public SceneListScriptableObject sceneList;
    public int[] sceneIndexes;
    public GameObject[] scenePlacements;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < sceneIndexes.Length; i++)
        {
            Instantiate(sceneList.runScenes[sceneIndexes[i]].sceneButton, scenePlacements[i].transform.position, scenePlacements[i].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
