using System.Linq;
using UnityEditor;
using UnityEngine;
using LowPolyConstructorFish;
[CustomEditor(typeof(LPCFish))]
public sealed class LPCFishInspector:Editor {
 public override void OnInspectorGUI(){var animal=(LPCFish)target;serializedObject.Update();var idle=serializedObject.FindProperty("idleIndex");var coat=serializedObject.FindProperty("coatIndex");EditorGUI.BeginChangeCheck();idle.intValue=EditorGUILayout.Popup("Плавання",idle.intValue,animal.idleNames);coat.intValue=EditorGUILayout.Popup("Забарвлення",coat.intValue,animal.coats.Select(x=>x.name).ToArray());if(EditorGUI.EndChangeCheck()){Undo.RecordObject(animal,"Animal selection");serializedObject.ApplyModifiedProperties();animal.SetColor(coat.intValue);if(Application.isPlaying)animal.SetIdle(idle.intValue);EditorUtility.SetDirty(animal);}EditorGUILayout.HelpBox("Плавання відтворюється без переміщення кореня. Анімація відтворюється у Play Mode.",MessageType.Info);serializedObject.ApplyModifiedProperties();}
}
