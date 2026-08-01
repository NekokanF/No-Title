using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class alphaMain_main : MonoBehaviour
{
    [SerializeField]
    Text scoreText;

    [SerializeField]
    Text timerText;

    public int score;

    [SerializeField]
    float timer;

    [SerializeField]
    alphaPlayer_main playerMain;
    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "TIME:" + timer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMain.goalFlag)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                timerText.text = "TIME:" + timer.ToString("f0");
            }
            else
            {
                timerText.text = "TIME:0";
                Invoke("reloadFunc", 0.5f);
            }
        }
    }

    public void reloadBt()
    {
        SceneManager.LoadScene("alpahSample");
    }

    public void reloadFunc()
    {
        SceneManager.LoadScene("alpahSample");
    }
}
