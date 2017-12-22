using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.TextAnimation;

public class HowTo : MonoBehaviour {
	public GameObject paragraph;	//プレハブを入れる
	public static int paragraphNum = 13;

	GameObject[] paragraphObject;
	TextAnimation[] textAnimation;

    int rules = 8;
    int prohabits = 5;

	public static int shownParagraph;
	public GameObject title;
	public string[] phrases = new string[paragraphNum];
	bool isTitle = true;

	// Use this for initialization
	void Start () {
		paragraphObject = new GameObject[paragraphNum];
		textAnimation = new TextAnimation[paragraphNum];
		for (int i = 0; i < rules; i++) {
			paragraphObject [i] = Instantiate (paragraph, this.transform) as GameObject;
			textAnimation [i] = paragraphObject [i].GetComponent<TextAnimation> ();
		}
		shownParagraph = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.STATE.Howto) {

			if (Input.GetKeyDown (KeyCode.A)) {
                if (paragraphObject[7] != null && paragraphObject[7].GetComponent<Text>().text != "")
                {
                    foreach (var t in paragraphObject)
                    {
                        Destroy(t);
                    }
                    for (int i = 0; i < prohabits; i++)
                    {
                        paragraphObject[i+8] = Instantiate(paragraph, this.transform) as GameObject;
                        textAnimation[i+8] = paragraphObject[i+8].GetComponent<TextAnimation>();
                    }
                }
				if (isTitle) {
					//Destroy (title);
                    title.GetComponent<Text>().text = "";
					isTitle = false;
					paragraphObject [shownParagraph].GetComponent<Text> ().text = phrases [shownParagraph];
					textAnimation [shownParagraph].PlayAnim (0);
					shownParagraph++;
				} 
				else {
					if (shownParagraph < paragraphNum) {
                        paragraphObject[shownParagraph].GetComponent<Text>().text = phrases[shownParagraph];
                        textAnimation[shownParagraph].PlayAnim(0);
					}
					shownParagraph++;
				}
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
