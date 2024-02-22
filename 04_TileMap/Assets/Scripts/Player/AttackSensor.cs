using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if(slime != null )
        {
            slime.ShowOutline();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null)
        {
            slime.ShowOutline(false);
        }
    }
}
