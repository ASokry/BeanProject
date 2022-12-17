using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTestUI : MonoBehaviour
{
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
