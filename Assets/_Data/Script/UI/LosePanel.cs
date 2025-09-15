using UnityEngine;


public class LosePanel : MonoBehaviour
{
    public void OnClickRestart()
    {

        if (PlatformGameManager.HasInstance)
        {
            PlatformGameManager.Instance.RestartGame();
        }
        this.gameObject.SetActive(false);
    }
    public void OnClickReturnMenu()
    {
        if (PlatformGameManager.HasInstance)
        {
            PlatformGameManager.Instance.ReturnMenu();
        }
    }
    public void OnClickCheckPoint()
    {
        if (RespawnManager.HasInstance)
        {
            RespawnManager.Instance.Respawn();
        }

        if (PlatformGameManager.HasInstance)
        {
            Time.timeScale = 1;
        }

        this.gameObject.SetActive(false);
    }
}

