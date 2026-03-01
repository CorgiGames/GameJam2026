using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int attackDamage = 10;

    private Transform target;
    private int pathIndex = 0;

    public int GetPathIndex()
{
    return pathIndex;
}

    private void Start()
    {
        if (LevelManager.main != null && LevelManager.main.path.Length > 0)
        {
            target = LevelManager.main.path[pathIndex];
        }
    }

    private void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                CharacterSpawner.onCharacterDestroy?.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }  
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public void AddMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Castle"))
        {
            Castle castle = collision.GetComponent<Castle>();
            
            if (castle != null)
            {
                castle.TakeDamage(attackDamage);
                
                CharacterSpawner.onCharacterDestroy?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}