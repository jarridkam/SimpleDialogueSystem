using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class BaseDialogueComponent
{
    [FoldoutGroup("Dialogue Settings", expanded: true)]
    [TextArea]
    public string[] dialogueText;

    [FoldoutGroup("Dialogue Settings")]
    public int priority;
}

