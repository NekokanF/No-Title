using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_check : MonoBehaviour
{
    //攻撃チェック判定
    public BoxCollider2D _Atk_check;

    //攻撃チェック
    public bool _Acheck;

    // Start is called before the first frame update

    void Awake()
    {
        _Atk_check = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        _Atk_check.enabled = true;
        _Acheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _Acheck = true;
            _Atk_check.enabled = false;
        }
    }
}
