using UnityEngine;

public class HealthBarSpawner : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Vector3 offset = new Vector3(0f, 1.0f, 0f);

    void Start()
    {
        Health h = GetComponent<Health>();
        if (h == null || hpBarPrefab == null) return;

        GameObject barObj = Instantiate(hpBarPrefab, transform.position + offset, Quaternion.identity);

        HealthBar hb = barObj.GetComponent<HealthBar>();
        hb.health = h;
        hb.offset = offset;
    }
}