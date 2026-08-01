using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    //リジッドボディ
    private Rigidbody _rb;
    //アニメーター
    private Animator _animator;

    //ステータス
    public int _st;

    //水カウント
    public int _ct;

    //スピード
    public float _speed;

    //移動量x
    private float _vx;

    //移動量z
    private float _vz;

    //ジャンプ力
    public float _jp;

    //接地
    private bool _gf;

    //タイマー
    private float _timer;

    //ボタンプッシュ
    private bool _ps_st;

    //_st=1-基本形
    //_st=2-移動
    //_st=3-ジャンプ前
    //_st=4-ジャンプ
    //_count=0-上昇
    //_count=1-トップ
    //_count=2-下降
    //_st=5-ジャンプ後

    int maxHp = 1000;
    int currentHp;
    //slider
    public Slider slider;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = this.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _ct = 0;
        _st = 1;
        _gf = true;
        //sliderを満タンにする
        slider.value = 1;
        currentHp = maxHp;
        Debug.Log("Start currentHp:" + currentHp);
    }

    // Update is called once per frame
    void Update()
    {
        _vx = 0;
        _vz = 0;

        if (currentHp <= -1)
        {
            currentHp = 0;
        }

        if (slider.value == 0&&_ct==0)
        {
            _speed -= 10;
            _ct = 1;
        }

        if (slider.value == 0 && _ct == 0)
        {
            _ct = 1;
        }

        if (Input.GetKey("d"))
        {
            _vx = _speed;
        }
        else if (Input.GetKey("a"))
        {
            _vx = -_speed;
        }

        if (Input.GetKey("w"))
        {
            _vz = _speed;
        }
        if (Input.GetKeyDown("space")&&_st==1|_st==2&&_ct==0)
        {
            int damage =50;
            Debug.Log("damage:" + damage);

            currentHp = currentHp - damage;
            Debug.Log("After currentHp:" + currentHp);

            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value:" + slider.value);
            if ((_st == 1 || _st == 2)&&_ps_st==false)
            {
                _ps_st = true;
                _st = 3;
                _timer = 0;;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)&&slider.value!=0)
        {
            _speed += 10;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && slider.value != 0)
        {
            _speed -= 10;
        }
        else if (Input.GetKey(KeyCode.LeftShift)&&_st!=1)
        {
            int damage = 3;
            Debug.Log("damage:" + damage);

            currentHp = currentHp - damage;
            Debug.Log("After currentHp:" + currentHp);

            slider.value = (float)currentHp / (float)maxHp; ;
            Debug.Log("slider.value:" + slider.value);
        }
        else
        {
            _ps_st = false;
        }
    }

    void FixedUpdate()
    {
        if (_st == 1)
        {
            if (_vx != 0 || _vz != 0)
            {
                _st = 2;
            }
        }
        else if (_st == 2)
        {
            transform.Translate(_vx / 50, 0, _vz / 50);
            if (_vx == 0 && _vz == 0)
            {
                _st = 1;
            }
        }
        else if (_st == 3)
        {
            {
                _timer = 0;
                _st = 4;
                _rb.AddForce(new Vector3(0, _jp, 0), ForceMode.Impulse);
            }
        }
        else if (_st == 5)
        {
            _st = 1;
        }
        if (_st == 2 || _st == 4)
        {
            transform.Translate(_vx / 50, 0, _vz / 50);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground"||collision.gameObject.tag=="Cube" && _st == 4)
        {
            _st = 5;
            _timer = 0;
            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _gf = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            _gf = false;
        }
    }
}
