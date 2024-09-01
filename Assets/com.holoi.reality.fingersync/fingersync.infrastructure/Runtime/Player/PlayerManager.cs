using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;


// https://docs-multiplayer.unity3d.com/netcode/current/basics/networkvariable/
struct PlayerInfo
{
    ulong id;
    Quaternion rotation;
    Vector3 translate;

    HandGesture leftGesture;
    Vector3 leftPosition;
    HandGesture rightGesture;
    Vector3 rightPosition;
}

public class PlayerManager : MonoBehaviour
{
    

    public void OnPlayerJoined()
    {
        // Spawn new gameobject
        
    }
}
