using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;         
    public Image fillImage;       
    public Vector3 offset = new Vector3(0f, 1.0f, 0f);

    void LateUpdate()
    {
        if (health == null) Destroy(gameObject);

        
        transform.position = health.transform.position + offset;

        
        float ratio = (float)health.CurrentHP / health.MaxHP;
        fillImage.fillAmount = Mathf.Clamp01(ratio);
    }
}