using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;

public class ConnectionManager : NetworkBehaviour
{
    public UnityEvent<ulong>    OnClientJoinedEvent;
    public UnityEvent<ulong>    OnClientLostEvent;

    public UnityEvent           OnServerConnectedEvent;
    public UnityEvent           OnServerLostEvent;


    public override void OnNetworkSpawn()
    {
        RegisterNetworkCallbacks();
    }

    public override void OnNetworkDespawn()
    {
        UnregisterNetworkCallbacks();
    }

    void RegisterNetworkCallbacks()
    {
        Debug.Log($"[{this.GetType().ToString()}] Add All NetCode Listener");
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        //NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        //NetworkManager.Singleton.OnClientStopped += OnClientStopped;
        //NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        //NetworkManager.Singleton.OnServerStopped += OnServerStopped;
        //NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
        //NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
    }

    void UnregisterNetworkCallbacks()
    {
        Debug.Log($"[{ this.GetType().ToString()}] Remove All NetCode Listener");
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        //NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
        //NetworkManager.Singleton.OnClientStopped -= OnClientStopped;
        //NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        //NetworkManager.Singleton.OnServerStopped -= OnServerStopped;
        //NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
        //NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
    }

    void OnClientStarted()
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnClientStarted");
    }

    void OnClientStopped(bool result)
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnClientStopped " + result);
    }

    void OnServerStarted()
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnServerStarted");
    }

    void OnServerStopped(bool result)
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnServerStopped " + result);
    }

    void OnTransportFailure()
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnTransportFailure");
    }

    void OnConnectionEvent(NetworkManager manager, ConnectionEventData data)
    {
        //Debug.Log("OnConnectionEvent | " + data.ClientId + "Remain Count " + manager.ConnectedClientsList.Count + "," + manager.ConnectedClients.Count + ", " + manager.ConnectedClientsIds.Count);
    }

    void OnClientConnectedCallback(ulong client_id)
    {
        Debug.Log(string.Format($"[{ this.GetType().ToString()}] OnClientConnectedCallback: IsHost:{IsHost}, IsClient:{IsClient}, ClientID:{client_id}"));

        if (IsHost)
        {
            OnClientJoinedEvent?.Invoke(client_id);
        }

        if (IsClient)
        {
            OnServerConnectedEvent?.Invoke();
        }
    }

    void OnClientDisconnectCallback(ulong client_id)
    {
        Debug.Log(string.Format($"[{ this.GetType().ToString()}] OnClientDisconnectCallback | IsHost:{IsHost}, IsClient:{IsClient}, ClientID:{client_id}"));
        if (IsHost)
        {
            OnClientLostEvent?.Invoke(client_id);
        }

        if (IsClient)
        {
            OnServerLostEvent?.Invoke();
        }
    }
}
