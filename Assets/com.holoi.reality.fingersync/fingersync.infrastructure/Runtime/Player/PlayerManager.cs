using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;


public class PlayerManager : MonoBehaviour
{
    public List<Player> PlayerList { get => playerList; }
    List<Player> playerList = new List<Player>();





    void LateUpdate()
    {
        playerList.Clear();
        var gameobject_list = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < gameobject_list.Length; i++)
        {
            //Debug.Log($"Player {i} Pos:{gameobject_list[i].transform.position.ToString()}");
            playerList.Add(gameobject_list[i].GetComponent<Player>());
        }
       
    }
}
