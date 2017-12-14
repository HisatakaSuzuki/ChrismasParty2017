using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.TextAnimation;

public class HowTo : MonoBehaviour {
	public GameObject paragraph;	//プレハブを入れる
	public static int paragraphNum = 10;

	GameObject[] paragraphObject;
	TextAnimation[] textAnimation;

	public static int shownParagraph;

	public string[] phrases = new string[paragraphNum]; //インスペクターから文字を入れておく

	// Use this for initialization
	void Start () {
		paragraphObject = new GameObject[paragraphNum];
		textAnimation = new TextAnimation[paragraphNum];
		for (int i = 0; i < paragraphObject.Length; i++) {
			paragraphObject [i] = Instantiate (paragraph, this.transform) as GameObject;
			textAnimation [i] = paragraphObject [i].GetComponent<TextAnimation> ();
		}
		shownParagraph = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.STATE.Howto) {
			if (Input.GetKeyDown (KeyCode.A)) {
				paragraphObject [shownParagraph].GetComponent<Text> ().text = phrases [shownParagraph];
				textAnimation [shownParagraph].PlayAnim (0);
				shownParagraph++;
			}
		} else if (GameManager.state == GameManager.STATE.PresentShow) {
			if (shownParagraph >= paragraphNum) {
				foreach (var obj in paragraphObject)
					Destroy (obj);
				shownParagraph = 0;	//もう使わないでしょう
			}
		}
	}
}
