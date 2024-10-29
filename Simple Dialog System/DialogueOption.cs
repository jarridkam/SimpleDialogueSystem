using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

[System.Serializable]
public class DialogueOption : BaseDialogueComponent
{
    [FoldoutGroup("Dialogue Settings", expanded: true)]
    [TextArea]
    public string dialogueText;

    [FoldoutGroup("Dialogue Settings", expanded: true)]
    public Response response;
}