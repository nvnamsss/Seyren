using Seyren.Example.Abilities;
using Seyren.System.Abilities;
using Seyren.System.Units;
using UnityEditor;
using UnityEngine;

namespace Seyren.Editor
{
    [CustomPropertyDrawer(typeof(AbilityCollection))]
    public class AbilityCollectionDrawer : PropertyDrawer
    {
        AbilityCollection collection;
        Unit unit;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("count"), GUIContent.none);
            
            if (property.serializedObject.hasModifiedProperties)
            {
                Debug.Log("a");
            }
            if (collection == null)
            {
                collection = property.serializedObject.targetObject.GetType().GetField("Ability").GetValue(property.serializedObject.targetObject) as AbilityCollection;
            }
            
            if (unit == null)
            {
                unit = property.serializedObject.targetObject as Unit;
            }
            //EditorGUI.PropertyField(p, property.FindPropertyRelative("count"), GUIContent.none);
            if (GUI.Button(new Rect(50, 50, 25, 25), "+"))
            {
                //collection.Add(new Dash(unit));
                //collection.count += 1;
                unit.Ability.Add(new Dash(unit));
                property.FindPropertyRelative("editorAbilities").InsertArrayElementAtIndex(0);
                property.FindPropertyRelative("editorAbilities").GetArrayElementAtIndex(0).intValue = Dash.Id;
                property.serializedObject.ApplyModifiedProperties();
                property.FindPropertyRelative("count").intValue = 7;
                //unit.GetType().GetField("a").SetValue(unit, 4);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

        }

    }
}
