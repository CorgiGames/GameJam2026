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
    // Add this line to fix the compiler error in CharacterMovement
    public static UnityEvent onCharacterDestroy = new UnityEvent();
    private GameObject prefabToPlace;
    private bool isPlacing = false;


    private void Awake()
    {
        main = this;
    }
    public void SpawnDecoyOnRandomPathPoint(GameObject prefab)
    {
        // 1. Access the path array from your LevelManager
        // Assuming LevelManager.main.path is an array of Transforms
        var path = LevelManager.main.path;

        if (path != null && path.Length > 0)
        {
            // 2. Pick a random point on the path
            int randomIndex = Random.Range(0, path.Length);
            Transform chosenPoint = path[randomIndex];

            // 3. Spawn the decoy at that point's position
            // I've added a 0.1f Y-offset to prevent "Z-fighting" with the ground
            Vector3 spawnPos = chosenPoint.position + new Vector3(0, 0.1f, 0);

            Instantiate(prefab, spawnPos, Quaternion.identity);

            Debug.Log($"Decoy successfully spawned on path point {randomIndex} at {spawnPos}");
        }
        else
        {
            Debug.LogError("TowerSpawner: The path array in LevelManager is empty or null!");
        }
    }
    public void SpawnCharacter(GameObject characterPrefab)
    {
        if (characterPrefab == null)
        {
            Debug.LogError("No prefab assigned to this button!");
            return;
        }
        Instantiate(characterPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
        Debug.Log($"Spawned: {characterPrefab.name}");
    }
}