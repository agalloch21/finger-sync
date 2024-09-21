using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    List<BaseEffect> effectList = new();

    [SerializeField]
    int defaultEffectIndex = 0;

    int currentEffectIndex = -1;

    public void ChangeEffect(int index)
    {
        if(currentEffectIndex != -1)
            effectList[currentEffectIndex].StopEffect();

        if(index < effectList.Count)
            effectList[index].StartEffect();

        currentEffectIndex = index;
    }

    void Start()
    {
        ChangeEffect(defaultEffectIndex);
    }
}
