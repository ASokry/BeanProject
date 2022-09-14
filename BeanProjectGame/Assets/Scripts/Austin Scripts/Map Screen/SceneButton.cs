using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public string sceneName;
    public string sceneType;
    public int sceneDifficulty;
    public bool sceneRan;
    private GameObject levelManager;
    public MapManager mapManager;
    public GameObject buttonParent;
    public GameObject sceneColumnObject;
    public SceneColumnPopulator sceneColumnPopulator;
    private SceneButton thisSceneButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        thisSceneButton = gameObject.GetComponent<SceneButton>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        mapManager = levelManager.GetComponent<MapManager>();
        buttonParent = gameObject.transform.parent.gameObject;
        sceneColumnObject = buttonParent.transform.parent.gameObject;
        sceneColumnPopulator = sceneColumnObject.GetComponent<SceneColumnPopulator>();
        sceneColumnPopulator.activeSceneButtons.Add(thisSceneButton);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0) && sceneColumnPopulator.sceneColumnActive)
        {
             Ray selectRay = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit mouseHit;

             if (Physics.Raycast(selectRay, out mouseHit))
             {
                if (mouseHit.transform == transform)
                {
                    mapManager.activeScene = gameObject;
                    sceneRan = true;
                    sceneColumnPopulator.PickNextSceneColumn(sceneRan);
                    //SceneManager.LoadScene(sceneName);
                }
             }

        }

    }
}
