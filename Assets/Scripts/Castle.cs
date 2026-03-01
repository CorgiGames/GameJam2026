using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour
{
    [Header("Data Source")]
    public PlayerDeckData playerDeckData;

    [Header("Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Scene Management")]
    public string round2DraftSceneName = "Draft2"; 
    public string winSceneName = "WinScene";

    [Header("UI References")]
    public TextMeshProUGUI healthText;
    public Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"[Castle] Starting health set: {currentHealth}");
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"[Castle] TakeDamage called! Damage taken: {damage}. Current health: {currentHealth}");
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateUI();
        Debug.Log($"[Castle] Health after damage: {currentHealth}");

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
            Debug.Log($"[Castle] UI Text updated: {healthText.text}");
        }
        else
        {
            Debug.LogWarning("[Castle] WARNING: healthText reference is unassigned!");
        }

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            Debug.Log($"[Castle] UI Bar Fill updated: {healthBarFill.fillAmount}");
        }
        else
        {
            Debug.LogWarning("[Castle] WARNING: healthBarFill reference is unassigned!");
        }
    }

    private void WinGame()
    {
        if (playerDeckData == null)
        {
            Debug.LogError("[Castle] PlayerDeckData is unassigned. Cannot determine next scene.");
            return;
        }

        if (playerDeckData.currentRound == 1)
        {
            Debug.Log("[Castle] Round 1 complete. Loading Round 2 Draft scene.");
            playerDeckData.currentRound = 2;
            SceneManager.LoadScene(round2DraftSceneName);
        }
        else
        {
            Debug.Log("[Castle] Final round complete. Loading WinScene.");
            SceneManager.LoadScene(winSceneName);
        }
    }
}