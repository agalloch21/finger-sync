using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Hands;

// https://docs-multiplayer.unity3d.com/netcode/current/basics/networkvariable/
public struct PlayerHand : INetworkSerializable, System.IEquatable<PlayerHand>
{
    public Vector3 position;
    public Quaternion rotation;
    public HandGesture gesture;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out position);
            reader.ReadValueSafe(out rotation);
            reader.ReadValueSafe(out gesture);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(position);
            writer.WriteValueSafe(rotation);
            writer.WriteValueSafe(gesture);
        }
    }

    public bool Equals(PlayerHand other)
    {
        return position == other.position && rotation == other.rotation && gesture == other.gesture;
    }
}

public class Player : NetworkBehaviour
{
    public NetworkVariable<PlayerHand> leftHand = new NetworkVariable<PlayerHand>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<PlayerHand> rightHand = new NetworkVariable<PlayerHand>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    //void Update()
    //{
    //    SetHandTransform(Handedness.Left);
    //    SetHandTransform(Handedness.Right);
    //}

    //void SetHandTransform(Handedness handedness)
    //{
    //    int hand_index = handedness == Handedness.Left ? 0 : 1;
    //    PlayerHand player_hand = handedness == Handedness.Left ? leftHand.Value : rightHand.Value;
   
    //    transform.GetChild(hand_index).SetPositionAndRotation(player_hand.position, player_hand.rotation);
    //}
}
