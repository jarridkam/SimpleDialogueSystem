using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue")]
public class Dialogue : ScriptableObject
{
    [ListDrawerSettings(Expanded = false, DraggableItems = false, ShowIndexLabels = true, ListElementLabelName = "dialogueText")]
    public List<Greeting> Greetings = new List<Greeting>();

    [ListDrawerSettings(Expanded = false, DraggableItems = false, ShowIndexLabels = true, ListElementLabelName = "dialogueText")]
    public List<Topic> Topics = new List<Topic>();

    [ListDrawerSettings(Expanded = false, DraggableItems = false, ShowIndexLabels = true, ListElementLabelName = "dialogueText")]
    public List<Goodbye> Goodbyes = new List<Goodbye>();

    public string[] GetGreetingText()
    {
        List<string> greetingsTexts = new List<string>();

        if (Greetings == null || Greetings.Count == 0)
        {
            Debug.LogWarning("Greetings list is empty or null.");
            return new string[0]; // temporary - when there is an interaction system, interaction button wont appear at all in this circumstance.
        }

        foreach (var line in Greetings[0].dialogueText)
        {
            greetingsTexts.Add(line);
            //Debug.Log(line);
        }
        return greetingsTexts.ToArray();
    }

    public List<Topic> GetTopics()
    {
        if (Topics != null && Topics.Count != 0)
        {
            return Topics;
        }

        Debug.Log("Topics is empty");
        return new List<Topic>(); 
    }

    public List<Goodbye> GetGoodbyes()
    {
        if (Goodbyes != null && Goodbyes.Count != 0)
        {
            return Goodbyes;
        }

        Debug.Log("Topics is empty");
        return new List<Goodbye>();
    }
}



