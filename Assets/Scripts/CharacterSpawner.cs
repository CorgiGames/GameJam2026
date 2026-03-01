using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner main;
    [Header("Decoy Settings")]
    [SerializeField] private Vector3 decoySpawnCoordinates = new Vector3(10f, 0f, 5f);

    public Transform pathSpawnPoint;
    public static UnityEvent onCharacterDestroy = new UnityEvent();
    private GameObject prefabToPlace;
    private bool isPlacing = false;

    // Tracks active units and decoys on the board
    public int activeUnitCount = 0;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // Add listener to decrease count when a unit is destroyed
        onCharacterDestroy.AddListener(DecreaseUnitCount);
    }

    public void DecreaseUnitCount()
    {
        activeUnitCount--;
        if (activeUnitCount < 0) activeUnitCount = 0;
    }

    public void SpawnDecoyOnRandomPathPoint(GameObject prefab)
    {
        var path = LevelManager.main.path;

        if (path != null && path.Length > 0)
        {
            int randomIndex = Random.Range(0, path.Length);
            Transform chosenPoint = path[randomIndex];

            Vector3 spawnPos = chosenPoint.position + new Vector3(0, 0.1f, 0);

            Instantiate(prefab, spawnPos, Quaternion.identity);
            
            // Increment active unit counter for decoy
            activeUnitCount++;

            Debug.Log($"Decoy successfully spawned on path point {randomIndex} at {spawnPos}");
        }
        else
        {
            Debug.LogError("CharacterSpawner: The path array in LevelManager is empty or null!");
        }
    }
    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityRoutine(duration));
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        // 1. Find all active characters (adjust 'Health' to your actual script name)
        Health[] allUnits = Object.FindObjectsByType<Health>(FindObjectsSortMode.None);

        // 2. Turn on Shields
        foreach (Health h in allUnits)
        {
            h.SetInvincible(true);
        }

        Debug.Log("Shields Active!");

        // 3. Wait for 5 seconds
        yield return new WaitForSeconds(duration);

        // 4. Turn off Shields
        Health[] remainingUnits = Object.FindObjectsByType<Health>(FindObjectsSortMode.None);
        foreach (Health h in remainingUnits)
        {
            h.SetInvincible(false);
        }

        Debug.Log("Shields Expired!");
    }
    public void SpawnCharacter(GameObject characterPrefab)
    {
        if (characterPrefab == null)
        {
            Debug.LogError("No prefab assigned to this button!");
            return;
        }
        Instantiate(characterPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
        
        // Increment active unit counter for unit
        activeUnitCount++;
        
        Debug.Log($"Spawned: {characterPrefab.name}");
    }
}