using Unity.Netcode;
using UnityEngine;

public class ShareableObject : NetworkBehaviour
{
    [SerializeField]
    Material m_DefaultMaterial;

    [SerializeField]
    Material m_SelectedMaterial;

    MeshRenderer m_MeshRenderer;

    void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void Select(bool selected)
    {
        m_MeshRenderer.material = selected ? m_SelectedMaterial : m_DefaultMaterial;

        // request ownership & update visibility
        RequestOwnership_ServerRpc();
    }

    [Rpc(SendTo.Server)]
    void RequestOwnership_ServerRpc(RpcParams rpcParams = default)
    {
        GetComponent<NetworkObject>().ChangeOwnership(rpcParams.Receive.SenderClientId);

        OnReceiveRequestResult_ClientRpc(true, RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.SpecifiedInParams)]
    void OnReceiveRequestResult_ClientRpc(bool result, RpcParams rpcParams = default)
    {
        Debug.Log($"[{ this.GetType().ToString()}] On Get Ownership. Clinet ID:{NetworkManager.Singleton.LocalClientId}");

        // notify the server broadcast to update visibility
        RequestUpdateVisibility_ServerRpc();
    }

    [Rpc(SendTo.Server)]
    void RequestUpdateVisibility_ServerRpc(RpcParams rpcParams = default)
    {
        UpdateVisibility_ClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    void UpdateVisibility_ClientRpc(RpcParams rpcParams = default)
    {
        bool visibile = !(OwnerClientId == NetworkManager.Singleton.LocalClientId);
        Debug.Log($"[{ this.GetType().ToString()}] Update Visibility. Visible: {visibile}");

        if(visibile)
        {
            // visible
        }
        else
        {
            // invisible
        }
    }
}
