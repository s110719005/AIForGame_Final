using UnityEditor;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    [SerializeField] protected NPCMovement npcCMovement;
    public virtual void OnStart(Animator animator)
    {

    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnExit()
    {

    }

    public void MakeNewDecision()
    {
        npcCMovement.MakeNewDecision();
    }
}
