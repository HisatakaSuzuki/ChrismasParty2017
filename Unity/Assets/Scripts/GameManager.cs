using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {
	public enum STATE
	{
		Setup,
		Howto,
		PresentShow,
		Lottery,
		End
	}

	public static STATE state = STATE.Setup;

	public AudioClip[] bgm;
	AudioSource audioSource;
    int bgmNum = 0;

    public float snapshotMove;

    public GameObject titleEnd;
    public string endComment;

	static public int presentNum = 30;

	static public int presentIndex = 0;
	static int luckyPresonIndex = -1;
	public Text test;
    static public int max = 0;
    static public int[] winnersNumberList;

    static public List<int> luckyPersonNum = new List<int>();

	public GameObject lotterySystem;

    public AudioMixerSnapshot startEndSnapshot;
    public AudioMixerSnapshot playingSnapshot;

    // Use this for initialization
    void Awake () {
        //当選番号配列の準備

        using (StreamReader sr = new StreamReader("config.txt"))
		{
            int count = 0;
            while (!sr.EndOfStream)
            {
                var tmp = sr.ReadLine();
                if (count == 0)
                {
                    max = int.Parse(tmp);
                }
                else presentNum = int.Parse(tmp);
                count++;
            }
		}
 
        winnersNumberList = new int[max];
        for (int i = 0; i < max; i++)
        {
            winnersNumberList[i] = i+1;
        }

        //システム時間をintで取得
        int seed = DateTime.Now.Hour * 60 * 60 * 1000 + DateTime.Now.Minute * 60 * 1000 +
    DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
        int temp = 0;

        System.Random _random = new System.Random(seed);

        for (int i = 0; i < max; i++)
        {
            int r = i + (int)(_random.NextDouble() * (max - i));
            temp = winnersNumberList[r];
            winnersNumberList[r] = winnersNumberList[i];
            winnersNumberList[i] = temp;
			test.text += winnersNumberList [i].ToString () + ",";
        }


        /* デバッグ */
        //for (int i = 0; i < max; i++)
        //{
        //    Debug.Log(winnersNumberList[i]);
        //}

        GameManager.state = GameManager.STATE.Howto;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.clip = bgm[bgmNum];
        audioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if ((audioSource.time + Time.deltaTime) > audioSource.clip.length)
        {
            if (bgmNum >= 30)
            {
                bgmNum = 0;
            }
            else
            {
                bgmNum++;
            }
            audioSource.clip = bgm[bgmNum];
            audioSource.Play();
        }
        
		if (state == STATE.Howto)
		{//ゲームの説明
            if(Input.GetKeyDown(KeyCode.A))
                playingSnapshot.TransitionTo(snapshotMove);

			if (HowTo.shownParagraph > HowTo.paragraphNum)
				state = STATE.PresentShow;
		}
		else if (state == STATE.PresentShow)
		{//プレゼントの紹介
			if(Input.GetKeyDown(KeyCode.A))
			{
				//ここでプレゼントアニメーション
				state = STATE.Lottery;
			}
		}
		else if (state == STATE.Lottery)
		{//入力待機・プレゼントが渡ったかどうかの判定待ち
			LotteryNumber[] tmp = lotterySystem.GetComponentsInChildren<LotteryNumber>();
			bool next = false;
			for (int i = 0; i < tmp.Length; i++) {
				next = tmp [i].isAtari;
				if (!tmp [i].isAtari)	break;
			}

			if (Input.GetKeyDown(KeyCode.A) && next)
			{
                using (StreamWriter sw = new StreamWriter("result.txt",true))
                {
                    sw.WriteLine("presentIndex:"+(presentIndex+1));
                    for (int i = 0; i < tmp.Length; i++)
                        sw.Write(tmp[i].text.text + ",");
                    sw.WriteLine("");
                }
                presentIndex++;
                state = STATE.PresentShow;
			}
		}
		else if (state == STATE.End)
		{//ゲーム終了
            startEndSnapshot.TransitionTo(snapshotMove);
            titleEnd.GetComponent<Text>().text = endComment;
        }

        //終了条件
        if (presentIndex >= presentNum)
		{
			GameManager.state = GameManager.STATE.End;
		}
	}


}
