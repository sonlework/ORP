using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private RectTransform mainMenuPanel;
    [SerializeField] private RectTransform stageSelectPanel;

    public void OnClickStartGame()
    {

        mainMenuPanel.gameObject.SetActive(false);
        stageSelectPanel.gameObject.SetActive(true);

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }
    public void OnClickSetting()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.SettingPanel.gameObject.SetActive(true);
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }

    public void OnClickQuitGame()
    {
        if (PlatformGameManager.HasInstance)
        {
            PlatformGameManager.Instance.QuitGame();
        }
    }
    public void OnClickStage1()
    {
        if (PlatformGameManager.HasInstance)
        {
            PlatformGameManager.Instance.StartGame();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("btnClick2");
        }
    }
    public void OnClickReturnMenu()
    {
        mainMenuPanel.gameObject.SetActive(true);
        stageSelectPanel.gameObject.SetActive(false);
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }
}
