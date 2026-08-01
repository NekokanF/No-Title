using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverse3_manager : MonoBehaviour
{
    //
    public Enemy3_st Enemy3_St;
    private BoxCollider2D _bx;

    // Start is called before the first frame update
    void Start()
    {
        _bx = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Enemy3_St._Est == 2)
        {
            _bx.enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Enemy3_St.reverse();
        }
    }
}
