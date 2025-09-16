using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System.Security.Cryptography;


#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlatformGameManager : BaseManager<PlatformGameManager>
{
    [SerializeField] private CinemachineCamera vcamPlayer;
    [SerializeField] private Transform playerTf;
    protected override void Awake()
    {
        base.Awake();

    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SceneManager.MoveGameObjectToScene(player, scene);
        }

        if (vcamPlayer != null)
        {
            vcamPlayer.Follow = player.transform;
            vcamPlayer.gameObject.SetActive(true);
        }
    }
    private void Start()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM("Menu Theme");
        }

        Time.timeScale = 0;
    }

    public void StartGame()
    {
        RestartGame();
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.gameObject.SetActive(true);
            UIManager.Instance.MenuPanel.gameObject.SetActive(false);
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM("City Theme");
        }
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.PausePanel.gameObject.SetActive(true);
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.PausePanel.gameObject.SetActive(false);
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.WinPanel.gameObject.SetActive(false);
            UIManager.Instance.LoosePanel.gameObject.SetActive(false);
            UIManager.Instance.PausePanel.gameObject.SetActive(false);
        }

        if (RespawnManager.HasInstance)
        {
            RespawnManager.Instance.SetDefaultPosition();
            RespawnManager.Instance.Respawn();
        }
    }


    public void ReturnMenu()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.gameObject.SetActive(false);
            UIManager.Instance.WinPanel.gameObject.SetActive(false);
            UIManager.Instance.LoosePanel.gameObject.SetActive(false);
            UIManager.Instance.PausePanel.gameObject.SetActive(false);
            UIManager.Instance.MenuPanel.gameObject.SetActive(true);
            UIManager.Instance.MenuPanel.OnClickReturnMenu();
        }
        if (RespawnManager.HasInstance)
        {
            RespawnManager.Instance.SetDefaultPosition();
            RespawnManager.Instance.Respawn();
        }
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM("Menu Theme");
            AudioManager.Instance.PlaySFX("btnClick");
        }

    }
    public void WinGame()
    {
        Time.timeScale = 0;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.ActiveBossHealth(false);
            UIManager.Instance.WinPanel.gameObject.SetActive(true);
        }
    }

    public void LooseGame()
    {

        Time.timeScale = 0;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.LoosePanel.gameObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
