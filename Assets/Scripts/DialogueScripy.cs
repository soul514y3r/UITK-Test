using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DialogueState
{
    Greet,
    Bye,
    Fight
}

[Serializable]
public struct DialogueEntry
{
    public DialogueState key;
    public string[] Text;
}

[CreateAssetMenu(fileName = "DialogueScripy", menuName = "Scriptable Objects/DialogueScripy")]
public class DialogueScripy : ScriptableObject
{
    public DialogueEntry[] entries;

    Dictionary<DialogueState, string[]> lookup;

    public string[] GetLines(DialogueState state)
    {
        lookup ??= entries.ToDictionary(e => e.key, e => e.Text);
        return lookup.TryGetValue(state, out var Text) ? Text : entries[0].Text;
    }
}
