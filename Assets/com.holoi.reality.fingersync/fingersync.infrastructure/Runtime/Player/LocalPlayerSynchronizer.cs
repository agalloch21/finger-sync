using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Hands;

public class LocalPlayerSynchronizer : MonoBehaviour
{
    [SerializeField] Transform worldRoot;
    [SerializeField] XRHandJointID syncHandJoint = XRHandJointID.MiddleProximal;
    [SerializeField] Transform[] fakeHandTransform;
    [SerializeField] GameObject localPlayerPrefab;
    GameObject localPlayer = null;

    public void OnServerConnected()
    {
        localPlayer = Instantiate(localPlayerPrefab, worldRoot);
    }

    public void OnServerDisconnected()
    {
        if (localPlayer != null)
        {
            DestroyImmediate(localPlayer);
            localPlayer = null;
        }
    }
#if UNITY_EDITOR
    void Update()
    {
        if (localPlayer == null || NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
            return;

        SetHandTransform(0);
        SetHandTransform(1);
    }

    void SetHandTransform(int hand_index)
    {
        var handJointPose = fakeHandTransform[hand_index];
        var handTransform = localPlayer.transform.GetChild(hand_index);
        handTransform.localPosition = handJointPose.position;
        handTransform.localRotation = handJointPose.rotation;

        NetworkManager.Singleton.LocalClient.PlayerObject.transform.GetChild(hand_index).SetPositionAndRotation(handTransform.position, handTransform.rotation);

        //Player player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
        //PlayerHand new_hand = new PlayerHand();
        //new_hand.gesture = (HandGesture)(Random.Range(0, 3));
        //new_hand.position = handTransform.position;
        //new_hand.rotation = handTransform.rotation;

        //if (hand_index == 0)
        //    player.leftHand.Value = new_hand;
        //else
        //    player.rightHand.Value = new_hand;
    }
#endif

    public void OnUpdatedHand(Handedness handedness, Hand hand)
    {
        if (localPlayer == null || NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
            return;


        if(handedness == Handedness.Left || handedness == Handedness.Right)
        {
            int hand_index = handedness == Handedness.Left ? 0 : 1;
            var handJointPose = hand.GetHandJointPose(syncHandJoint);
            var handTransform = localPlayer.transform.GetChild(hand_index);
            handTransform.localPosition = handJointPose.position;
            handTransform.localRotation = handJointPose.rotation;

            NetworkManager.Singleton.LocalClient.PlayerObject.transform.GetChild(hand_index).SetPositionAndRotation(handTransform.position, handTransform.rotation);


            //Player player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
            //PlayerHand new_hand = new PlayerHand();
            //new_hand.gesture = (HandGesture)(Random.Range(0, 3));
            //new_hand.position = handTransform.position;
            //new_hand.rotation = handTransform.rotation;

            //if (handedness == Handedness.Left)
            //    player.leftHand.Value = new_hand;
            //else
            //    player.rightHand.Value = new_hand;
        }
    }
}
