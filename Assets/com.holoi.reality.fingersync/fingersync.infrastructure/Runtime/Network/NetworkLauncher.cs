using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class NetworkLauncher : MonoBehaviour
{
    [SerializeField] string serverIP = "192.168.0.135";

    public string ServerIP { get => serverIP; set => serverIP = value; }
    public string LocalIP { get => GetLocalIPAddress(); }

    
    public void StartHost()
    {
        OnBeforeHostStarted();

        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        OnBeforeClientStarted();

        NetworkManager.Singleton.StartClient();
    }

    public void Shutdown()
    {
        NetworkManager.Singleton.Shutdown();
    }

    public void SetServerIP(string ip)
    {
        if (IsIPAddressValide(ip) == false) return;

        serverIP = ip;  
    }

    void OnBeforeHostStarted()
    {
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (unityTransport != null)
        {
            //string localIPAddress = GetLocalIPAddress();
            unityTransport.SetConnectionData("127.0.0.1", (ushort)7777);
            unityTransport.ConnectionData.ServerListenAddress = "0.0.0.0";
            //m_HostIPAddress.text = $"Host IP Address: {localIPAddress}";
        }
    }

    void OnBeforeClientStarted()
    {
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (unityTransport != null)
        {
            unityTransport.SetConnectionData(ServerIP, (ushort)7777);
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

    bool IsIPAddressValide(string ip)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$");
    }
}
