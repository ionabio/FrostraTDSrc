using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float speed = 2f;

    private Transform target;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {

        target = Level.instance.paths[index];
        rb.position = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            index++;
            if (index >= Level.instance.paths.Length)
            {
                Level.instance.RemoveLife();
                Destroy(gameObject);
                return;
            }
            target = Level.instance.paths[index];
        }
    }

    void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        rb.velocity = direction.normalized * speed;
    }
    
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void Slow()
    {
        speed = speed/2.0f;
    }
}
