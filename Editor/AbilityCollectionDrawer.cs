using Base2D.Example.Abilities;
using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using UnityEditor;
using UnityEngine;

namespace Base2D.Editor
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
            
            if (collection == null)
            {
                collection = property.serializedObject.targetObject.GetType().GetField("Ability").GetValue(property.serializedObject.targetObject) as AbilityCollection;
            }

            if (unit == null)
            {
                unit = property.serializedObject.targetObject as Unit;
            }

            if (GUI.Button(new Rect(50, 50, 25, 25), "+"))
            {
                //collection.Add(new Dash(unit));
                //collection.count += 1;
                unit.Ability.Add(new Dash(unit));
                unit.a = 3;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

    }
}
