using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; // Input System paketi

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

    public void StartPlacingDecoy(GameObject prefab, GameObject cardObj, CardData cardData, TowerCombatManager manager)
    {
        if (isPlacing) return; 
        StartCoroutine(PlacementRoutine(prefab, cardObj, cardData, manager));
    }

    private IEnumerator PlacementRoutine(GameObject prefab, GameObject cardObj, CardData cardData, TowerCombatManager manager)
    {
        yield return null; 

        isPlacing = true;
        prefabToPlace = prefab;
        Debug.Log("Decoy yerleştirme modu aktif. Ekranda bir yere tıklayın.");

        while (isPlacing)
        {
            // Yeni Input System ile sol tık kontrolü
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                // Yeni Input System ile fare koordinatını alma
                Vector2 screenPosition = Mouse.current.position.ReadValue();
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPosition);
                mousePos.z = 0f; 

                Instantiate(prefabToPlace, mousePos, Quaternion.identity);
                activeUnitCount++;
                
                Debug.Log($"Decoy manuel olarak {mousePos} konumuna yerleştirildi.");

                if (manager != null)
                {
                    manager.PlayCard(cardObj, cardData);
                }

                isPlacing = false;
                prefabToPlace = null;
            }
            yield return null;
        }
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
        Health[] allUnits = Object.FindObjectsByType<Health>(FindObjectsSortMode.None);

        foreach (Health h in allUnits)
        {
            h.SetInvincible(true);
        }

        Debug.Log("Shields Active!");

        yield return new WaitForSeconds(duration);

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
        
        activeUnitCount++;
        
        Debug.Log($"Spawned: {characterPrefab.name}");
    }
}