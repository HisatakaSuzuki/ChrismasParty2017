using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.IO;


public class LotterySystem : MonoBehaviour {
	public GameObject lotteryNumberObject;
	GameObject[] shownNumbers;
    Text lotteryText;
	Color textColor;

    public float sleepTime = 0.1f;
    public float seed = 1.0f;

	bool showFlag;

    private int lotteryCount = 0;

    // Use this for initialization
    void Start () {
		showFlag = false;
    }

	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.STATE.Wait) {
			if (!showFlag) {
				shownNumbers = new GameObject[8];
				for (int i = 0; i < shownNumbers.Length; i++) {
					shownNumbers [i] = Instantiate (lotteryNumberObject, this.transform) as GameObject;
					StartCoroutine(ShowNumber (i, 100 * i));
				}
				showFlag = true;
			}

            if (Input.GetKeyDown(KeyCode.R))
                lotteryCount++;
            StartCoroutine("Lottery");

        }
        else if (GameManager.state == GameManager.STATE.Lottery) {//抽選
            if (Input.GetKeyDown(KeyCode.R))
				StartCoroutine ("Lottery");
                lotteryCount++;
        }
        else if(GameManager.state == GameManager.STATE.PresentShow || GameManager.state == GameManager.STATE.End){
			showFlag = false;
		}
	}

	IEnumerator ShowNumber(int i, float distY){
		Text t = shownNumbers [i].GetComponent<Text> ();
		for (; ; ) {
			yield return new WaitForSeconds (0.01f);
			Color c = new Color (.0f, .0f, .0f, t.color.a + 0.01f);
			t.color = c;
			if (t.color.a >= 0.8f) {
				break;
			}
		}
	}

    IEnumerator Lottery()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(sleepTime);
            shownNumbers[0].GetComponent<Text>().text = GameManager.aList[lotteryCount].ToString();
            sleepTime = sleepTime + Time.deltaTime / seed;
            if (sleepTime > 0.5f)
            {
                sleepTime = 0.1f;
				//lotteryText.text = GameManager.LuckyNumber;
                break;
            }
        }
    }
}
