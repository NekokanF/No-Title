using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player2Manager : MonoBehaviour
{
    private Enemy_st Enemy_st;

    //弱判定
    public BoxCollider2D _bx;

    //強判定
    public BoxCollider2D _sbx;

    //リジッドボディ
    private Rigidbody2D _rb;

    //ステータス
    public int _st;

    //アニメーター
    private Animator _animator;

    //スピード
    public float _speed;

    //移動量x
    private float _vx;

    //ジャンプ力
    public float _jp_p;

    //スライド速度
    private float _sds = 9.0f;

    //ぶつかったときのノックバック速度
    private float _KBS1 = -4.0f;

    private float _KBS2 = -3.0f;

    //空中ダッシュ速度
    private float _AirDs;

    //接地
    public bool _gf;

    //左右判断
    public int _RLD;
    //_RLD=1-右
    //_RLD=2-左

    //ダメージカウント
    public int _dct;

    //移動タイマー
    public float _timer;

    //攻撃タイマー
    public float _a_timer;

    //スライディングタイマー
    public float _s_timer;

    //スライディング判定タイマー
    public float _sh_timer;

    //空中ダッシュタイマー
    public float _ad_timer;

    //回復タイマー
    public float _heal_timer;

    //立ち判定
    public CapsuleCollider2D _standcol;

    //スライディング判定
    public CapsuleCollider2D _slidecol;

    //プレイヤー
    public GameObject _pla;

    //点滅用
    public SpriteRenderer sp;

    //スタミナゲージ
    public int maxSt = 2500;
    public int currentSt;

    //スタミナslider
    public Slider _Stslider;

    //スタミナカウント
    public int _sct;

    //スタミナ回復クールタイム
    public float _stcd;

    //
    public bool _damage;

    //Hpゲージ
    public int maxHp;
    public int currentHp;

    //Hp slider
    public Slider _Hpslider;

    //Hp回復回数カウント
    public int _healct;

    public bool _atkcheack;

    public bool _healcheck;
    public bool _healcheck2;

    //無敵カウント
    public bool _muteki;

    //死亡時無敵カウント
    public bool _death_muteki;

    //無敵タイマー
    public float _muteki_timer;

    //攻撃力
    public int _Atk;

    //防御力(仮)
    public int _DEF;
    private int _kekka;

    //回復パーティクル
    public ParticleSystem Heal_particle;

    //攻撃当たり判定
    public bool _check;

    //count
    public int countst;

    //オーディオ
    private AudioSource _audio;
    public AudioClip Heal;
    public AudioClip attack;
    public AudioClip Hit;
    public AudioClip slide;

    //_st=1-待機
    //_st=2-移動
    //_st=3-ジャンプ
    //_st=4-降下
    //_st=5-弱攻撃
    //_st=6-強攻撃
    //_st=7-右スライディング
    //_st=8-左スライディング
    //_st=9-被ダメージ(ぶつかったとき)
    //_st=10-死亡
    //_st=11-空中ダッシュ
    //_St=13-被ダメージ(攻撃受けた時)

    private SpriteRenderer _sp;

    // Start is called before the first frame update
    void Awake()
    {
        countst = 0;
        _animator = GetComponent<Animator>();
        _sp = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }
    void Start()
    {
        _st = 1;
        _Atk = 20;
        _speed = 6;
        _jp_p = 14;
        _atkcheack = false;
        _sct = 0;
        _AirDs = 15;
        _healct = 2;
        _healcheck = false;
        _healcheck2 = false;
        _muteki = false;
        _death_muteki = false;
        _animator.Play("Idle");
        _damage = false;
        _gf = true;
        _standcol.enabled = true;
        _slidecol.enabled = false;
        _bx.enabled = false;
        _sbx.enabled = false;
        maxSt = 2500;
        maxHp = 200;
        //sliderを満タンにする
        _Stslider.value = 1;
        currentSt = maxSt;
        _Hpslider.value = 1;
        currentHp = maxHp;
        _check = false;
        Heal_particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    // Update is called once per frame
    void Update()
    {
        _vx = 0;
        float x = Input.GetAxisRaw("Horizontal")*Time.deltaTime;
        Vector3 scale = transform.localScale;
        //ダメージを受けた時の点滅
        if (_damage == true)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 20));
            sp.color = new Color(1f, 1f, 1f, level);
        }
        //体力回復
        if (Input.GetKeyDown("e") && _healct!=0)
        {
            if(_st!=10)
            {
                _audio.clip = Heal;
                _audio.Play();
                Heal_particle.Play(true);
                _healcheck = true;
                int Hpheal = 120;
                currentHp = currentHp + Hpheal;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
                _healct -= 1;
            }
            
        }
        //移動系
        if (Input.GetKey("d") && (_st == 1 || _st == 2 || _st == 3 || _st == 4))
        {
            _RLD = 1;
            _vx = _speed;
            scale.x = 6;
            transform.localScale = scale;
        }
        else if (Input.GetKey("a") && (_st == 1 || _st == 2 || _st == 3 || _st == 4))
        {
            _RLD = 2;
            _vx = -_speed;
            scale.x = -6;
            transform.localScale = scale;
        }
        //ダッシュ
        if (Input.GetKeyDown(KeyCode.LeftShift) && (_Stslider.value != 0))
        {
            _speed = 9;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && (_Stslider.value != 0))
        {
            _speed = 6;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && _st !=1)
        {
            int Stdamage = 1;
            currentSt = currentSt - Stdamage;
            _Stslider.value = (float)currentSt / (float)maxSt; ;
        }
        //ジャンプ
        if (Input.GetKeyDown("space"))
        {
            if ((_st == 1 || _st == 2) && (_gf == true))
            {
                _timer = 0;
                _st = 3;
                _animator.Play("jump");
            }
        }
        //弱攻撃
        if (Input.GetMouseButtonDown(0) && _st==1)
        {
            _a_timer = 0;
            _st = 5;
            _animator.Play("Attack");
            _audio.clip = attack;
            _audio.Play();
        }
        //強攻撃
        if (Input.GetMouseButtonDown(1) && _st == 1)
        {
            _a_timer = 0;
            _st = 6;
            _animator.Play("Dash-Attack");
            _audio.clip = attack;
            _audio.Play();
        }
        //弱攻撃
        if (Input.GetMouseButtonDown(0) && _st == 2)
        {
            _a_timer = 0;
            _st = 5;
            _animator.Play("Attack");
            _audio.clip = attack;
            _audio.Play();

        }
        //強攻撃
        if (Input.GetMouseButtonDown(1) && _st == 2)
        {
            _a_timer = 0;
            _st = 6;
            _animator.Play("Dash-Attack");
            _audio.clip = attack;
            _audio.Play();
        }
        
        //空中ダッシュ
        if (Input.GetMouseButtonDown(2)&&_st==4|_st==11&&_Stslider.value!=0)
        {
            _audio.clip = slide;
            _audio.Play();
            int Stdamage = 1255;
            currentSt = currentSt - Stdamage;
            _Stslider.value = (float)currentSt / (float)maxSt; ;
            _ad_timer = 0;
            _st = 11;
            _animator.Play("Dash");
        }
        //右スライディング
        else if (Input.GetMouseButton(2) && (Input.GetKey("d") && _st == 2) && (_Stslider.value != 0))
        {
            _audio.clip = slide;
            _audio.Play();
            _RLD = 1;
            int Stdamage = 500;
            currentSt = currentSt - Stdamage;
            _Stslider.value = (float)currentSt / (float)maxSt; ;
            _sh_timer = 0;
            _s_timer = 0;
            _st = 7;
            _standcol.enabled = false;
            _slidecol.enabled = true;
            _animator.Play("Slide");
        }
        //左スライディング
        else if (Input.GetMouseButton(2) && (Input.GetKey("a") && _st == 2)&&(_Stslider.value!=0))
        {
            _audio.clip = slide;
            _audio.Play();
            int Stdamage = 500;
            currentSt = currentSt - Stdamage;
            _Stslider.value = (float)currentSt / (float)maxSt; ;
            _RLD = 2;
            _sh_timer = 0;
            _s_timer = 0;
            _st = 8;
            _standcol.enabled = false;
            _slidecol.enabled = true;
            _animator.Play("Slide");
        }
        //待機スタミナ回復
        else if (_st == 1&&currentSt>=1&&currentSt<=2500)
        {
            int Stheal = 3;
            currentSt = currentSt + Stheal;
            _Stslider.value = (float)currentSt / (float)maxSt; ;
        }
        //ダッシュスタミナ消費
        else if (_st == 2 && currentSt >= 1 && currentSt <= 2500&&!(Input.GetKey(KeyCode.LeftShift)))
        {
            int Stheal = 2;
            currentSt = currentSt + Stheal;
            _Stslider.value = (float)currentSt / (float)maxSt; 
        }
        if (_Stslider.value == 0 & _sct == 0&_speed==9)
        {
            _speed = 6;
        }
        if (_Stslider.value == 0 && _sct == 0)
        {
            _sct = 1;
        }
    }
    void FixedUpdate()
    {
        if (currentSt <= -1)
        {
            currentSt = 0;
        }
        //走り
        if (_st == 1)
        {
            if (_vx != 0)
            {
                _st = 2;
                _animator.Play("Run");
            }
        }
        else if (_st == 2)
        {
            transform.Translate(_vx / 50, 0, 0);
            if (_vx == 0)
            {
                _st = 1;
                _animator.Play("Idle");
            }
        }
        else if (_st == 3)
        {
            _st = 4;
            _rb.AddForce(new Vector2(0, _jp_p), ForceMode2D.Impulse);
            _animator.Play("jump");
        }
        else if (_st == 4)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                _timer = 0;
                _animator.Play("Fall");
            }
        }
        if (_st == 2 && _gf == false)
        {
            _st = 4;
            _animator.Play("Fall");
        }
        else if (_st == 5)
        {
            _a_timer += Time.deltaTime;
            if (_a_timer >= 0.35f)
            {
                _bx.enabled = true;
                if (_a_timer >= 0.45f)
                {
                    _bx.enabled = false;
                    if (_a_timer >= 0.7f)
                    {
                        _audio.Stop();
                        _a_timer = 0;
                        _st = 1;
                        _bx.enabled = false;
                        _gf = true;
                        _animator.Play("Idle");
                    }
                }
            }
        }
        else if (_st == 6)
        {
            _a_timer += Time.deltaTime;
            if((_a_timer >= 0.2f))
            {
                _audio.clip = attack;
                _audio.Play();
                if (_a_timer >= 0.35f)
                {
                    if (_a_timer >= 0.4f)
                    {
                        _sbx.enabled = true;
                        if (_a_timer >= 0.5f)
                        {
                            _sbx.enabled = false;
                            if (_a_timer >= 1.02f)
                            {
                                _audio.Stop();
                                _a_timer = 0;
                                _st = 1;
                                _sbx.enabled = false;
                                _gf = true;
                                _animator.Play("Idle");
                            }
                        }


                    }

                }
            }
        }
        else if (_st == 7)
        {
            this.transform.position += new Vector3(_sds * Time.deltaTime, 0, 0);
            _s_timer += Time.deltaTime;
            _sh_timer += Time.deltaTime;
            if (_s_timer >= 0.7f)
            {
                _s_timer = 0;
                _st = 1;
                _standcol.enabled = true;
                _slidecol.enabled = false;
                _animator.Play("Idle");
            }
        }
        else if (_st == 8)
        {
            this.transform.position -= new Vector3(_sds * Time.deltaTime, 0, 0);
            _s_timer += Time.deltaTime;
            _sh_timer += Time.deltaTime;
            if (_s_timer >= 0.7f)
            {
                _s_timer = 0;
                _st = 1;
                _standcol.enabled = true;
                _slidecol.enabled = false;
                _animator.Play("Idle");
            }
        }
        else if (_sh_timer >= 0.6f)
        {
            _standcol.enabled = true;
            _slidecol.enabled = false;
            _sh_timer = 0;
        }
        //ぶつかったときのノックバック
        else if (_st == 9)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                _timer = 0;
                _bx.enabled = false;
                _damage = true;
                _gf = true;
                _st = 1;
                _animator.Play("Idle");
                if (_speed >= 9)
                {
                    _speed = 6;
                }
            }
            if (_RLD == 1)
            {
                this.transform.position += new Vector3(_KBS1 * Time.deltaTime, 0, 0);
            }
            if (_RLD == 2)
            {
                this.transform.position -= new Vector3(_KBS1 * Time.deltaTime, 0, 0);
            }
        }
        //攻撃を受けたとき
        else if (_st == 13)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                _timer = 0;
                _sbx.enabled = false;
                _bx.enabled = false;
                _damage = true;
                _gf = true;
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                _st = 1;
                _animator.Play("Idle");
                if (_speed >= 9)
                {
                    _speed = 6;
                }
            }
            if (_RLD == 1)
            {
                this.transform.position += new Vector3(_KBS2 * Time.deltaTime, 0, 0);
            }
            if (_RLD == 2)
            {
                this.transform.position -= new Vector3(_KBS2 * Time.deltaTime, 0, 0);
            }
        }
        //死亡
        if (currentHp<=0)
        {
            _death_muteki = true;
            _st = 10;
            _animator.Play("Death");
            if (_st == 10)
            {
                _timer += Time.deltaTime;
                if (_timer >= 3.0f)
                {
                    if (countst == 1)
                    {
                        SceneManager.LoadScene("Stage1GAMEOVER");
                    }
                    if (countst == 2)
                    {
                        SceneManager.LoadScene("Stage2GAMEOVER");
                    }
                    if (countst == 3)
                    {
                        SceneManager.LoadScene("Stage3GAMEOVER");
                    }
                }
            }
        }
        //空中ダッシュ
        if(_st==11)
        {
            _ad_timer += Time.deltaTime;
            if (_RLD == 1)
            {
                _rb.constraints= RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                transform.Translate(_AirDs/50, 0, 0);
            }
            if (_RLD == 2)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                transform.Translate(-_AirDs/50, 0, 0);
            }
            if (_ad_timer >= 0.3f)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                _st = 4;
                _ad_timer = 0;
                _animator.Play("Fall");
            }

        }
        if (_st == 3 || _st == 4)
        {
            transform.Translate(_vx / 50, 0, 0);
        }
        //スタミナクールダウン
        if (_sct == 1)
        {
            _stcd+=Time.deltaTime;
            if (_stcd >= 5.0f)
            {
                int Stheal = 2500;
                currentSt = currentSt + Stheal;
                _Stslider.value = (float)currentSt / (float)maxSt; ;
                _stcd = 0;
                _sct = 0;
            }
        }
        if (_healcheck == true)
        {
            _heal_timer += Time.deltaTime;
            if (_heal_timer >= 0.5f)
            {
                _heal_timer = 0;
                Heal_particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _healcheck = false;

            }
            if (currentHp>maxHp)
            {
                currentHp = maxHp;
            }
        }
        if(_muteki == true)
        {
            _muteki_timer += Time.deltaTime;
            if(_muteki_timer >= 0.8f&&_st!=10)
            {
                _muteki_timer = 0;
                _muteki = false;
                if(_muteki_timer >= 3.0f && _st == 10)
                {
                    _muteki_timer = 0;
                    _muteki = false;
                }

            }
        }
    }
    public void countstup()
    {
        countst += 1;
    }
    public void countupst2()
    {
        countst += 2;
    }
    public void countupst3()
    {
        countst += 3;
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_st != 5)
        {
            if (_st != 6)
            {
                if (_st != 12)
                {
                    if (other.gameObject.tag == "Ground" && _st != 9)
                    {
                        _gf = true;
                        _st = 1;
                        _animator.Play("Idle");
                    }
                    
                }

            }
        }
        if(other.gameObject.tag =="reverse")
        {

        }
        //ダメージ受け
        if (other.gameObject.tag == ("Enemy1Attack")&&_muteki==false)
        {
            _audio.clip = Hit;
            _audio.Play();
            _muteki = true;
            _st = 13;
            StartCoroutine(OnDamage());
            int Hpdamage = 40;
            _kekka = (Hpdamage - (_DEF / 5));
            _Hpslider.value = (float)currentHp / (float)maxHp; ;
            _animator.Play("Hurt");
            if (_kekka >= 0)
            {
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
            else if (_kekka <= -1)
            {
                _kekka = 1;
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
        }
        if (other.gameObject.tag == ("Enemy1Attack1") && _muteki == false)
        {
            _audio.clip = Hit;
            _audio.Play();
            _muteki = true;
            _st = 13;
            StartCoroutine(OnDamage());
            int Hpdamage = 70;
            _kekka = (Hpdamage - (_DEF / 5));
            _Hpslider.value = (float)currentHp / (float)maxHp; ;
            _animator.Play("Hurt");
            if (_kekka >= 0)
            {
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
            else if (_kekka <= -1)
            {
                _kekka = 1;
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
        }
        if (other.gameObject.tag == ("Enemy1Attack2") && _muteki == false)
        {
            _audio.clip = Hit;
            _audio.Play();
            _muteki = true;
            _st = 13;
            StartCoroutine(OnDamage());
            int Hpdamage = 120;
            _kekka = (Hpdamage - (_DEF / 5));
            _Hpslider.value = (float)currentHp / (float)maxHp; ;
            _animator.Play("Hurt");
            if (_kekka >= 0)
            {
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
            else if (_kekka <= -1)
            {
                _kekka = 1;
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
        }
        if (other.gameObject.tag == ("Enemy1Attack3") && _muteki == false)
        {
            _audio.clip = Hit;
            _audio.Play();
            _muteki = true;
            _st = 13;
            StartCoroutine(OnDamage());
            int Hpdamage = 180;
            _kekka = (Hpdamage - (_DEF / 5));
            _Hpslider.value = (float)currentHp / (float)maxHp; ;
            _animator.Play("Hurt");
            if (_kekka >= 0)
            {
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
            else if (_kekka <= -1)
            {
                _kekka = 1;
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
        }
        if (other.gameObject.tag == ("Enemy1Attack4") && _muteki == false)
        {
            _audio.clip = Hit;
            _audio.Play();
            _muteki = true;
            _st = 13;
            StartCoroutine(OnDamage());
            int Hpdamage = 200;
            _kekka = (Hpdamage - (_DEF / 5));
            _Hpslider.value = (float)currentHp / (float)maxHp; ;
            _animator.Play("Hurt");
            if (_kekka >= 0)
            {
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
            else if (_kekka <= -1)
            {
                _kekka = 1;
                currentHp = currentHp - _kekka;
                _Hpslider.value = (float)currentHp / (float)maxHp; ;
            }
        }


    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(_st!=5)
        {
            if(_st!=6)
            {
                if (other.gameObject.tag == "Ground")
                {
                    _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    _gf = false;
                }
            }
        }
    }
    public IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(0.8f);
        _damage = false;
        sp.color = new Color(1f, 1f, 1f, 1f);
    }
}
        
