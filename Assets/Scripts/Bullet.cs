using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Material frostMat;
    [Header("Attributes")]
    [SerializeField] private float speed = 1f;


    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    private bool isFrost = false;
    public void SetFrost()
    {
        isFrost = true;
        speed = speed * 2.0f;
        GetComponentInChildren<SpriteRenderer>().material = frostMat;
    }

    public void SetFire()
    {
        speed = speed * 3.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            //   Destroy(gameObject);
            return;
        }
        Vector2 direction = target.position - transform.position;
        rb.velocity = direction.normalized * speed;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //todo check if enemy is destroyable
            if (isFrost)
                collision.gameObject.GetComponent<EnemyMove>().Slow();
            else
            {
                Destroy(collision.gameObject);
                Level.instance.AddCoins(10);
            }

            Destroy(gameObject);
        }
    }
}
