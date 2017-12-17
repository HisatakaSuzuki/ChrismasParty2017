using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.IO;


public class LotterySystem : MonoBehaviour {
	public GameObject lotteryNumberObject;	//スロット用番号テキストのプレハブ

	GameObject[] showNumbers;	//実際に表示するスロットのテキストオブジェクト
    
	Text[] lotteryText;	//showNumberのTextコンポーネントを格納

    public float sleepTime = 0.1f;
    public float seed = 1.0f;

	bool showFlag;

	float[] endTimes = new float[8]{
		//0.3f,0.32f,0.34f,0.36f,0.38f,0.4f,0.42f,0.44f
        0.4f,0.48f,0.56f,0.64f,0.72f,0.8f,0.88f,0.96f
    };
    float restrictionTime = 0.0f;

    private int lotteryCount = 0;

    public AudioClip[] effect;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
		showFlag = false;
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        restrictionTime += Time.deltaTime;

        if (GameManager.state == GameManager.STATE.Lottery) {
            if (!showFlag) {
				int index = GameManager.luckyPersonNum [GameManager.presentIndex];
				if (index < 4) {
					this.gameObject.GetComponent<GridLayoutGroup> ().startAxis = GridLayoutGroup.Axis.Vertical;
				} else {
					this.gameObject.GetComponent<GridLayoutGroup> ().startAxis = GridLayoutGroup.Axis.Horizontal;
				}

				showNumbers = new GameObject[index];
				lotteryText = new Text[index];
                for (int i = 0; i < showNumbers.Length; i++) {
					showNumbers [i] = Instantiate (lotteryNumberObject, this.transform) as GameObject;
					lotteryText [i] = showNumbers [i].GetComponent<Text> ();
					//StartCoroutine(ShowNumber (i, 100 * i));
				}
				showFlag = true;
			}
            if (Input.GetKeyDown(KeyCode.R) && restrictionTime > 2.0f) {
                restrictionTime = 0.0f;
                bool allAtariFlag = true;
                for(int j = 0; j < showNumbers.Length; j++)
                {
                    if (!showNumbers[j].GetComponent<LotteryNumber>().isAtari)
                    {
                        allAtariFlag = false;
                    }
                }

                if (!allAtariFlag)
                {
                    audioSource.clip = effect[0];
                    audioSource.Play();
                }
                for (int i = 0; i < showNumbers.Length; i++) {
					StartCoroutine(Lottery(i,endTimes[i]));
				}
			}               
        }
        else if(GameManager.state == GameManager.STATE.PresentShow || GameManager.state == GameManager.STATE.End){
			if (showNumbers != null) {
				foreach (var s in showNumbers)
					Destroy (s);
			}
			showNumbers = null;
			showFlag = false;
		}
	}

	IEnumerator ShowNumber(int i, float distY){
		for (; ; ) {
			Color c = new Color (.0f, .0f, .0f, lotteryText[i].color.a + 0.01f);
			lotteryText[i].color = c;
			if (lotteryText[i].color.a >= 0.8f) {
				break;
			}
			yield return new WaitForSeconds (0.01f);
		}
	}

	IEnumerator Lottery(int i, float endtime)
    {
		if (!showNumbers [i].GetComponent<LotteryNumber> ().isAtari) {
			float time = 0.0f;
			for (; ; )
			{
				//showNumbers[i].GetComponent<Text>().text = GameManager.aList[lotteryCount].ToString();
				lotteryText[i].text = Random.Range(1,601).ToString();//あとで1~maxまでに変更する
				time += Time.deltaTime;
				if (time > endtime)
				{
					lotteryText[i].text = GameManager.winnersNumberList[lotteryCount].ToString();
                    audioSource.clip = effect[1];
                    audioSource.Play();
                    break;
				}
				yield return new WaitForSeconds(sleepTime);
			}
            lotteryCount++;
        }
    }
}
