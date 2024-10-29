using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

[System.Serializable]
public class Greeting : BaseDialogueComponent
{
    [FoldoutGroup("Dialogue Settings", expanded: true)]
    public Response greetingText;
}

