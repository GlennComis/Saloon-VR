using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static bool isHost;

    public void SetIsHost(bool value)
    {
        isHost = value;
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetIsHost(true);
        }
      
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetIsHost(false);
        }
    }
}
