using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using System.Linq;

public class DialogueDisplay : MonoBehaviour
{
    public float TextSpeed;
    public TextMeshProUGUI TextComponent;
    public Dialogue CurrentDialogue; // Temporary public
    public Transform VerticalGroup;

    private int index;
    private string[] lines;
    private List<Button> dialogOptionButtons = new List<Button>();
    private StringBuilder currentLine;

    private enum DialogueUIState
    {
        NpcSpeaking,
        PlayerChoosing,
        Responding,
        Goodbye
    }

    private DialogueUIState _dialogueUiState;
    private DialogueUIState dialogueUiState
    {
        get => _dialogueUiState;
        set
        {
            _dialogueUiState = value;
            OnStateChanged(_dialogueUiState);
        }
    }

    void Start()
    {
        dialogueUiState = DialogueUIState.NpcSpeaking;
        TextComponent.text = string.Empty;

        if (CurrentDialogue != null)
        {
            Debug.Log("Dialogue assigned.");
            lines = CurrentDialogue.GetGreetingText();

            if (lines.Length > 0)
            {
                Debug.Log(lines[0]);
                StartDialogue();
            }
            else
            {
                Debug.LogWarning("Greeting lines are empty.");
            }
        }
        else
        {
            Debug.LogWarning("CurrentDialogue is null.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueUiState == DialogueUIState.Goodbye && TextComponent.text == lines[index])
            {
                CloseDialogue();
            }
            else if (dialogueUiState == DialogueUIState.NpcSpeaking)
            {
                if (TextComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    TextComponent.text = lines[index];
                }
            }
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueUiState = DialogueUIState.PlayerChoosing;
            SetTopicButtons();
        }
    }

    private void SetTopicButtons()
    {
        ClearExistingButtons();

        var topics = CurrentDialogue.GetTopics();

        foreach (Topic topic in topics)
        {
            var topicButton = CreateDialogOptionButton();

            if (topicButton != null)
            {
                var topicButtonText = topicButton.GetComponentInChildren<TextMeshProUGUI>();
                if (topicButtonText != null)
                {
                    topicButtonText.text = topic.dialogueText;
                    Debug.Log("Option all set!");

                    topicButton.onClick.AddListener(() => HandleDialogueChoice(topic));
                    dialogOptionButtons.Add(topicButton);
                }
                else
                {
                    Debug.LogWarning("The button has a missing TextMeshProUGUI component");
                }
            }
        }
        CreateGoodbye();
    }

    private void CreateGoodbye()
    {
        Button goodbyeButton = CreateDialogOptionButton();

        var goodbyes = CurrentDialogue.GetGoodbyes();
        dialogOptionButtons.Add(goodbyeButton);

        var goodbyeButtonText = goodbyeButton.GetComponentInChildren<TextMeshProUGUI>();
        goodbyeButtonText.text = goodbyes[0].dialogueText;

        goodbyeButton.onClick.AddListener(() => HandleDialogueChoice(goodbyes[0]));
    }

    private void HandleDialogueChoice(DialogueOption dialogueOption)
    {
        if (CurrentDialogue.Goodbyes.Contains(dialogueOption))
        {
            dialogueUiState = DialogueUIState.Goodbye;
        }
        else
        {
            dialogueUiState = DialogueUIState.Responding;
        }

        ShowResponse(dialogueOption.response.responseText);
    }

    private void ShowResponse(string[] responseText)
    {
        if (responseText != null && responseText.Length > 0)
        {
            lines = responseText;
            index = 0;
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine());

            if (dialogueUiState != DialogueUIState.Goodbye)
            {
                dialogueUiState = DialogueUIState.NpcSpeaking;
            }
        }
    }

    private void OnStateChanged(DialogueUIState newState)
    {
        switch (newState)
        {
            case DialogueUIState.PlayerChoosing:
                UnlockDialogOptions();
                break;

            case DialogueUIState.NpcSpeaking:
                LockDialogOptions();
                break;
            case DialogueUIState.Responding:
                LockDialogOptions();
                break;
            case DialogueUIState.Goodbye:
                LockDialogOptions();
                break;
        }
    }

    private void LockDialogOptions()
    {
        foreach (Button btn in dialogOptionButtons)
        {
            btn.interactable = false;
        }
    }

    private void UnlockDialogOptions()
    {
        foreach (Button btn in dialogOptionButtons)
        {
            btn.interactable = true;
        }
    }

    private void ClearExistingButtons()
    {
        foreach (Button btn in dialogOptionButtons)
        {
            Destroy(btn.gameObject);
        }
        dialogOptionButtons.Clear();
    }

    private void CloseDialogue()
    {
        ClearExistingButtons();
        gameObject.SetActive(false);
        VerticalGroup.gameObject.SetActive(false);
        Debug.Log("Dialogue closed.");
    }

    private IEnumerator TypeLine()
    {
        currentLine = new StringBuilder();
        foreach (char c in lines[index].ToCharArray())
        {
            currentLine.Append(c);
            TextComponent.text = currentLine.ToString();
            yield return new WaitForSeconds(TextSpeed);
        }

        if (dialogueUiState == DialogueUIState.NpcSpeaking && index == lines.Length - 1)
        {
            dialogueUiState = DialogueUIState.PlayerChoosing;
            SetTopicButtons();
        }
    }

    private Button CreateDialogOptionButton()
    {
        const string topicPrefabFilePath = "Prefabs/UI_topic-box";
        GameObject topicPrefab = Resources.Load<GameObject>(topicPrefabFilePath);

        if (topicPrefab != null)
        {
            GameObject topicInstance = Instantiate(topicPrefab, VerticalGroup);
            topicInstance.transform.localScale = Vector3.one;

            RectTransform rectTransform = topicInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = Vector3.zero;

            Button topicButton = topicInstance.GetComponent<Button>();
            return topicButton;
        }
        else
        {
            Debug.LogWarning("Prefab not found!");
            return null;
        }
    }
}
