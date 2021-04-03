using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grab_object : MonoBehaviour
{
    private Player p;
    private void Awake()
    {
        p = GameObject.Find("player").GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with object " + collision.name);
        if (p != null)
        {
            if (p.grabbed_object == null)
            {
                p.grabbed_object = collision.transform;
                p.offset_to_grabbed_object = collision.transform.position - transform.position;
            }
        }
        else Debug.Log("Failed to detect Player !!");
    }

}
