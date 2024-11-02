using Unity.Netcode;
using UnityEngine;


public class CustomNetworkedRigidbody : NetworkBehaviour
{
    private Rigidbody rb;
    private float lastSyncTime = 0f;
    private float syncInterval = 0.01f;
    
    //private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<Vector3> networkVelocity = new NetworkVariable<Vector3>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            //networkPosition.OnValueChanged += OnPositionChanged;
            networkVelocity.OnValueChanged += OnVelocityChanged;
        }
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            if (Time.time - lastSyncTime >= syncInterval)
            {
                if (/*Vector3.Distance(networkPosition.Value, rb.position) > 0.01f ||*/ 
                    Vector3.Distance(networkVelocity.Value, rb.linearVelocity) > 0.01f)
                {
                    //networkPosition.Value = rb.position;
                    UpdateServerVelocityServerRpc(rb.linearVelocity);
                    lastSyncTime = Time.time;
                }
            }
        }
        else
        {
            //rb.position = Vector3.Lerp(rb.position, networkPosition.Value, syncInterval);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, networkVelocity.Value, syncInterval / 2);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void UpdateServerVelocityServerRpc(Vector3 newVelocity)
    {
        networkVelocity.Value = newVelocity;
    }

    private void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!IsOwner)
        {
            rb.position = newPos;
        }
    }

    private void OnVelocityChanged(Vector3 oldVel, Vector3 newVel)
    {
        if (!IsOwner)
        {
            if(!rb.isKinematic)
                rb.linearVelocity = newVel;
        }
    }
}