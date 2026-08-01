using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverse_manager : MonoBehaviour
{
    //
    public Enemy_st Enemy_St;
    private BoxCollider2D _bx;

    // Start is called before the first frame update
    void Start()
    {
        _bx = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Enemy_St._Est==2)
        {
            _bx.enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Enemy_St.reverse();
        }
    }
}
