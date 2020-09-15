using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(ComponentSelector))]
public class MyCustomEditor : Editor
{
    System.Type[] possible;
    string[] options;
    public override void OnInspectorGUI() {

        ComponentSelector selector = (ComponentSelector) target;
        System.Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        possible = (from System.Type type in types where type.IsSubclassOf(typeof(AbstractCamera)) select type).ToArray();
        options = new string[possible.Length];
        for (int i = 0; i < possible.Length; i++)
        {
            options[i] = possible[i].ToString();
        }
        selector.selected = EditorGUILayout.Popup("Strategy", selector.selected, options);
    }
}
