using UnityEngine;
using UnityEngine.XR.Hands;

public class ControlPanelToggler : MonoBehaviour
{
    [SerializeField] private GameObject m_ControlPanel;

    private void Start()
    {
        m_ControlPanel.SetActive(false);
    }

    public void OnHandGestureChanged(Handedness handedness, HandGesture _, HandGesture newGesture)
    {
        if (handedness == Handedness.Right)
            m_ControlPanel.SetActive(newGesture == HandGesture.FacingSelf);
    }
}
