using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D _collider;
    public void ActivateShield()
    {
        
    }

    public void DeactivateShield()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // a very simplified system for reflection from the shield
        if (collision.GetComponent<Enemy>())
            collision.GetComponent<Enemy>().RevertDirection();
    }
}
