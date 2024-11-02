using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkSingletonMonoBehaviour<GameManager>
{
    // Static
    public static int MAX_PLAYERS = 6;
    
    // State
    private Dictionary<ulong, int> currentPlayerMasks = new Dictionary<ulong, int>();

    [Header("Configuration")] [SerializeField]
    private List<GameObject> spawnPositions;
    
    public void AddPlayerToPlayerMasksDictionary(ulong playerId, int maskIndex)
    {
        currentPlayerMasks.Add(playerId, maskIndex);
    }
    
    public void RemovePlayerToPlayerMasksDictionary(ulong playerId)
    {
        currentPlayerMasks.Remove(playerId);
    }  
    
    public int GetUniquePlayerMaskId()
    {
        if (!currentPlayerMasks.ContainsValue(0))
        {
            print("Returning 0 (Player One)");
            return 0;
        } 
        
        if (!currentPlayerMasks.ContainsValue(1))
        {
            print("Returning 1 (Player Two)");
            return 1;
        }  
        
        if (!currentPlayerMasks.ContainsValue(2))
        {
            print("Returning 2 (Player Three)");
            return 2;
        }
         
        if (!currentPlayerMasks.ContainsValue(3))
        {
            print("Returning 3 (Player Four)");
            return 3;
        }
         
        if (!currentPlayerMasks.ContainsValue(4))
        {
            print("Returning 4 (Player Five)");
            return 4;
        }
        
        if (!currentPlayerMasks.ContainsValue(5))
        {
            print("Returning Five (Player Six)");
            return 5;
        }
        
        Debug.LogError("All colors are taken? returned Spectator (99)");
        return 99;
    }

    public Vector3 GetSpawnPoint(int index, Transform playerTransform)
    {
        var spawnPointPosition = Vector3.zero;
        spawnPointPosition.y = playerTransform.position.y;
        spawnPointPosition.x = spawnPositions[index].transform.position.x;
        spawnPointPosition.z = spawnPositions[index].transform.position.z;

        return spawnPointPosition;
    }
}