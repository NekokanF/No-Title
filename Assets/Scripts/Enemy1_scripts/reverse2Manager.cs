using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverse2_manager : MonoBehaviour
{
    //
    public Enemy2_st Enemy2_St;
    private BoxCollider2D _bx;

    // Start is called before the first frame update
    void Start()
    {
        _bx = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Enemy2_St._Est == 2)
        {
            _bx.enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Enemy2_St.reverse();
        }
    }
}
