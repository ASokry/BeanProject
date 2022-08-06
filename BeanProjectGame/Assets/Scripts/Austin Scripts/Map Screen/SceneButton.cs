using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public string sceneName;
    public string sceneType;
    public int sceneDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
             Ray selectRay = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit mouseHit;

             if (Physics.Raycast(selectRay, out mouseHit))
             {
                if (mouseHit.transform == transform)
                {
                    SceneManager.LoadScene(sceneName);
                }
             }

        }

    }
}
