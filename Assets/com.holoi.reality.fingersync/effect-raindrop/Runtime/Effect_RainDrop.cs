using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Effect_RainDrop : BaseEffect
{
    
    public ParticleSystem particleSystem;
    Material particleMat;

    float animationDuration = 2;
    float animationTime = 0;
    float animationDirection = 0;

    void Start()
    {
        if(particleSystem != null)
            particleMat = particleSystem.GetComponent<ParticleSystemRenderer>().material;
    }

    public override void StartEffect()
    {
        // create or show paricle system
        particleSystem.gameObject.SetActive(true);


        // start updating effect
        effectEnabled = true;
        animationDirection = 1;
    }

    public override void StopEffect()
    {
        // stop updating effect
        effectEnabled = false;
        animationDirection = -1;
    }


    void UpdateAnimation()
    {
        animationTime += Time.deltaTime * animationDirection;
        if(animationTime > animationDuration)
        {
            animationTime = animationDuration;
            animationDirection = 0;
        }

        if(animationTime < 0)
        {
            animationTime = 0;
            animationDirection = 0;

            // destory or hide particle system
            particleSystem.gameObject.SetActive(false);
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
        float nearest_dis = float.MaxValue;
        //if (player.leftHand.Value.gesture == HandGesture.Fisting)
        {
            float dis = GetNearestFistingHand(myself.transform.GetChild(0).position);
            if (dis < nearest_dis) nearest_dis = dis;
        }
        //if (player.rightHand.Value.gesture == HandGesture.Fisting)
        {
            float dis = GetNearestFistingHand(myself.transform.GetChild(1).position);
            if (dis < nearest_dis) nearest_dis = dis;
        }


        if (particleSystem == null)
            return;


        float parameter = Remap(nearest_dis, 0.01f, 0.2f, 1, 0, true);
        ParticleSystem.LimitVelocityOverLifetimeModule velocity_module = particleSystem.limitVelocityOverLifetime;
        velocity_module.drag = parameter;

        ParticleSystem.MainModule main_module = particleSystem.main;
        main_module.simulationSpeed = 1 - parameter;

        if(particleMat != null)
        {
            particleMat.SetFloat("_Alpha", Mathf.Clamp01(animationTime / animationDuration));
        }
            

        //Debug.Log($"Nearest Dis:{nearest_dis}, Parameter:{parameter}");

        //// Get the Velocity over lifetime modult
        //ParticleSystem.VelocityOverLifetimeModule snowVelocity = GameObject.Find("Snow").GetComponent<ParticleSystem>().velocityOverLifetime;

        ////And to modify the value
        //ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve();
        //rate.constantMax = 10.0f; // or whatever value you want
        //snowVelocity.x = rate;

    }


    float GetNearestFistingHand(Vector3 pos)
    {
        float min_dis = float.MaxValue;
        float dis;
        foreach (var opponent in PlayerManager.Instance.OpponentList)
        {
            if(opponent.Key != NetworkManager.Singleton.LocalClientId)
            {
                //if(player_list[i].leftHand.Value.gesture == HandGesture.Fisting)
                {
                    dis = Vector3.Distance(opponent.Value.transform.GetChild(0).position, pos);
                    if (dis < min_dis) min_dis = dis;
                }
                //if (player_list[i].rightHand.Value.gesture == HandGesture.Fisting)
                {
                    dis = Vector3.Distance(opponent.Value.transform.GetChild(1).position, pos);
                    if (dis < min_dis) min_dis = dis;
                }
            }
        }
       
        return min_dis;
    }


    public static float Remap(float v, float src_min, float src_max, float dst_min, float dst_max, bool need_clamp = false)
    {
        if (need_clamp)
            v = Mathf.Clamp(v, src_min, src_max);

        if (src_min == src_max)
            v = 0;
        else
            v = (v - src_min) / (src_max - src_min);

        return v * (dst_max - dst_min) + dst_min;
    }

}
