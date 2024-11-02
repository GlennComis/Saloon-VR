using System;
using System.Collections.Generic;
using StudioXRToolkit.Runtime.Scripts.Abstracts;
using Unity.Netcode;

public class NetworkSessionManager : SingletonMonoBehaviour<NetworkSessionManager>
{
    public enum BuildType
    {
        Server,
        Host,
        Client,
    }
    
    public NetworkManager networkManager;
    public Dictionary<ulong, PlayerController> players = new Dictionary<ulong, PlayerController>();
    public PlayerController localPlayer;
    public BuildType buildType;

    protected override void Awake()
    {
        base.Awake();
        networkManager = GetComponent<NetworkManager>();
    }
    
    private void Start()
    {
        StartInstance();
    }

    private void StartInstance()
    {
        switch (buildType)
        {
            case BuildType.Server:
                networkManager.StartServer();
                break;
            case BuildType.Client:
                networkManager.StartClient();
                break;
            case BuildType.Host:
                networkManager.StartHost();
                break;
        }
    }
}