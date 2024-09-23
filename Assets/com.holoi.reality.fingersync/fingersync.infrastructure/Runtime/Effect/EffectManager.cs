using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;

public enum EffectPairMode
{
    Single = 0,
    All
}

public class EffectManager : NetworkBehaviour
{
    [SerializeField]
    List<BaseEffect> effectList = new();

    [SerializeField]
    int defaultEffectIndex = 1;

    int currentEffectIndex = -1;

    EffectPairMode currentPairMode = EffectPairMode.Single;
    EffectPairMode previousPairMode = EffectPairMode.Single;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            // initialize effect
            SetEffectStatusToAll(EffectPairMode.All, defaultEffectIndex);

            return;
        }            

        if(IsClient)
        {
            // Query effect mode and effect index
            // if effect mode is All, align with others
            // if effect mode is Single, enter default effect
            QueryEffectStatus_ServerRpc();

            return;
        } 
    }

    // Server
    public void SetEffectStatusToAll(EffectPairMode pair_mode, int effect_index)
    {
        if (IsServer == false)
            return;

        SetEffectStatusToAll_ClientRpc(pair_mode, effect_index);
    }

    // Client
    public void OnPairCompleted()
    {
        // request performing default effect
        if (currentPairMode == EffectPairMode.Single)
        {
            RequestEnteringEffect(0);
        }
    }

    public void RequestEnteringEffect(int effect_index)
    {
        // effect can only be changed by server if pair mode is All
        if (currentPairMode == EffectPairMode.All)
            return;

        if (PlayerManager.Instance != null && PlayerManager.Instance.OpponentList != null)
        {
            ulong opponent_id = ulong.MaxValue;

            // didn't find a way to pass list<ulong> to Rpc function.
            // assume we only have single opponent for now.
            if (PlayerManager.Instance.OpponentList.Count > 0)
                opponent_id = PlayerManager.Instance.OpponentList.Keys.First();

            RequestEnteringEffect_ServerRpc(effect_index, opponent_id);
        }
    }



    #region Query Effect
    [Rpc(SendTo.Server)]
    void QueryEffectStatus_ServerRpc(RpcParams rpcParams = default)
    {
        OnReceiveQueryResult_ClientRpc(currentPairMode, currentEffectIndex, RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
    }
    [Rpc(SendTo.SpecifiedInParams)]
    void OnReceiveQueryResult_ClientRpc(EffectPairMode pair_mode, int effect_index, RpcParams rpcParams = default)
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnReceiveQueryResult_ClientRpc. PairMode:{pair_mode}, EffectIndex:{effect_index}");

        SetPairMode(pair_mode);

        if(pair_mode == EffectPairMode.All)
        {
            SetEffect(effect_index);
        }
        else if(pair_mode == EffectPairMode.Single)
        {
            SetEffect(defaultEffectIndex);
        }
        
    }
    #endregion


    #region Request entering specific effect
    [Rpc(SendTo.Server)]
    void RequestEnteringEffect_ServerRpc(int effect_index, ulong opponent_id, RpcParams rpcParams = default)
    {
        if(currentPairMode == EffectPairMode.Single)
        {
            List<ulong> client_list = new List<ulong>();
            client_list.Add(rpcParams.Receive.SenderClientId);
            if(opponent_id != ulong.MaxValue)
                client_list.Add(opponent_id);

            // Send to raiser and his opponent
            OnReceiveRequestResult_ClientRpc(effect_index, rpcParams.Receive.SenderClientId, RpcTarget.Group(client_list, RpcTargetUse.Temp));
        }
    }

    [Rpc(SendTo.SpecifiedInParams)]
    void OnReceiveRequestResult_ClientRpc(int effect_index, ulong raiser_id, RpcParams rpcParams = default)
    {
        Debug.Log($"[{ this.GetType().ToString()}] OnReceiveRequestResult_ClientRpc. PairMode:{currentPairMode}, EffectIndex:{effect_index}, RaiserId:{raiser_id}");

        if (currentPairMode == EffectPairMode.Single)
        {
            // check if raiser is self or component
            if (raiser_id == NetworkManager.Singleton.LocalClientId
                || (PlayerManager.Instance != null && PlayerManager.Instance.OpponentList != null && PlayerManager.Instance.OpponentList.Keys.First() == raiser_id))
            {
                SetEffect(effect_index);
            }
        }
    }

    #endregion


    #region Broadcast & Set Effect
    [Rpc(SendTo.Everyone)]
    void SetEffectStatusToAll_ClientRpc(EffectPairMode pair_mode, int effect_index)
    {
        Debug.Log($"[{ this.GetType().ToString()}] SetEffectStatusToAll_ClientRpc.");

        SetPairMode(pair_mode);

        SetEffect(effect_index);
    }
    #endregion


    void SetPairMode(EffectPairMode pair_mode)
    {
        Debug.Log($"[{ this.GetType().ToString()}] SetPairMode: {pair_mode}, CurrentMode:{currentPairMode}");

        if (currentPairMode == pair_mode)
            return;        

        previousPairMode = currentPairMode;
        currentPairMode = pair_mode;

        // set pairmode for current effect


        // reset opponent list
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.RefreshOpponetListByPairMode(pair_mode);
        }
    }

    void SetEffect(int effect_index)
    {
        Debug.Log($"[{ this.GetType().ToString()}] SetEffect: {effect_index}, CurrentEffect:{currentEffectIndex}");

        if (currentEffectIndex != -1)
            effectList[currentEffectIndex].StopEffect();

        if (effect_index < effectList.Count)
            effectList[effect_index].StartEffect();

        currentEffectIndex = effect_index;
    }
}
