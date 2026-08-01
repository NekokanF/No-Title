using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_check : MonoBehaviour
{
    //プレイヤー検知範囲
    public BoxCollider2D _Player_check;

    //プレイヤー検知
    public bool _P_check;

    // Start is called before the first frame update

    void Awake()
    {
        _Player_check = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _Player_check.enabled = true;
        _P_check = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _P_check = true;
        }
    }
   
}
