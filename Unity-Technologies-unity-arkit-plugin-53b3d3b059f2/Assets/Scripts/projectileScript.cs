using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private Vector3 targetPos;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2)
        {
            Destroy(this.gameObject);
        }
        if (target != null)
        {
            targetPos = target.transform.position;
        }
        Vector3 towards = targetPos - transform.position;
        towards = towards.normalized * speed;
        GetComponent<Rigidbody>().velocity = towards;
        transform.rotation = Quaternion.LookRotation(towards);

    }
}
