using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueOption : BaseDialogueComponent
{
    [TextArea]
    public string[] Response;
}