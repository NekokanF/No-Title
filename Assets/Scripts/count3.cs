using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class count3 : MonoBehaviour
{
    public Player2Manager player;

    // Start is called before the first frame update

    void awake()
    {
        player = GetComponent<Player2Manager>();
    }
    void Start()
    {

        player.countupst3();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
