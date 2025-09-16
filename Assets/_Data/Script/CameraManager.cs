﻿using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static List<CinemachineCamera> cameras = new List<CinemachineCamera>();
    public static CinemachineCamera ActiveCamera = null;

    public static bool IsActiveCamera(CinemachineCamera cam)
    {
        return cam == ActiveCamera;
    }
    public static void SwitchCamera(CinemachineCamera newCam)
    {
        newCam.Priority = 10;
        ActiveCamera = newCam;
        foreach (CinemachineCamera cam in cameras)
        {
            if (cam != newCam)
                cam.Priority = 0;
        }
    }
    public static void RegisterCamera(CinemachineCamera cam)
    {
        if (!cameras.Contains(cam))
            cameras.Add(cam);
    }
    public static void UnregisterCamera(CinemachineCamera cam)
    {
        if (cameras.Contains(cam))
            cameras.Remove(cam);
    }
}
