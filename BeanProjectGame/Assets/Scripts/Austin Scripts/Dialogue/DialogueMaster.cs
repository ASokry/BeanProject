using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMaster : MonoBehaviour
{
    [Header("Scriptable Object")]
    public DialogueScriptableObject dialogue;
    public string[] triggerTexts;

    [Header ("Text Boxes")]
    public Text characterNameBox;
    public Text dialogueBox;
    public GameObject[] responseBoxes;
    public Button[] responseButtons;
    public Text[] responseTexts;

    [Header("Dialogue Logic")]
    public int currentDialogueStep;
    public int numberOfResponses;
    public bool inDialogue;
    private string chosenTrigger;
    //public List<string> activeResponses = new List<string>();
    public List<string> activeResponses;
    public List<int> activeButtonNumber;
    public List <string> invalidResponse;
    public List<int> invalidButtonNumber;
    public DialogueTriggerManager dialogueTriggerManager;
    public DialoguePrerequisiteManager dialoguePrerequisiteManager;
    //public DialogueMaterialTrigger dialogueMaterialTrigger;

    // Start is called before the first frame update

    void Start()
    {
        foreach (GameObject buttonObject in responseBoxes)
        {
            buttonObject.SetActive(false);
        }

        loadDialogue(0);
        responseButtons = new Button[responseBoxes.Length];
        for (int i = 0; i< responseBoxes.Length; i++)
        {
            responseButtons[i] = responseBoxes[i].GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inDialogue)
        {
            characterNameBox.text = dialogue.npcName;            
        }
    }

    public void buttonPressed(int buttonNumber)
    {
        for (int i = 0; i < dialogue.NPCDialogues[currentDialogueStep].responses.Length; i++) 
        {
            if (buttonNumber == dialogue.NPCDialogues[currentDialogueStep].responses[i].buttonNumber)
            {
                if(dialogue.NPCDialogues[currentDialogueStep].responses[i].trigger != null)
                {
                    chosenTrigger = dialogue.NPCDialogues[currentDialogueStep].responses[i].trigger;
                    dialogueTriggerManager.TriggerSet(chosenTrigger);
                }
                activeResponses.Clear();
                activeButtonNumber.Clear();
                invalidResponse.Clear();
                invalidButtonNumber.Clear();
                loadDialogue(dialogue.NPCDialogues[currentDialogueStep].responses[i].nextDialogue);          
            }
        }
    }

    public void loadDialogue(int nextResponse)
    {
        //activeResponses.Add("Work");
        dialogueBox.text = dialogue.NPCDialogues[nextResponse].text;
        numberOfResponses = dialogue.NPCDialogues[nextResponse].responses.Length;
        currentDialogueStep = nextResponse;
        //Prereq Test////////////////////////////////////////////////////////////////////////
        for (int i = 0; i < numberOfResponses; i++)
        {

            if (dialogue.NPCDialogues[nextResponse].responses[i].prereq == "")
            {
                activeResponses.Add(dialogue.NPCDialogues[nextResponse].responses[i].reply);
                activeButtonNumber.Add(dialogue.NPCDialogues[nextResponse].responses[i].buttonNumber);
            }
            else
            {
                for (int e = 0; e < dialoguePrerequisiteManager.dialoguePrerequisites.Length; e++)
                {

                    if (dialogue.NPCDialogues[nextResponse].responses[i].prereq == dialoguePrerequisiteManager.dialoguePrerequisites[e].triggerWord)
                    {
                        if (dialoguePrerequisiteManager.dialoguePrerequisites[e].satisfied == true)
                        {
                            activeResponses.Add(dialogue.NPCDialogues[nextResponse].responses[i].reply);
                            activeButtonNumber.Add(dialogue.NPCDialogues[nextResponse].responses[i].buttonNumber);
                        }
                        else
                        {
                            invalidResponse.Add(dialogue.NPCDialogues[nextResponse].responses[i].reply);
                            invalidButtonNumber.Add(dialogue.NPCDialogues[nextResponse].responses[i].buttonNumber);
                        }
                    }
                }
            }
            
        }

        for (int i = 0; i < responseBoxes.Length; i++)
        {
            /*for (int o = 0; o < invalidButtonNumber.Count; o++)
            {
                if(invalidButtonNumber[o] == i)
                {
                    responseBoxes[i].SetActive(true);
                    responseTexts[i].text = invalidResponse[o];
                }
            }*/
            for(int e = 0; e < activeButtonNumber.Count; e++)
            {
                if(activeButtonNumber[e] == i)
                {
                    responseBoxes[i].SetActive(true);
                    responseTexts[i].text = activeResponses[e];
                }
                else
                {
                    responseBoxes[i].SetActive(false);
                }
            }

        }

        for (int i = 0; i < activeResponses.Count; i++)
        {
            if(i == activeButtonNumber[i])
            {
                responseBoxes[i].SetActive(true);
                responseButtons[i].interactable = true;
            }
            responseTexts[i].text = activeResponses[i];
        }

        for (int i = 0; i < invalidResponse.Count; i++)
        {
            if(i == invalidButtonNumber[i])
            {
                responseBoxes[i].SetActive(true);
                responseButtons[i].interactable = false;
            }
            responseTexts[i].text = invalidResponse[i];
        }

        //Prereq Test////////////////////////////////////////////////////////////////////////


        /*for (int i = 0; i < responseBoxes.Length; i++)
        {
             if(i >= numberOfResponses)
             {
                 responseBoxes[i].SetActive(false);
             }
             if (i < numberOfResponses)
             {
                responseBoxes[i].SetActive(true);
             }

        }*/

        /*for (int i = 0; i < numberOfResponses; i++)
        {
           responseTexts[i].text = dialogue.NPCDialogues[nextResponse].responses[i].reply;
        }*/
    } 
}
