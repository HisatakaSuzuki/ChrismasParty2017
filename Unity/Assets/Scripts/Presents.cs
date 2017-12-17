using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Presents : MonoBehaviour {
	public Texture2D[] presentTexture;

	RawImage rawImage;
	RectTransform rect;
	Color color;

	bool showFlag;
	float ratio;
	public float max = 600.0f;
	// Use this for initialization
	void Start () {
		rawImage = this.GetComponent<RawImage> ();
		rawImage.texture = presentTexture [0];
//		rect = this.GetComponent<RectTransform> ();
//		rect.sizeDelta = CalculateRectSize (presentTexture [0].width, presentTexture [0].height, max);
//		color = new Color (rawImage.color.r, rawImage.color.g, rawImage.color.b, .0f);
//		rawImage.color = color;
//
		showFlag = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
    	if (GameManager.state == GameManager.STATE.PresentShow)
        {//プレゼントの紹介
            rawImage.texture = presentTexture[GameManager.presentIndex];
            rect = this.GetComponent<RectTransform> ();
            rect.sizeDelta = CalculateRectSize (presentTexture [GameManager.presentIndex].width, presentTexture [GameManager.presentIndex].height, max);
			if (!showFlag) {
				StartCoroutine (ShowImage ());
				showFlag = true;
			}
        }
    }

	IEnumerator ShowImage(){
		for (; ; ) {
			yield return new WaitForSeconds (0.01f);
			Color c = new Color (rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a + 0.01f);
			rawImage.color = c;
			if (rawImage.color.a >= 0.95f) {
				break;
			}
		}
	}

    Vector2 CalculateRectSize(float w, float h, float m){
		Vector2 output;
		float ratio;

		if (w >= h) {
			ratio = h / w;
			output = new Vector2 (m, m * ratio);
		} else {
			ratio = w / h;
			output = new Vector2 (m * ratio, m);
		}

		return output;
	}
		
}
