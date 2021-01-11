using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject prefabExplosion;
    public int numberOfBounces = 2;

    private float lastTimeBounce = 0f;

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("TopBoundary")) {
            ProjectileExplode();
        }

        if (collision.collider.CompareTag("Player")) {
            GameManager.Instance.KillPlayer(collision.collider.gameObject);
            ProjectileExplode();
        } else if (collision.collider.CompareTag("Projectile")) {
            ProjectileExplode();
        } else if (collision.collider.CompareTag("Wall")
                    || collision.collider.CompareTag("Ground")
                    || collision.collider.CompareTag("Boundary")) {

            Debug.Log("Bounce");

            if (Time.time >= lastTimeBounce + 0.05f) {
               if (numberOfBounces == 0) {
                   ProjectileExplode();
               } else {
                   numberOfBounces -= 1;
                   lastTimeBounce = Time.time;
                   // TODO implement audiomanager
                   // AudioManager.instance.Play("Bounce");
               }
            }
        }
    }

    private void ProjectileExplode() {
        // AudioManager.instance.Play("Explosion");
        //GameObject explosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        //Destroy(explosion, 10f);
        Destroy(this.gameObject);
        // add camerashake
    }
}
