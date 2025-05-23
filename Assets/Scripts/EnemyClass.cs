using UnityEngine;
using System.Collections;

public class EnemyClass : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float fallingSpeed = 10f;
    private bool isHit = false;
    private Renderer rend;
    public GameObject enemyModelPivot;
    public GameObject explosionFXPrefab;

    void Start()
    {
        // หา Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        // สุ่มตำแหน่งและความเร็ว
        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(1f, 7f);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY / Camera.main.pixelHeight, 20f));
        transform.position = new Vector3(worldPos.x, randomY, 20f);

        speed = Random.Range(2f, 6f);

        // Get Renderer for color effect
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!isHit && target != null)
        {
            // find the distance between the enemy and the target
            Vector3 distance = target.position - transform.position;

            // normalize the distance vector to get the direction
            Vector3 direction = distance.normalized;

            // move the enemy towards the target
            transform.position += direction * speed * Time.deltaTime;

            // Rotate towards the target
            enemyModelPivot.transform.LookAt(target.position);
        }
        else if (isHit)
        {
            // If hit, stop moving
            speed = 0f;

            // Move the enemy ship downwards
            transform.position += Vector3.down * fallingSpeed * Time.deltaTime;

            // Rotate the enemy ship
            transform.Rotate(Vector3.forward * 180f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit && other.CompareTag("Bullet"))
        {
            FindObjectOfType<MainLogic>()?.AddScore();
            isHit = true;
            Destroy(gameObject, 2f);
            //StartCoroutine(FlashRedAndDestroy());

            // Instantiate explosion effect
            if (explosionFXPrefab != null)
            {
                GameObject explosionFX = Instantiate(explosionFXPrefab, transform.position, Quaternion.identity);
                Destroy(explosionFX, 2f); // Destroy the explosion effect after 2 seconds
            }
        }
    }

    private IEnumerator FlashRedAndDestroy()
    {
        float duration = 0.9f;
        float timer = 0f;
        bool toggle = false;

        while (timer < duration)
        {
            if (rend != null)
            {
                rend.material.color = toggle ? Color.red : Color.white;
            }

            toggle = !toggle;
            timer += 0.15f;
            yield return new WaitForSeconds(0.15f);
        }

        Destroy(gameObject);
    }
}
