using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

    public float speed;
    public float lifeTime;
    public float distance;
    public float damage;
    public LayerMask whatIsSolid;

    [SerializeField]
    private EnemyController AI;

    // Use this for initialization
    void Start()
    {
        AI = GetComponent<EnemyController>();
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.GetComponent<PlayerController>().TakeDamage(damage);
            }
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
