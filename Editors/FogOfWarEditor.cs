
namespace Seyren.Editor {
    using UnityEditor;
    using UnityEngine;

    namespace Seyren.Editor {
        // [CustomEditor(typeof(FogOfWar))]
        // public class FogOfWarEditor : Editor {
        //     private SerializedProperty doodadPrefab;
        //     private SerializedProperty mapLayer;

        //     private void OnEnable() {
        //         doodadPrefab = serializedObject.FindProperty("doodadPrefab");
        //         mapLayer = serializedObject.FindProperty("mapLayer");
        //     }

        //     public override void OnInspectorGUI() {
        //         serializedObject.Update();

        //         EditorGUILayout.PropertyField(doodadPrefab);
        //         EditorGUILayout.PropertyField(mapLayer);

        //         if (GUILayout.Button("Place Doodad")) {
        //             PlaceDoodad();
        //         }

        //         serializedObject.ApplyModifiedProperties();
        //     }

        //     private void PlaceDoodad() {
        //         FogOfWar fogOfWar = (FogOfWar)target;

        //         // Perform the logic to place the doodad on the map
        //         // You can use the doodadPrefab and mapLayer properties to customize the placement

        //         // Example code:
        //         GameObject doodad = Instantiate(fogOfWar.doodadPrefab, Vector3.zero, Quaternion.identity);
        //         doodad.layer = fogOfWar.mapLayer;

        //         // You can add additional customization to the doodad placement here

        //         Debug.Log("Doodad placed on map!");
        //     }
        // }
    }
}