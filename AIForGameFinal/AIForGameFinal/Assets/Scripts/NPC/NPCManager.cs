using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField]
    private List<NPCMovement> npcs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GEPCore.Instance.SetNPCs(npcs);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var npc in npcs)
        {
            npc.OnUpdate();
        }
    }
}
