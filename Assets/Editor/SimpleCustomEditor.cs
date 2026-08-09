
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

public class SimpleCustomEditor : EditorWindow
{
    Bitpacked target;
    DropdownField dropdown;
    VisualElement labelContainer;
    Dictionary<string, List<FieldInfo>> Groups;
    

    [MenuItem("Tools/Byte Group Debugger")]
    public static void Open()
    {
    var window = GetWindow<SimpleCustomEditor>("Byte Groups");
    window.RefreshGroups();
    }
    public static void OpenToGroup(string ER)
    {
    var window = GetWindow<SimpleCustomEditor>("Byte Groups");
    window.RefreshGroups();

    if (ER != null)
        window.dropdown.value = ER;
    }

    void CreateGUI()
    {
        var refreshButton = new Button(RefreshGroups) { text = "Refresh" };
        rootVisualElement.Add(refreshButton);

        dropdown = new DropdownField("Byte Group");
        dropdown.RegisterValueChangedCallback(evt => ShowGroup(evt.newValue));
        dropdown.name = "Group Select";
        rootVisualElement.Add(dropdown);

        labelContainer = new VisualElement();
        rootVisualElement.Add(labelContainer);
    }



void RefreshGroups()
{
    var guids = AssetDatabase.FindAssets("t:Bitpacked");
    if (guids.Length == 0) { target = null; return; }
    target = AssetDatabase.LoadAssetAtPath<Bitpacked>(AssetDatabase.GUIDToAssetPath(guids[0]));
    if (target == null) return;

    var allFields = target.GetType()
        .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    var byteNameByGroup = allFields
        .Where(f => f.FieldType == typeof(byte)).Select(f => (Field: f, Attr: f.GetCustomAttribute<BitpackInt>()))
        .Where(x => x.Attr != null).ToDictionary(x => x.Attr.Group, x => x.Field.Name);

    Groups = allFields
        .Where(f => f.FieldType == typeof(bool)).Select(f => (Field: f, Attr: f.GetCustomAttribute<BitpackInt>()))
        .Where(x => x.Attr != null).GroupBy(x => x.Attr.Group)
        .ToDictionary(g => byteNameByGroup.TryGetValue(g.Key, out var name) ? name : $"Byte {g.Key}",
        g => g.Select(x => x.Field).ToList());

    dropdown.choices = Groups.Keys.ToList();
}

public void ShowGroup(string choice)
{
    labelContainer.Clear();
    if (Groups == null || string.IsNullOrEmpty(choice) || !Groups.TryGetValue(choice, out var members)) return;

    if (members.Count > 8)
        {
        var warning = new Label($"Warning {members.Count}/8 bits used — overflow");
        warning.style.color = new StyleColor(Color.red);
        labelContainer.Add(warning);
        }
    

    for (int i = 0; i < members.Count; i++)
        labelContainer.Add(new Label($"Bit {i}: {members[i].Name}"));
}

}

[InitializeOnLoad]
static class PackFailureListener
{
    static PackFailureListener()
    {
        PackEvents.OnPackFailed += group => SimpleCustomEditor.OpenToGroup(group);
    }
}
