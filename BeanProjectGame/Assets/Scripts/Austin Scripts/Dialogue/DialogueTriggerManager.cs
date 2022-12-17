using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerManager : MonoBehaviour
{
    public DialoguePlotEvents[] dialoguePlotEvents;
    public DialogueMaterialEvents[] dialogueMaterialEvents;

    [System.Serializable]
    public class DialoguePlotEvents
    {
        public string triggerWord;
    }
    [System.Serializable]
    public class DialogueMaterialEvents
    {
        public string triggerWord;
        public Renderer objectRenderer;
        public Material material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerSet(string trigger)
    {
        for(int i = 0; i < dialogueMaterialEvents.Length; i++)
        {
            if(dialogueMaterialEvents[i].triggerWord == trigger)
            {
                dialogueMaterialEvents[i].objectRenderer.material = dialogueMaterialEvents[i].material;
            }
        }
    }
}
