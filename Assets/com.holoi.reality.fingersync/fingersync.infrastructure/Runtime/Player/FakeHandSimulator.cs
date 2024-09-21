using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class FakeHandSimulator : MonoBehaviour
{
    public UnityEvent<Handedness, HandGesture, HandGesture> OnHandGestureChanged_Simulation;

    float currentTime;
    float randomDuration;
    bool isFisting = false;
    Handedness currentHandedness = Handedness.Left;
#if UNITY_EDITOR
    void Start()
    {
        //StartCoroutine(TriggerFakeEvent());
        RefreshTimer();
    }

    //IEnumerator TriggerFakeEvent()
    //{
    //    yield return new WaitForSeconds(Random.Range(3f, 6f));

    //    OnHandGestureChanged_Simulation?.Invoke(Random.Range(0f, 1f) > 0.5f ? Handedness.Left : Handedness.Right, HandGesture.None, HandGesture.Fisting);
    //}

    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > randomDuration)
        {
            TriggerEvent(isFisting);
            RefreshTimer();

            isFisting = !isFisting;
        }
    }

    void TriggerEvent(bool is_fisting)
    {
        OnHandGestureChanged_Simulation?.Invoke(currentHandedness, is_fisting==true ? HandGesture.Fisting : HandGesture.None, is_fisting==false ? HandGesture.Fisting : HandGesture.None);
    }

    void RefreshTimer()
    {
        currentTime = 0;
        randomDuration = Random.Range(5f, 10f);
        currentHandedness = Random.Range(0f, 1f) > 0.5f ? Handedness.Left : Handedness.Right;
    }

    
#endif
}
