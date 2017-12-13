using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

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

	const int presentNum = 30;

	static public int presentIndex = 0;
	static int luckyPresonIndex = -1;

    static public int max = 0;
    static public int[] winnersNumberList;
    static public int[] luckyPersonNum = { 8,8,8,8,8,1,1,1,2,1,1,
                                    1,1,1,1,1,1,1,1,1,
                                    1,1,1,1,1,1,1,1,1,1,1
                                  };

    // Use this for initialization
    void Start () {
		//当選番号配列の準備
		using (StreamReader sr = new StreamReader("num.txt"))
		{
			while (!sr.EndOfStream)
			{
				var tmp = sr.ReadLine();
					max = int.Parse(tmp);
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
        }

        /* デバッグ */
        //for (int i = 0; i < max; i++)
        //{
        //    Debug.Log(winnersNumberList[i]);
        //}

        GameManager.state = GameManager.STATE.Howto;
		//audioSource = this.gameObject.GetComponent<AudioSource>();
		//audioSource.clip = bgm[0];
		//audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == STATE.Howto)
		{//ゲームの説明
			state = STATE.PresentShow;
		}
		else if (state == STATE.PresentShow)
		{//プレゼントの紹介
			if(Input.GetKeyDown(KeyCode.A))
			{
				state = STATE.Lottery;
			}
		}
		else if (state == STATE.Lottery)
		{//入力待機・プレゼントが渡ったかどうかの判定待ち
			if (Input.GetKeyDown(KeyCode.A))
			{
                presentIndex++;
                state = STATE.PresentShow;
			}
		}
		else if (state == STATE.End)
		{//ゲーム終了
            if (Input.GetKeyDown(KeyCode.A))
            GameManager.state = GameManager.STATE.End;
        }

        //終了条件
        if (presentIndex >= presentNum)
		{
			GameManager.state = GameManager.STATE.End;
		}
	}
}
