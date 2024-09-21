using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using HoloKit.ImageTrackingRelocalization;
using Xiaobo.UnityToolkit.Helper;
using System.Net.Sockets;
using System.Net;

public class DisplayInfo : MonoBehaviour
{
    public TimestampSynchronizer timestampSynchronizer;
    public FingerSyncManager fingerSyncManager;
    
    public Transform worldRoot;
    public Transform cameraOffset;
    public Transform volumeCamera;
    public Transform leftHandTF;
    public Transform rightHandTF;
    Hand leftHand;
    HandGesture leftHandGesture = HandGesture.None;

    Hand rightHand;
    HandGesture rightHandGesture = HandGesture.None;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //HelperModule.Instance.SetInfo("Local IP", GetLocalIPAddress());
        //HelperModule.Instance.SetInfo("IsRelocating", stabilizer.IsRelocalizing.ToString());
        HelperModule.Instance.SetInfo("Timestamp", timestampSynchronizer.HasSynced.ToString());
        HelperModule.Instance.SetInfo("Timestamp Progress", Mathf.Floor(timestampSynchronizer.Progress * 100).ToString());
        HelperModule.Instance.SetInfo("Timestamp Result", timestampSynchronizer.ResultTimestampOffset.ToString());
        HelperModule.Instance.SetInfo("IsFingerSyncing", fingerSyncManager.m_IsSyncing.ToString());
        HelperModule.Instance.SetInfo("Progress", Mathf.Floor(fingerSyncManager.Progress * 100).ToString());
        HelperModule.Instance.SetInfo("WorldRoot Pos", worldRoot.position.ToString());
        HelperModule.Instance.SetInfo("Camera Offset", cameraOffset.position.ToString());
        HelperModule.Instance.SetInfo("Volume Camera", volumeCamera.position.ToString());
        HelperModule.Instance.SetInfo("Left Hand TF", leftHandTF.position.ToString());
        HelperModule.Instance.SetInfo("Right Hand TF", rightHandTF.position.ToString());

        if (leftHand != null)
        {
            HelperModule.Instance.SetInfo("Left Hand Gesture", leftHandGesture.ToString());
            HelperModule.Instance.SetInfo("Left Hand Pos", leftHand.GetHandJointPose(XRHandJointID.MiddleProximal).position.ToString());
        }
        if(rightHand != null)
        {
            HelperModule.Instance.SetInfo("Right Hand Gesture", rightHandGesture.ToString());
            HelperModule.Instance.SetInfo("Right Hand Pos", rightHand.GetHandJointPose(XRHandJointID.MiddleProximal).position.ToString());
        }
    }

    public void OnHandGestureChanged(Handedness handedness, HandGesture oldGesture, HandGesture newGesture)
    {
        if(handedness == Handedness.Left)
        {
            leftHandGesture = newGesture;
        }
        else if(handedness == Handedness.Right)
        {
            rightHandGesture = newGesture;
        }
    }
    public void OnUpdatedHand(Handedness handedness, Hand hand)
    {
        if (handedness == Handedness.Left)
        {
            leftHand = hand;
        }
        else if (handedness == Handedness.Right)
        {
            rightHand = hand;
        }
    }

    string GetLocalIPAddress()
    {

        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) // && ip.ToString().Contains("192.168"))
            {
                return ip.ToString();
            }
        }
        //throw new System.Exception("No network adapters with an IPv4 address in the system!");
        return "";
    }
}
