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

private void OnTriggerEnter2D(Collider2D collision)
{
    // Adım 1: Çarpılan objenin adını ve tag'ini yazdır
    Debug.Log($"[Movement] Çarpışma gerçekleşti! Obje: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

    // Adım 2: Tag kontrolü (Büyük-küçük harf duyarlıdır!)
    if (collision.CompareTag("Castle"))
    {
        Debug.Log("[Movement] 'Castle' tag'i doğrulandı.");

        // Adım 3: Script erişim kontrolü
        Castle castle = collision.GetComponent<Castle>();
        
        if (castle != null)
        {
            Debug.Log("[Movement] Castle scripti bulundu, hasar gönderiliyor...");
            castle.TakeDamage(attackDamage);
            
            CharacterSpawner.onCharacterDestroy.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError($"[Movement] HATA: Çarpılan '{collision.gameObject.name}' objesinde 'Castle' scripti bulunamadı!");
        }
    }
    else
    {
        Debug.LogWarning($"[Movement] Çarpılan objenin tag'i 'Castle' değil! Mevcut tag: {collision.gameObject.tag}");
    }
}
}