using System.Collections.Generic;
using UnityEngine;

public class GasStationSpot
{
    public Transform transform;
    public bool isOccupied;
}

public class GasStationManager : MonoBehaviour
{
    public static GasStationManager instance;
    [SerializeField] private List<Transform> gasStationTransforms;
    private List<GasStationSpot> gasStationSpots = new List<GasStationSpot>();
    
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var transform in gasStationTransforms)
        {
            GasStationSpot gasStationSpot = new GasStationSpot();
            gasStationSpot.transform = transform;
            gasStationSpot.isOccupied = false;
            gasStationSpots.Add(gasStationSpot);
        }
    }

    public GasStationSpot GetEmptySpot()
    {
        foreach (var gasStationSpot in gasStationSpots)
        {
            if(!gasStationSpot.isOccupied) 
            { 
                gasStationSpot.isOccupied = true;
                return gasStationSpot; 
            }
        }
        return null;
    }

    public bool HasEmptySpot()
    {
        foreach (var gasStationSpot in gasStationSpots)
        {
            if(!gasStationSpot.isOccupied) 
            { 
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
