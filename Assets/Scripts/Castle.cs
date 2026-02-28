using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI References")]
    public TextMeshProUGUI healthText;
    public Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"[Castle] Başlangıç canı set edildi: {currentHealth}");
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"[Castle] TakeDamage çağrıldı! Alınan hasar: {damage}. Mevcut can: {currentHealth}");
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateUI();
        Debug.Log($"[Castle] Hasar sonrası yeni can: {currentHealth}");

        if (currentHealth <= 0)
        {
            WinGame();
        }
    }

    private void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
            Debug.Log($"[Castle] UI Yazısı güncellendi: {healthText.text}");
        }
        else
        {
            Debug.LogWarning("[Castle] UYARI: healthText referansı atanmamış!");
        }

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            Debug.Log($"[Castle] UI Bar Fill güncellendi: {healthBarFill.fillAmount}");
        }
        else
        {
            Debug.LogWarning("[Castle] UYARI: healthBarFill referansı atanmamış!");
        }
    }

    private void WinGame()
    {
        Debug.Log("[Castle] OYUN KAZANILDI! Kale yok edildi.");
    }
}