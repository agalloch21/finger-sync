using System.Collections.Generic;
using UnityEngine;

public class Effect_Share : BaseEffect
{
    List<GameObject> objectList;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer == false)
            return;

        // spawn objects
    }
}
