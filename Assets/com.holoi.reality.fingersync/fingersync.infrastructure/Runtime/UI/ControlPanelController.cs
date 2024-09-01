using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HoloKit.ImageTrackingRelocalization;

public class ControlPanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private TMP_Text themeText;

    [SerializeField] private TMP_Text debugButtonText;

    [SerializeField] private TMP_Text logButtonText;

    [SerializeField] private Transform debugPanle;

    [SerializeField] private Transform logPanle;

    private ImageTrackingStablizer imageTrackingStablizer;

    void Start()
    {
        imageTrackingStablizer = FindFirstObjectByType<ImageTrackingStablizer>();
    }

    void Update()
    {
        if(imageTrackingStablizer != null)
        {
            if (imageTrackingStablizer.IsRelocalizing)
                progressText.text = "Relocalizing | " + Mathf.Floor(imageTrackingStablizer.Progress * 100).ToString();
            else
                progressText.text = "None";
        }
        
    }

    public void ToggleDebugPanel()
    {
        if(debugPanle.gameObject.activeSelf)
        {
            debugPanle.gameObject.SetActive(false);
            debugButtonText.text = "Show Debug";
        }
        else
        {
            debugPanle.gameObject.SetActive(true);
            debugButtonText.text = "Hide Debug";
        }
        
    }

    public void ToggleLogPanel()
    {
        if (logPanle.gameObject.activeSelf)
        {
            logPanle.gameObject.SetActive(false);
            logButtonText.text = "Show Log";
        }
        else
        {
            logPanle.gameObject.SetActive(true);
            logButtonText.text = "Hide Log";
        }
    }
}
