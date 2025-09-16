using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class RespawnManager : BaseManager<RespawnManager>
{
    [SerializeField] private float respawnTime = 2f;
    [SerializeField] private Transform playerTf;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform defaultPos;
    [SerializeField] private CinemachineCamera vcamPlayer;


    protected override void Awake()
    {
        base.Awake();

        if (playerTf != null)
            SceneManager.MoveGameObjectToScene(playerTf.gameObject, SceneManager.GetActiveScene());
    }

    private void Start()
    {
        if (playerTf == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTf = playerObj.transform;
        }

        if (playerTf != null && defaultPos != null)
        {
            defaultPos.position = playerTf.position;
            SetRespawnPoint(playerTf);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerTf != null)
            SceneManager.MoveGameObjectToScene(playerTf.gameObject, scene);
    }

    public void Respawn(Action onComplete = null)
    {
        StartCoroutine(RespawnCo(onComplete));
    }

    private IEnumerator RespawnCo(Action onComplete = null)
    {

        if (playerTf == null || playerTf.gameObject == null)
            yield break;

        var healthCtrl = playerTf.GetComponent<PlayerHealthController>();
        var playerCtrl = playerTf.GetComponent<PlayerController>();

        playerTf.gameObject.SetActive(false);

        if (healthCtrl != null)
            healthCtrl.StopAllCoroutines();

        yield return new WaitForSeconds(respawnTime);

        if (playerTf == null || playerTf.gameObject == null)
            yield break;

        if (respawnPoint != null)
            playerTf.position = respawnPoint.position;

        healthCtrl?.ResetHealth();
        playerCtrl?.ResetDashes();

        playerTf.gameObject.SetActive(true);

        if (vcamPlayer != null)
            vcamPlayer.Follow = playerTf;

        EnemyHealthController[] enemies = FindObjectsByType<EnemyHealthController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            enemy.Respawn();
        }
    }

    public void SetRespawnPoint(Transform newRespawnPosition)
    {
        if (newRespawnPosition != null && respawnPoint != null)
            respawnPoint.position = newRespawnPosition.position;
    }

    public void SetDefaultPosition()
    {
        if (defaultPos != null && respawnPoint != null)
            respawnPoint.position = defaultPos.position;
    }
}
