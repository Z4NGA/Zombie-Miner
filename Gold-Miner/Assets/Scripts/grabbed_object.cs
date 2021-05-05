using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbed_object : MonoBehaviour
{
    enum object_type
    {
        ore,
        stone,
        diamond,
        dynamite,
        money_bag
    }
    public GameObject particule;
    [SerializeField] private object_type type;
    [SerializeField] private float weight = 1;
    [SerializeField] private int price = 100;
    [SerializeField] private string description="";
    public int get_price(float stone_knowledge,float gem_affinity)
    {
        if (type == object_type.diamond) return Mathf.FloorToInt(price * gem_affinity);
        if (type == object_type.stone) return Mathf.FloorToInt(price * stone_knowledge);
        if (type == object_type.dynamite) return 1;
        return price;
    }
    public float get_weight() 
    { 
        return weight;
    }
    public void explode()
    {
        GameObject explosion = Instantiate(particule, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
        Destroy(gameObject);
    }
}
