using UnityEngine;

public class HealerAbility : MonoBehaviour
{
    [SerializeField] private float healInterval = 2f;
    [SerializeField] private int healAmount = 2;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= healInterval)
        {
            HealFrontAlly();
            timer = 0f;
        }
    }

    private void HealFrontAlly()
    {
        CharacterMovement[] characters = FindObjectsByType<CharacterMovement>(FindObjectsSortMode.None);

        CharacterMovement frontCharacter = null;
        int highestPathIndex = -1;

        foreach (CharacterMovement cm in characters)
        {
            if (cm == null) continue;

            int index = cm.GetPathIndex();

            if (index > highestPathIndex)
            {
                highestPathIndex = index;
                frontCharacter = cm;
            }
        }

        if (frontCharacter != null)
        {
            Health hp = frontCharacter.GetComponent<Health>();

            if (hp != null && hp.CurrentHP < hp.MaxHP)
            {
                hp.Heal(healAmount);
                Debug.Log("Healer healed: " + frontCharacter.name);
            }
        }
    }
}