using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.IO;


public class LotterySystem : MonoBehaviour {
    Text lotteryText;
	Color textColor;

    public float sleepTime = 0.1f;
    public float seed = 1.0f;

	// Use this for initialization
	void Start () {
        lotteryText = this.gameObject.GetComponent<Text>();
		textColor = new Color (.0f,.0f,.0f,.0f);
		lotteryText.color = textColor;
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.STATE.Lottery) {//抽選
			textColor.a = 1.0f;
			lotteryText.color = textColor;
			StartCoroutine ("Lottery");
		} else if(GameManager.state == GameManager.STATE.PresentShow || GameManager.state == GameManager.STATE.End){
			textColor.a = 0.0f;
			lotteryText.color = textColor;
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
				lotteryText.text = GameManager.LuckyNumber;
                break;
            }
        }
    }
}
