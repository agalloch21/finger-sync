using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkPanelController : MonoBehaviour
{
    [SerializeField] private Button m_StartHostButton;

    [SerializeField] private Button m_StartClientButton;

    [SerializeField] private Button m_ShutdownButton;

    [SerializeField] private TMP_Text m_NetworkStatusText;

    [SerializeField] private Button m_SetServerIPButton;

    [SerializeField] private TMP_Text m_LocalIPText;

    [SerializeField] private TMP_Text m_ServerIPText;

    [SerializeField] private Transform m_ServerIPPanel;

    private NetworkLauncher networkLauncher;

    private void Start()
    {
        m_StartHostButton.gameObject.SetActive(true);
        m_StartClientButton.gameObject.SetActive(true);
        m_ShutdownButton.gameObject.SetActive(false);

        networkLauncher = FindFirstObjectByType<NetworkLauncher>();
    }

    private void Update()
    {
        if (!NetworkManager.Singleton.IsConnectedClient)
        {
            m_NetworkStatusText.text = "Status: Not Connected";

            if (!m_StartHostButton.gameObject.activeSelf && !m_StartClientButton.gameObject.activeSelf && m_ShutdownButton.gameObject.activeSelf)
            {
                m_StartHostButton.gameObject.SetActive(true);
                m_StartClientButton.gameObject.SetActive(true);
                m_ShutdownButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (NetworkManager.Singleton.IsHost)
            {
                m_NetworkStatusText.text = $"Status: Host({NetworkManager.Singleton.ConnectedClients.Count})";
            }
            else
            {
                m_NetworkStatusText.text = $"Status: Joined({NetworkManager.Singleton.LocalClientId})";
            }

            if (m_StartHostButton.gameObject.activeSelf && m_StartClientButton.gameObject.activeSelf && !m_ShutdownButton.gameObject.activeSelf)
            {
                m_StartHostButton.gameObject.SetActive(false);
                m_StartClientButton.gameObject.SetActive(false);
                m_ShutdownButton.gameObject.SetActive(true);
            }
        }

        m_LocalIPText.text = networkLauncher.LocalIP;
        m_ServerIPText.text = networkLauncher.ServerIP;
    }

    public void OnOpenServerIPPanel()
    {
        m_ServerIPPanel.gameObject.SetActive(true);
    }

    public void OnCloseServerIPPanel()
    {
        m_ServerIPPanel.gameObject.SetActive(false);
    }

    public void OnConfirmServerIP()
    {
        if (m_ServerIPText == null || networkLauncher == null) return;

        networkLauncher.SetServerIP(m_ServerIPText.text);
    }
}
