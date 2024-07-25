using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue", order = 0, fileName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string itemName = "Dialogue Sequence Title";
     //public string itemDescription = string.Empty;
    [TextArea(3, 30)] public string[] dialogueLines;
}