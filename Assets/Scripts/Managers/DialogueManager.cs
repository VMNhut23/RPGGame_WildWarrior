using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialoguePanel;   
    public TextMeshProUGUI dialogueText;           
    public Button[] choiceButtons;     

    [Header("Dialogue Content")]
    public string[] dialogueLines;      
    public string[] playerChoices;     

    private int currentDialogueIndex = 0;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentDialogueIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            DisplayChoices();
        }
    }

    private void DisplayChoices()
    {
        dialogueText.text = "Bạn muốn làm gì?";
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < playerChoices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<Text>().text = playerChoices[i];
                int choiceIndex = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => HandleChoice(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void HandleChoice(int choiceIndex)
    {
        Debug.Log("Người chơi đã chọn: " + playerChoices[choiceIndex]);
        EndDialogue();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
