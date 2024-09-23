using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseEffect : NetworkBehaviour
{
    //protected Dictionary<ulong, Player> opponentList;

    protected bool effectEnabled = false;

    public virtual void StartEffect()
    {

    }

    public virtual void StopEffect()
    {

    }

    public virtual void OnPairCompleted()
    {

    }

    //public virtual void SpecifyOpponent(ulong client_id, Player player)
    //{
    //    if (opponentList != null)
    //        opponentList.Add(client_id, player);
    //}

    //public virtual void ClearOpponent()
    //{
    //    if (opponentList != null)
    //        opponentList.Clear();
    //}

    //public void OnPlayerLeft(ulong client_id)
    //{
    //    if(opponentList != null && opponentList.ContainsKey(client_id))
    //    {
    //        opponentList.Remove(client_id);
    //    }
    //}
}
