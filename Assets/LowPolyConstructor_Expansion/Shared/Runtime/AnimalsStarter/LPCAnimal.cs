using System;
using UnityEngine;
namespace LowPolyConstructorAnimals {
 [DisallowMultipleComponent]
 public sealed class LPCAnimal:MonoBehaviour {
  [Serializable] public sealed class RendererMaterials { public Renderer renderer; public Material[] materials; }
  [Serializable] public sealed class Coat { public string name; public RendererMaterials[] targets; }
  public Animator animator;
  public string[] idleNames=Array.Empty<string>();
  public Coat[] coats=Array.Empty<Coat>();
  [HideInInspector] public string alternateColliderIdle;
  [HideInInspector] public Collider standingCollider, alternateCollider;
  [SerializeField] int idleIndex;
  [SerializeField] int coatIndex;
  public int IdleIndex=>idleIndex;
  public int CoatIndex=>coatIndex;
  void Awake(){ApplySelection();}
  void OnValidate(){ApplyMaterials();if(Application.isPlaying)ApplyIdle();}
  public void SetIdle(int index){idleIndex=Mathf.Clamp(index,0,Mathf.Max(0,idleNames.Length-1));ApplyIdle();}
  public void SetColor(int index){coatIndex=Mathf.Clamp(index,0,Mathf.Max(0,coats.Length-1));ApplyMaterials();}
  public void ApplySelection(){ApplyMaterials();ApplyIdle();}
  void ApplyIdle(){if(animator&&animator.runtimeAnimatorController&&idleNames.Length>0){idleIndex=Mathf.Clamp(idleIndex,0,idleNames.Length-1);animator.applyRootMotion=false;animator.Play(idleNames[idleIndex],0,0);}ApplyColliderPose();}
  void ApplyColliderPose(){if(!standingCollider||!alternateCollider)return;bool alternate=idleNames.Length>0&&idleNames[Mathf.Clamp(idleIndex,0,idleNames.Length-1)]==alternateColliderIdle;standingCollider.enabled=!alternate;alternateCollider.enabled=alternate;}
  void ApplyMaterials(){if(coats.Length==0)return;coatIndex=Mathf.Clamp(coatIndex,0,coats.Length-1);if(coats[coatIndex].targets==null)return;foreach(var target in coats[coatIndex].targets)if(target.renderer)target.renderer.sharedMaterials=target.materials;}
 }
}
