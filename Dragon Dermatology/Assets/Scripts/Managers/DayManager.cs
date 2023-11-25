using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    public List<Dragon> clients;
    public int currentClientIndex = -1;

    public List<DragonScale> scalesCollected;
    public int coinsCollected;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void AddToQueue(Dragon dragon)
    {
        // check if dragon in queue already
        clients.Add(dragon);
    }

    public bool SetCurrentClient(Dragon dragon)
    {
        if (IsClientIndexValid(currentClientIndex)) return false;

        // check if dragon is already in queue first then change index
        // temporary fix just adding the dragon passed
        clients.Add(dragon);
        currentClientIndex = clients.Count - 1;
        return true;
    }

    public bool IsClientIndexValid(int index)
    {
        return currentClientIndex >= 0 && currentClientIndex < clients.Count;
    }

    public Dragon GetCurrentClient()
    {
        if (IsClientIndexValid(currentClientIndex))
            return clients[currentClientIndex];
        else
            return null;
    }
}
