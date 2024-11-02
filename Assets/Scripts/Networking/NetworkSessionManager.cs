using System;
using System.Collections.Generic;
using StudioXRToolkit.Runtime.Scripts.Abstracts;
using Unity.Netcode;

public class NetworkSessionManager : SingletonMonoBehaviour<NetworkSessionManager>
{
    public NetworkManager networkManager;
    public Dictionary<ulong, PlayerController> players = new Dictionary<ulong, PlayerController>();
    public PlayerController localPlayer;

    protected override void Awake()
    {
        base.Awake();
        networkManager = GetComponent<NetworkManager>();
    }
}