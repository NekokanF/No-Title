using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Enemy3_st : MonoBehaviour
{

    public GameObject _enemy;

    //���W�b�h�{�f�B
    private Rigidbody2D _rb;

    //�A�j���[�^�[
    private Animator _animator;

    //�^�C�}�[
    public float _timer;

    //��_���^�C�}�[
    public float _dtimer;

    //�X�e�[�^�X
    public int _Est;

    //�G�ő�̗�
    public int _E_maxHp;

    public int currentE_Hp;

    //�GHp slider
    public Slider _E_Hpslider;

    //�G�h���
    public int _E_def;

    //�G�_���[�W�v�Z����
    private int _E_kekka;

    //�R���C�_�[
    public CapsuleCollider2D _E_bx;

    //��_��
    public bool _hit;

    public int _Atk;

    //���ݒl
    public int _hirumi;

    public bool _damage;

    //�_�ŗp
    public SpriteRenderer sp;

    //sp�Q�[�W�Ǘ�
    public bool _spcheack;

    //�G�t�F�N�g�Ȃ�
    public ParticleSystem Death_particle;
    public ParticleSystem Attack4;
    public ParticleSystem Attackfirst;
    public ParticleSystem Attackfinal;

    //�ړ����x
    public float _speed;

    //�ړ���x
    private float _vx;

    //�v���C���[���m�����蔻��
    public BoxCollider2D _Playercheckbx;

    //�U�����m
    public bool _Acheck;

    //�U��1����
    public float _Atimer;
    //�U��2����
    public float _A2timer;
    public bool _A2check;
    //�U��3����
    public float _A3timer;
    public float _A4timer;
    public float _A5timer;
    public float _A6timer;

    //�U��1����
    public BoxCollider2D _Acoll;
    //�U��2����
    public BoxCollider2D _A2coll;
    //�U��3����
    public BoxCollider2D _A3coll;
    //�U��4����
    public BoxCollider2D _A4coll;
    //�U������5����
    public BoxCollider2D _A5coll;
    //�U������6����
    public BoxCollider2D _A6coll;

    //�A���[�g
    public GameObject alart;
    //�A���[�g2
    public GameObject alart2;
    //�A���[�g2
    public GameObject alart3;

    //����n
    public Player_check Player_check;
    public Attack_check Attack_check;
    public BoxCollider2D _Atkcheck;
    public BoxCollider2D _RorL;

    //�U���N�[���_�E��
    public bool _Atkcooldown;
    public float _Atkcooltimer;

    //�U�������_��
    public int _rdAtk;

    //�U�����
    public float _Turntimer;
    //�U������^�C�}�[���~�߂�
    public bool TurntimerStop;

    //�v���C���[�`�F�b�N
    private GameObject _Player;
    //�v���C���[�`�F�b�N�X�N���v�g
    private Player_check _Player_check;
    private GameObject _Player2;
    public Player2Manager Player2Manager;

    //_Est=1-�ҋ@
    //_Est=2-���S
    //_Est=3-��_��
    //_Est=4-�U��1
    //_Est=5-�U��2
    //_Est=6-�W�����v�z��
    //_Est=7-�ړ�
    //_Est=8-�U��3
    //_Est=9-����

    Transform playerTr; // �v���C���[��Transform
    public float speed; // �G�̓����X�s�[�h
    public bool _ct;

    public bool _RLcheck;

    // Start is called before the first frame update

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _Player = transform.Find("Player_check").gameObject;
        _Player_check = _Player.GetComponent<Player_check>();
    }
    void Start()
    {
        _Est = 1;
        _hirumi = 0;
        _dtimer = 0;
        _Atimer = 0;
        _A2timer = 0;
        _A3timer = 0;
        _A4timer = 0;
        _A5timer = 0;
        _A6timer = 0;
        _hit = false;
        _E_bx.enabled = true;
        //�G�̗�
        _E_maxHp = 720;
        _E_Hpslider.value = 1;
        _E_def = 0;
        currentE_Hp = _E_maxHp;
        Death_particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Attack4.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Attackfirst.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Attackfinal.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        alart.SetActive(false);
        alart2.SetActive(false);
        alart3.SetActive(false);
        _Playercheckbx.enabled = true;
        Attack_check = GameObject.Find("Attack_check").GetComponent<Attack_check>();
        _Acheck = Attack_check._Acheck;
        _Acoll.enabled = false;
        _A2coll.enabled = false;
        _A3coll.enabled = false;
        _A4coll.enabled = false;
        _A5coll.enabled = false;
        _A6coll.enabled = false;
        _Atkcooldown = false;
        _Atkcooltimer = 0;
        _Turntimer = 0;
        TurntimerStop = false;
        _rdAtk = 0;
        _A2check = false;
        _ct = false;
        _RLcheck = false;
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        _vx = 0;
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        Vector3 scale = transform.localScale;

        if (Player_check._P_check == false)
        {
            _Turntimer += Time.deltaTime;
            if (_Turntimer >= 6.0f)
            {
                scale.x = 4;
                transform.localScale = scale;
                if (_Turntimer >= 12.0f)
                {
                    scale.x = -4;
                    transform.localScale = scale;
                    _Turntimer = 0;
                }
            }
        }
        if (Player_check._P_check == true && TurntimerStop == false)
        {
            _Turntimer = 0;
            TurntimerStop = true;
        }
    }
    public void reverse()
    {
        Vector3 scale = transform.localScale;
        Debug.Log("aaaaaaaaaaaaaaa");
        if (scale.x == -4)
        {
            scale.x = 4;
            transform.localScale = scale;
        }
        else if (scale.x == 4)
        {
            scale.x = -4;
            transform.localScale = scale;
        }


    }
    void FixedUpdate()
    {
        Player2Manager = GameObject.Find("Player").GetComponent<Player2Manager>();
        _Atk = Player2Manager._Atk;
        Debug.Log(_Atk + "�U����");
        Player_check = GameObject.Find("Player_check").GetComponent<Player_check>();
        _Player_check._P_check = Player_check._P_check;
        Debug.Log(_Player_check._P_check + "�v���C���[�`�F�b�N");
        Attack_check = GameObject.Find("Attack_check").GetComponent<Attack_check>();
        _Acheck = Attack_check._Acheck;
        Debug.Log(_Acheck + "�U������");

        if (_Est == 1)
        {
            _animator.Play("Idle");
        }
        if (currentE_Hp <= 0)
        {
            _Est = 2;
        }
        if (_Est == 2)
        {
            _dtimer = 0;
            _Atimer = 0;
            _Atkcooltimer = 0;
            _hirumi = 0;
            _rb.linearVelocity = Vector3.zero;
            _Atkcooldown = false;
            _animator.Play("Death");
            Attack_check._Acheck = false;
            _Acoll.enabled = false;
            _A2coll.enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezePosition;
            Attack_check._Atk_check.enabled = false;
            _E_bx.enabled = false;
            _timer += Time.deltaTime;
            if (_timer >= 1.0f)
            {
                Death_particle.Play(true);
            }
            if (_timer >= 2.0f)
            {
                clear3();
                Death_particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(_enemy);
            }
        }
        if (_hirumi >= 10)
        {
            _Atkcheck.enabled = false;
            Attack_check._Acheck = false;
            _Acoll.enabled = false;
            _A2coll.enabled = false;
            _A2check = false;
            _Atimer = 0;
            _A2timer = 0;
            _A3timer = 0;
            _A4timer = 0;
            _A5timer = 0;
            _A6timer = 0;
            _rdAtk = 0;
            alart.SetActive(false);
            alart2.SetActive(false);
            alart3.SetActive(false);
            _Est = 3;
            _animator.Play("Idle");
        }
        if (_Est == 3)
        {
            _dtimer += Time.deltaTime;
            if (_dtimer >= 0.6f)
            {
                _dtimer = 0;
                _Est = 7;
                _hirumi = 0;
                _Atkcheck.enabled = true;
            }
        }
        //�U��
        if (_Acheck == true)
        {
            _Est = 4;
            _Atimer += Time.deltaTime;
            if (_Est == 4)
            {
                _animator.Play("Attack1");
                if (_Atimer >= 0.42f)
                {
                    _Acoll.enabled = true;
                    if (_Atimer >= 0.56f)
                    {
                        _Acoll.enabled = false;
                        if (_Atimer >= 0.9f)
                        {
                            _Est = 7;
                            _Atimer = 0;
                            _rdAtk = Random.Range(0, 5);
                            _A2check = true;
                        }
                    }
                }
            }
        }
        if (_Est == 5)
        {
            _animator.Play("Attack2");
            _A2timer += Time.deltaTime;
            if (_A2timer >= 0.42f)
            {
                _A2coll.enabled = true;
                if (_A2timer >= 0.55f)
                {
                    _A2coll.enabled = false;
                    if (_A2timer >= 0.65f)
                    {
                        if (_A2timer >= 0.95f)
                        {
                            _Atkcooldown = true;
                            _Est = 7;
                            _A2timer = 0;
                            _rdAtk = Random.Range(1, 3);
                        }
                    }
                }
            }
        }
        if (_Est == 8)
        {
            _animator.Play("Attack3");
            _A3timer += Time.deltaTime;
            alart2.SetActive(true);
            if (_A3timer >= 1.2f)
            {
                _A3coll.enabled = true;
                _A4coll.enabled = true;
                Attack4.Play(true);
                if (_A3timer >= 1.3f)
                {
                    _A3coll.enabled = false;
                    alart2.SetActive(false);
                }
                if( _A3timer >= 1.7f)
                {
                    Attack4.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    _A4coll.enabled = false;
                }
                if (_A3timer >= 2f)
                {
                    if (_A3timer >= 2.0f)
                    {
                        _Atkcooldown = true;
                        _Est = 7;
                        _A3timer = 0;
                        _rdAtk = 0;
                    }
                }
            }
        }
        if (_Est == 9)
        {
            _animator.Play("Attack4");
            _A4timer += Time.deltaTime;
            alart.SetActive(true);
            _Atkcheck.enabled = false;
            if (_A4timer >= 1.3f)
            {
                _A5coll.enabled = true;
                Attackfirst.Play(true);
                if (_A4timer >= 2f)
                {
                    Attackfirst.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    _A5coll.enabled = false;
                    alart.SetActive(false);
                }
                if (_A4timer >= 2.3f)
                {  
                    if (_A4timer >= 2.4f)
                    {
                        _Est = 10;
                        _A4timer = 0;
                        _rdAtk = 0;
                    }
                }
            }
        }
        if (_Est == 10)
        {
            _animator.Play("Attack5");
            _A5timer += Time.deltaTime;
            alart3.SetActive(true);
            if (_A5timer >= 1.3f)
            {
                _A6coll.enabled = true;
                Attackfinal.Play(true);
                if (_A5timer >= 1.75f)
                {
                    Attackfinal.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    _A6coll.enabled = false;
                    alart3.SetActive(false);
                }
                if (_A5timer >= 2.3f)
                {
                    if (_A5timer >= 2.4f)
                    {
                        _Atkcooldown = true;
                        _Est = 7;
                        _A5timer = 0;
                        _rdAtk = 0;
                        _Atkcheck.enabled = true;
                    }
                }
            }
        }
        if (_Est == 11)
        {
            _animator.Play("Attack4");
            _A6timer += Time.deltaTime;
            alart.SetActive(true);
            _Atkcheck.enabled = false;
            if (_A6timer >= 1.3f)
            {
                _A5coll.enabled = true;
                Attackfirst.Play(true);
                if (_A6timer >= 2f)
                {
                    Attackfirst.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    _A5coll.enabled = false;
                    alart.SetActive(false);
                }
                if (_A6timer >= 2.3f)
                {
                    if (_A6timer >= 2.4f)
                    {
                        _Atkcooldown = true;
                        _Est = 7;
                        _A6timer = 0;
                        _rdAtk = 0;
                    }
                }
            }
        }

        if (_rdAtk == 1 && _A2check == true)
        {
            Attack_check._Acheck = false;
            _Est = 7;
            _Atkcooldown = true;
            _rdAtk = 0;
            _A2check = false;
        }
        if (_rdAtk == 3)
        {
            _Est = 5;
            Attack_check._Acheck = false;
            _A2check = false;
            _Atimer = 0;
        }
        if (_rdAtk == 2)
        {
            _Est = 8;
            Attack_check._Acheck = false;
            _A2check = false;
            _Atimer = 0;

        }
        if (_rdAtk == 4)
        {
            _Est = 11;
            Attack_check._Acheck = false;
            _A2check = false;
            _Atimer = 0;

        }

        if (_Atkcooldown == true)
        {
            _Atkcooltimer += Time.deltaTime;
            if (_Atkcooltimer >= 0.8f)
            {
                _Atkcooltimer = 0;
                _Atkcooldown = false;
                Attack_check._Atk_check.enabled = true;
            }
        }
        if (_Player_check._P_check == true && _ct == false)
        {
            _ct = true;
            _Est = 9;
        }
        if (_Est == 7)
        {
            _animator.Play("Run");
            // �v���C���[�Ɍ����Đi��
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTr.position.x, 0), speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, playerTr.position) <= 5.0f)
                return;
        }

    }
    public void clear3()
    {
        SceneManager.LoadScene("Stage3CLEAR");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player_Attack"))
        {
            _E_kekka = (_Atk - (_E_def / 5));
            currentE_Hp = currentE_Hp - _E_kekka;
            _E_Hpslider.value = (float)currentE_Hp / (float)_E_maxHp;
            _hirumi += _E_kekka / 3;
        }
        if (other.CompareTag("Player_Attack1"))
        {
            _E_kekka = ((_Atk * 3) - (_E_def / 5));
            currentE_Hp = currentE_Hp - _E_kekka;
            _E_Hpslider.value = (float)currentE_Hp / (float)_E_maxHp;
            _hirumi += _E_kekka / 5;
        }
    }
}
