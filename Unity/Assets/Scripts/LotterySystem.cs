using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.IO;


public class LotterySystem : MonoBehaviour {
    Text lotteryText;
    public float sleepTime = 0.1f;
    public float seed = 1.0f;

    const int presentNum = 30;
    const int luckyPersonNum = 20;
    
    public int[,] luckyNumbers = new int[presentNum,luckyPersonNum];

    public int presentIndex = 0;
    public int luckyPresonIndex = 0;

    public Texture[] presentTexture;

    public AudioClip[] bgm;
    AudioSource audioSource;

    enum STATE
    {
        Setup,
        Howto,
        PresentShow,
        Wait,
        Lottery,
        End
    }

    STATE state = STATE.Setup;

	// Use this for initialization
	void Start () {
        lotteryText = this.gameObject.GetComponent<Text>();
        audioSource = this.gameObject.GetComponent<AudioSource>();

        //当選番号配列の準備
        using (StreamReader sr = new StreamReader("test.csv"))
        {
            int i = 0;
            while (!sr.EndOfStream)
            {
                var tmp = sr.ReadLine().Split(',');
                for (int j = 0; j < luckyPersonNum; j++)
                {
                    luckyNumbers[i, j] = int.Parse(tmp[j]);
                }
                i++;
            }
        }

        state = STATE.Howto;
        audioSource.clip = bgm[0];
        audioSource.Play();
	}

	// Update is called once per frame
	void Update () {
        if (state == STATE.Howto)
        {//ゲームの説明
            state = STATE.PresentShow;
            lotteryText.text = "How To";
        }
        else if (state == STATE.PresentShow)
        {//プレゼントの紹介
            lotteryText.text = "Present Introduce";
            if(Input.GetKeyDown(KeyCode.A))
            {
                state = STATE.Wait;
            }
        }
        else if (state == STATE.Wait)
        {//入力待機・プレゼントが渡ったかどうかの判定待ち
            if (Input.GetKeyDown(KeyCode.A))
            {
                state = STATE.Lottery;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {//プレゼントの当選者が表れた
                presentIndex++;
                luckyPresonIndex = 0;
                state = STATE.PresentShow;
            }
        }
        else if (state == STATE.Lottery)
        {//抽選
            StartCoroutine("Lottery");
            state = STATE.Wait;
        }
        else if (state == STATE.End)
        {//ゲーム終了
            lotteryText.text = "End";
        }
        
        //終了条件
        if (presentIndex >= presentNum)
        {
            state = STATE.End;
        }
	}

    IEnumerator Lottery()
    {
        for (; ; )
        {   
            yield return new WaitForSeconds(sleepTime);
            lotteryText.text = Random.Range(1, 601).ToString();
            sleepTime = sleepTime + Time.deltaTime / seed;
            if (sleepTime > 0.5f)
            {
                sleepTime = 0.1f;
                lotteryText.text = luckyNumbers[presentIndex, luckyPresonIndex].ToString();
                luckyPresonIndex++;
                break;
            }
        }
    }
}
