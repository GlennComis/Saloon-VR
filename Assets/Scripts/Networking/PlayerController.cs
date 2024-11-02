using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    // Networking
    public int currentPlayerMask;
    public NetworkVariable<int> playerMaskIndex;

    [Header("Components")] [SerializeField]
    private NetworkObject networkObject;
    [SerializeField] private GameObject maskParent;

    // State
    private bool isSpectator;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // TODO: Get components when we have them (like xrOrigin, InputActionManager etc.)

        // Set the player mask index
        if (IsServer)
        {
            int maskIndex = GameManager.Instance.GetUniquePlayerMaskId();
            GameManager.Instance.AddPlayerToPlayerMasksDictionary(networkObject.OwnerClientId, maskIndex);
            SetPlayerMaskClientRPC(maskIndex, networkObject.OwnerClientId);
        }

        SyncPlayerMasks();
        SyncPlayerDataToNetworkInstanceManager();
        SetOwnerObjects(IsOwner);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        NetworkSessionManager.Instance.players.Remove(networkObject.OwnerClientId);

        if (IsServer)
        {
            GameManager.Instance.RemovePlayerToPlayerMasksDictionary(networkObject.OwnerClientId);
        }
    }

    private void SyncPlayerDataToNetworkInstanceManager()
    {
        NetworkSessionManager.Instance.players.Add(networkObject.OwnerClientId, this);

        if (IsOwner)
            NetworkSessionManager.Instance.localPlayer = this;
    }

    private void SyncPlayerMasks()
    {
        if (IsOwner)
            return;

        currentPlayerMask = playerMaskIndex.Value;
        SetMaskVisual(currentPlayerMask);
    }
    
    private void SetMaskVisual(int maskIndex)
    {
        maskParent.transform.GetChild(maskIndex).gameObject.SetActive(true);
    }

    private void SetOwnerObjects(bool isOwner)
    {
        // TODO: Implement, set owner objects only active for the owner.
    }

    #region Kanker RPCs

    // RPCs

    [ClientRpc]
    private void SetPlayerMaskClientRPC(int maskIndex, ulong playerId)
    {
        //Check if this is the targeted player
        if (playerId == networkObject.OwnerClientId)
        {
            UpdateMaskIndexServerRPC(maskIndex);
            currentPlayerMask = maskIndex;
        }

        SetMaskVisual(maskIndex);

        // Set spawnpoint for local player
        if (IsLocalPlayer)
        {
            Debug.Log("Setting spawn for local player");
            transform.position = GameManager.Instance.GetSpawnPoint(maskIndex, transform);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateMaskIndexServerRPC(int maskId)
    {
        playerMaskIndex.Value = maskId;
    }

    #endregion
}