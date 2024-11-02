using System;
using System.Collections.Generic;
using StudioXRToolkit.Runtime.Scripts.Abstracts;
using Unity.Netcode;

public class NetworkSessionManager : SingletonMonoBehaviour<NetworkSessionManager>
{
    public NetworkManager networkManager;
    public Dictionary<ulong, PlayerController> players = new Dictionary<ulong, PlayerController>();

    //private const string LOCAL_IP = "127.0.0.1";
    //private const string DEVELOPMENT_IP = "136.144.164.184";
    //private const string DEVELOPMENT_PORT = "7778";
    
    public PlayerController localPlayer;

    public bool isLocalBuild;
    public enum BuildType
    {
        Server,
        Host,
        Client,
    }

    public BuildType buildType;

    protected override void Awake()
    {
        base.Awake();
        networkManager = GetComponent<NetworkManager>();
        //StartInstance(RoomManager.isHost);
    }



}
