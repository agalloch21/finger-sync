using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;


public class PlayerManager : MonoBehaviour
{
    public Dictionary<ulong, Player> PlayerList { get => playerList; }
    protected Dictionary<ulong, Player> playerList = new();

    public Dictionary<ulong, Player> OpponentList { get => opponentList; }
    protected Dictionary<ulong, Player> opponentList = new();

    ulong pairedPlayerId = ulong.MaxValue;
    Player pairedPlayer = null;

    EffectPairMode effectPairMode = EffectPairMode.Single;

    public UnityEvent<ulong> OnPlayerJoined;
    public UnityEvent<ulong> OnPlayerLeft;

    public UnityEvent<ulong> OnPairCompleted;
    ////////////////////////////////////////////////
    ///
    /// Todo: Determine when pairing complete
    ///
    ////////////////////////////////////////////////

    public void RefreshOpponetListByPairMode(EffectPairMode pair_mode)
    {
        effectPairMode = pair_mode;
        UpdateOpponentList();
    }

    void UpdateOpponentList()
    {
        if (effectPairMode == EffectPairMode.All)
        {
            opponentList.Clear();
            foreach (var player in playerList)
            {
                if (player.Key != NetworkManager.Singleton.LocalClientId)
                    opponentList.Add(player.Key, player.Value);
            }
        }
        else if (effectPairMode == EffectPairMode.Single)
        {
            opponentList.Clear();

            if (pairedPlayerId != ulong.MaxValue && pairedPlayer != null && playerList.ContainsKey(pairedPlayerId))
            {
                opponentList.Add(pairedPlayerId, pairedPlayer);
            }
        }

        Debug.Log($"[{ this.GetType().ToString()}] UpdateOpponentList. Count: {opponentList.Count}");
    }

    void Update()
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        if (NetworkManager.Singleton == null)
            return;

        var gameobject_list = GameObject.FindGameObjectsWithTag("Player");

        bool need_update_opponent_list = false;
        // check if player left
        List<ulong> player_to_be_removed = new();
        foreach(var player in playerList)
        {
            ulong client_id = player.Key;

            bool exist = false;
            for(int i=0; i<gameobject_list.Length; i++)
            {
                if(gameobject_list[i].GetComponent<NetworkObject>().OwnerClientId == client_id)
                {
                    exist = true;
                    break;
                }
            }

            if(exist == false)
            {
                player_to_be_removed.Add(client_id);
                need_update_opponent_list = true;
            }
        }

        for(int i=0; i<player_to_be_removed.Count; i++)
        {
            ulong client_id = player_to_be_removed[i];

            playerList.Remove(client_id);

            OnPlayerLeft?.Invoke(client_id);

            Debug.Log($"[{ this.GetType().ToString()}] Player {client_id} Left. Player Count:{playerList.Count}");
        }

        // check if new player joined
        for(int i=0; i< gameobject_list.Length; i++)
        {
            ulong client_id = gameobject_list[i].GetComponent<NetworkObject>().OwnerClientId;
            if(playerList.ContainsKey(client_id) == false)
            {
                playerList.Add(client_id, gameobject_list[i].GetComponent<Player>());

                OnPlayerJoined?.Invoke(client_id);

                need_update_opponent_list = true;

                Debug.Log($"[{ this.GetType().ToString()}] Player {client_id} Joined. Player Count:{playerList.Count}");
            }
        }


        // update opponent list
        if(need_update_opponent_list)
        {
            UpdateOpponentList();
        }

    }

    //void UpdatePlayerList()
    //{
    //    if (NetworkManager.Singleton == null)
    //        return;

    //    var client_list = NetworkManager.Singleton.ConnectedClients;
    //    var gameobject_list = GameObject.FindGameObjectsWithTag("Player");


    //    // check if player left by finding id in connected list.
    //    // at that time, the gameobject may has not been detoried
    //    for(int i=playerList.Count - 1; i>= 0; i--)
    //    {
    //        ulong client_id = playerList[i].GetComponent<NetworkObject>().OwnerClientId;
    //        if(client_list.ContainsKey(client_id) == false)
    //        {
    //            playerList.Remove(client_id);

    //            OnPlayerLeft?.Invoke(client_id);

    //            Debug.Log($"[{ this.GetType().ToString()}] Player {client_id} Left. Player Count:{playerList.Count}");
    //        }
    //    }

    //    // check if new player joined by checking the existance of the gameobject
    //    // at that time, the id may has been added to the connected list for a while
    //    for (int i = 0; i < gameobject_list.Length; i++)
    //    {
    //        ulong client_id = gameobject_list[i].GetComponent<NetworkObject>().OwnerClientId;
    //        if(playerList.ContainsKey(client_id) == false && client_list.ContainsKey(client_id) == true)
    //        {
    //            playerList.Add(client_id, gameobject_list[i].GetComponent<Player>());

    //            OnPlayerJoined?.Invoke(client_id);

    //            Debug.Log($"[{ this.GetType().ToString()}] Player {client_id} Joined. Player Count:{playerList.Count}");
    //        }
    //    }

    //}

    public void OnClientJoinedEvent(ulong client_id)
    {
        // do nothing, wait until the gameobject has been inistiated 
    }

    public void OnClientLostEvent(ulong client_id)
    {
        // remove Player immediately
        //playerList.Remove(client_id);

        //OnPlayerLeft?.Invoke(client_id);

        //UpdatePlayerList();
    }

    protected static PlayerManager _Instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindAnyObjectByType<PlayerManager>();
                if (_Instance == null)
                {
                    GameObject go = new GameObject();
                    _Instance = go.AddComponent<PlayerManager>();
                }
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        _Instance = null;
    }
}
