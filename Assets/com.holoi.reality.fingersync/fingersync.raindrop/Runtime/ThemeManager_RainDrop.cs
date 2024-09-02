using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ThemeManager_RainDrop : MonoBehaviour
{
    public PlayerManager playerManager;
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Singleton == null || NetworkManager.Singleton.LocalClient.PlayerObject == null)
            return;

        Player player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
        float nearest_dis = float.MaxValue;
        if (player.leftHand.Value.gesture == HandGesture.Fisting)
        {
            float dis = GetNearestFistingHand(player.leftHand.Value.position);
            if (dis < nearest_dis) nearest_dis = dis;
        }
        if (player.rightHand.Value.gesture == HandGesture.Fisting)
        {
            float dis = GetNearestFistingHand(player.rightHand.Value.position);
            if (dis < nearest_dis) nearest_dis = dis;
        }


        float particle_speed = Remap(nearest_dis, 0.01f, 0.2f, 1, 0, true);
        ParticleSystem.LimitVelocityOverLifetimeModule velocity_module = particleSystem.limitVelocityOverLifetime;
        velocity_module.drag = particle_speed;

        //// Get the Velocity over lifetime modult
        //ParticleSystem.VelocityOverLifetimeModule snowVelocity = GameObject.Find("Snow").GetComponent<ParticleSystem>().velocityOverLifetime;

        ////And to modify the value
        //ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve();
        //rate.constantMax = 10.0f; // or whatever value you want
        //snowVelocity.x = rate;

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


    public void StartTheme()
    {

    }

    public void EndTheme()
    {

    }

    float GetNearestFistingHand(Vector3 pos)
    {


        List<Player> player_list = playerManager.PlayerList;

        float min_dis = float.MaxValue;
        float dis;
        for (int i=0; i< player_list.Count; i++)
        {
            if(player_list[i].GetComponent<NetworkObject>().OwnerClientId != NetworkManager.Singleton.LocalClientId)
            {
                //if(player_list[i].leftHand.Value.gesture == HandGesture.Fisting)
                {
                    dis = Vector3.Distance(player_list[i].leftHand.Value.position, pos);
                    if (dis < min_dis) min_dis = dis;
                }
                //if (player_list[i].rightHand.Value.gesture == HandGesture.Fisting)
                {
                    dis = Vector3.Distance(player_list[i].rightHand.Value.position, pos);
                    if (dis < min_dis) min_dis = dis;
                }
            }
        }
        return min_dis;
    }

}
