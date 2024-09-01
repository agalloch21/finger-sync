using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;


public class PlayerManager : MonoBehaviour
{
    List<Player> playerList;


    

    void LateUpdate()
    {
        var gameobject_list = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < gameobject_list.Length; i++)
        {
            //Debug.Log($"Player {i} Pos:{gameobject_list[i].transform.position.ToString()}");
        }
       
    }
}
