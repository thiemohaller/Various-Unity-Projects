using UnityEngine;

public class Target : MonoBehaviour {
    public float health = 50f;

    public void TakeDamage(float damageAmount) {
        health -= damageAmount;
        if (health <= 0f) {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
