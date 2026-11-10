using System.Linq;
using UnityEditor;
using UnityEngine;
using LowPolyConstructorAnimals;
[CustomEditor(typeof(LPCAnimal))]
public sealed class LPCAnimalInspector:Editor {
 public override void OnInspectorGUI(){var animal=(LPCAnimal)target;serializedObject.Update();var idle=serializedObject.FindProperty("idleIndex");var coat=serializedObject.FindProperty("coatIndex");EditorGUI.BeginChangeCheck();idle.intValue=EditorGUILayout.Popup("Idle",idle.intValue,animal.idleNames);coat.intValue=EditorGUILayout.Popup("Забарвлення",coat.intValue,animal.coats.Select(x=>x.name).ToArray());if(EditorGUI.EndChangeCheck()){Undo.RecordObject(animal,"Animal selection");serializedObject.ApplyModifiedProperties();animal.SetColor(coat.intValue);if(Application.isPlaying)animal.SetIdle(idle.intValue);EditorUtility.SetDirty(animal);}EditorGUILayout.HelpBox("Idle не переміщує корінь. Анімація відтворюється у Play Mode.",MessageType.Info);DrawPropertiesExcluding(serializedObject,"m_Script","idleIndex","coatIndex");serializedObject.ApplyModifiedProperties();}
}
