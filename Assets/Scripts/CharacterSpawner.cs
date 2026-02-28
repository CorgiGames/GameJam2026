using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner main;

    // Add this line to fix the compiler error in CharacterMovement
    public static UnityEvent onCharacterDestroy = new UnityEvent();

    private void Awake()
    {
        main = this;
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