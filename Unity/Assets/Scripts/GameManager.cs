using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GameManager : MonoBehaviour {
	public enum STATE
	{
		Setup,
		Howto,
		PresentShow,
		Wait,
		Lottery,
		End
	}

	public static STATE state = STATE.Setup;

	public AudioClip[] bgm;
	AudioSource audioSource;

	const int presentNum = 30;
	const int luckyPersonNum = 20;

	static int[,] luckyNumbers = new int[presentNum,luckyPersonNum];

	static int presentIndex = 0;
	static int luckyPresonIndex = -1;

	public static string LuckyNumber{
		get{
			return luckyNumbers [presentIndex, luckyPresonIndex].ToString();
		}
	}


	// Use this for initialization
	void Start () {
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
				state = STATE.Wait;
			}
		}
		else if (state == STATE.Wait)
		{//入力待機・プレゼントが渡ったかどうかの判定待ち
			if (Input.GetKeyDown(KeyCode.A))
			{
				luckyPresonIndex++;
				state = STATE.Lottery;
			}
			if (Input.GetKeyDown(KeyCode.S))
			{//プレゼントの当選者が表れた
				presentIndex++;
				luckyPresonIndex = -1;
				state = STATE.PresentShow;
			}
		}
		else if (state == STATE.Lottery)
		{
			GameManager.state = GameManager.STATE.Wait;
		}
		else if (state == STATE.End)
		{//ゲーム終了
			
		}

		//終了条件
		if (presentIndex >= presentNum)
		{
			GameManager.state = GameManager.STATE.End;
		}
	}
}
