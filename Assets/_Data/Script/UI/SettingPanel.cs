using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] public Slider _bgmSlider, _sfxSlider;

    public void TogleMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleBGM();
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }
    public void TogleSFX()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSFX();
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }
    public void ChangeBGMVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.BGMVolume(_bgmSlider.value);
        }
    }
    public void ChangeSFXVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SFXVolume(_sfxSlider.value);
        }
    }
    public void SaveSetting()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.SettingPanel.gameObject.SetActive(false);
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("btnClick");
        }
    }
}

