using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private GameObject busterIcon;
    [SerializeField] private GameObject saberIcon;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private Color playerHealthDefautColor;
    [SerializeField] private GameObject bossHealthGo;
    private Color currentColor;
    private int playerMaxHealth;

    public void SetMaxHealth(int maxHealthValue)
    {
        playerMaxHealth = maxHealthValue;
        playerHealthSlider.maxValue = maxHealthValue;
    }

    public void UpdateHealth(int currentHealthValue)
    {
        playerHealthSlider.value = currentHealthValue;
        float healthPercent = (float)currentHealthValue / playerMaxHealth;

        if (healthPercent <= 0.7f)
        {
            currentColor = Color.yellow;
        }
        else if (healthPercent <= 0.3f)
        {
            currentColor = Color.red;
        }
        else
        {
            currentColor = playerHealthDefautColor;
        }

        playerHealthSlider.fillRect.GetComponent<Image>().color = currentColor;
    }

    public void ResetHealth()
    {
        UpdateHealth(playerMaxHealth);
    }

    public void UpdateWeaponIcon(WeaponType newWeapon)
    {

        if (newWeapon == WeaponType.Buster)
        {
            busterIcon.SetActive(true);
            saberIcon.SetActive(false);
        }
        else if (newWeapon == WeaponType.LightSaber)
        {
            busterIcon.SetActive(false);
            saberIcon.SetActive(true);
        }
    }
    public void ActiveBossHealth(bool status)
    {
        bossHealthGo.SetActive(status);
    }
    public void SetBossMaxHealth(int maxHealthValue)
    {
        bossHealthSlider.maxValue = maxHealthValue;
    }

    public void UpdateBossHealth(int currentHealthValue)
    {
        bossHealthSlider.value = currentHealthValue;
    }
}
