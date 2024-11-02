using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class NetworkGrabbable : NetworkBehaviour
{
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;
        /// <summary>
        /// This is for the player that currently holds(owns) the object
        /// </summary>
        private NetworkObject holdingOwnerNetworkObject;
        private NetworkObject grabbableNetworkObject;

        private void Awake()
        {
            interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            grabbableNetworkObject = GetComponent<NetworkObject>();
        }
    

        private void Start()
        {
            interactable.selectEntered.AddListener(OnPickup);
            interactable.selectExited.AddListener(OnDrop);
        }
        
        public override void OnDestroy()
        {
            interactable.selectEntered.RemoveListener(OnPickup);
            interactable.selectExited.RemoveListener(OnDrop);
            
            base.OnDestroy();
        }

        private void OnPickup(SelectEnterEventArgs arg)
        {
            holdingOwnerNetworkObject = arg.interactorObject.transform.GetComponentInParent<NetworkObject>();
            ulong clientId = holdingOwnerNetworkObject.OwnerClientId;
                
            if (clientId == NetworkSessionManager.Instance.localPlayer.OwnerClientId)
            {
                ChangeHoldingOwnerIDServerRPC(clientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeHoldingOwnerIDServerRPC(ulong newClientId)
        {
            grabbableNetworkObject.ChangeOwnership(newClientId);
            //Debug.LogError("OnPickup Client id = " + newClientId);
        }
        
        private void OnDrop(SelectExitEventArgs arg)
        {
            if (!IsServer)
                return;
            
            holdingOwnerNetworkObject = null;
        }
}
