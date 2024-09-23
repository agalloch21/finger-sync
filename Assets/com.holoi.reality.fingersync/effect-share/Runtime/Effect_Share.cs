using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Effect_Share : BaseEffect
{
    [SerializeField]
    List<GameObject> prefabList;

    [SerializeField]
    int totalCount = 40;

    [SerializeField]
    Vector3 bounds = new Vector3(5, 5, 5);


    List<GameObject> objectList = new List<GameObject>();

    float animationDuration = 2;
    float animationTime = 0;
    float animationDirection = 0;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer == false)
            return;

        // spawn objects
        SpawnPrefabs();
    }

    public override void OnPairCompleted()
    {
        base.OnPairCompleted();
    }

    void SpawnPrefabs()
    {
        if (IsServer == false)
            return;

        if (prefabList == null || prefabList.Count == 0)
            return;


        objectList.Clear();
        for (int i = 0; i < totalCount; i++)
        {
            int random_index = Mathf.FloorToInt(Mathf.Min(Random.Range(0f, prefabList.Count), prefabList.Count - 1));
            Vector3 random_pos = new Vector3(Random.Range(-1f, 1f) * bounds.x, Random.Range(-1f, 1f) * bounds.y, Random.Range(-1f, 1f) * bounds.z);
            // spawn new object
            var go = Instantiate(prefabList[random_index]);
            go.transform.position = random_pos;
            var instanceNetworkObject = go.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
            objectList.Add(go);
        }
    }

    void RedistributeOwnership()
    {
        if (IsServer == false)
            return;

        for(int i=0; i< objectList.Count; i++)
        {
            // randomly set ownership
            int random_client_index = Mathf.FloorToInt(Mathf.Min(Random.Range(0f, NetworkManager.Singleton.ConnectedClientsIds.Count), NetworkManager.Singleton.ConnectedClientsIds.Count - 1));
            ulong client_id = NetworkManager.Singleton.ConnectedClientsIds[random_client_index];

            objectList[i].GetComponent<NetworkObject>().ChangeOwnership(client_id);
        }
        
    }

    public override void StartEffect()
    {
        Debug.Log($"[{ this.GetType().ToString()}] Start.");

        // initialize effect
        RedistributeOwnership();

        // start updating effect
        effectEnabled = true;
        animationDirection = 1;
    }

    public override void StopEffect()
    {
        Debug.Log($"[{ this.GetType().ToString()}] Stop.");

        // stop updating effect
        effectEnabled = false;
        animationDirection = -1;
    }

    void UpdateAnimation()
    {
        animationTime += Time.deltaTime * animationDirection;
        if (animationTime > animationDuration)
        {
            animationTime = animationDuration;
            animationDirection = 0;
        }

        if (animationTime < 0)
        {
            animationTime = 0;
            animationDirection = 0;
        }
    }

    void Update()
    {
        UpdateAnimation();

        if (animationTime <= 0)
            return;

        if (NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
            return;


        Player myself = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
    }
}
