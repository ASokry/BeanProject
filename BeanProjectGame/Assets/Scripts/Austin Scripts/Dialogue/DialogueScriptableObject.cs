using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueScriptableObject : ScriptableObject
{
    public int npcID;
    public string npcName;
    public NPCDialogue[] NPCDialogues;
}

[System.Serializable]
public class NPCDialogue
{
    public string text;
    public Response[] responses;
}

[System.Serializable]
public class Response
{
    public int nextDialogue;
    public int buttonNumber;
    public string reply;
    public string prereq;
    public string trigger;

}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue / New Dialogue")]
public class DialogueData : DialogueScriptableObject { }