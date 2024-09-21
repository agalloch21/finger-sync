using UnityEngine;

public class WorldRootResetter : MonoBehaviour
{
    [SerializeField] private Transform m_RootTransform;

    [SerializeField] private Transform m_CameraOffset;

    [SerializeField] private Transform m_VolumeCamera;

    public void OnFingerSyncCompleted(Vector3 position, Quaternion rotation)
    {
        //m_RootTransform.position = position;
        Quaternion finalRotation = rotation;// * Quaternion.Euler(90f, 0f, 0f);
        //m_RootTransform.rotation = Quaternion.Euler(0f, finalRotation.eulerAngles.y, 0f);

        m_CameraOffset.position = position;
        m_CameraOffset.rotation = Quaternion.Euler(0f, finalRotation.eulerAngles.y, 0f);

        m_VolumeCamera.position = position;
        m_VolumeCamera.rotation = Quaternion.Euler(0f, finalRotation.eulerAngles.y, 0f);
    }
}
