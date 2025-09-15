using UnityEngine;


public class PausePanel : MonoBehaviour
{

    public void OnClickResume()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
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
}

