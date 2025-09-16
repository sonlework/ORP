using Unity.Cinemachine;
using UnityEngine;


public class CameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        CameraManager.RegisterCamera(GetComponent<CinemachineCamera>());
    }
    private void OnDisable()
    {
        CameraManager.UnregisterCamera(GetComponent<CinemachineCamera>());
    }
}

