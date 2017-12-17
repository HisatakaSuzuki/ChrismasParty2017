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

	public Vector3 originalPosition;
	public Vector3 movedPosition;

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
		if (GameManager.state == GameManager.STATE.PresentShow) {//プレゼントの紹介
			if (showFlag) {
				showFlag = false;
				Color prev = new Color (rawImage.color.r, rawImage.color.g, rawImage.color.b, 0.0f);
				rect.localPosition = originalPosition;
				rawImage.color = prev;
			}
			rawImage.texture = presentTexture [GameManager.presentIndex];
			rect = this.GetComponent<RectTransform> ();
			rect.sizeDelta = CalculateRectSize (presentTexture [GameManager.presentIndex].width, presentTexture [GameManager.presentIndex].height, max);
			if (!showFlag) {
				StartCoroutine (ShowImage ());
				showFlag = true;
			}

			if (Input.GetKeyDown (KeyCode.A)) {
				Debug.Log ("StartCoroutine");
				StartCoroutine (ImageMove ());
			}
		} else if (GameManager.state == GameManager.STATE.PresentShow) {
			if (Input.GetKeyDown (KeyCode.A)) {
				Debug.Log ("PrevData Change");

			}
		}
    }


	IEnumerator ImageMove(){
		Vector3 originScale = rect.localScale;
		Vector3 zero = new Vector3 (0.0f, 0.0f, 0.0f);

		for(;;){
			if (rect.localScale == zero)
				break;
			Vector3 updateScale = rect.localScale - new Vector3 (0.1f, 0.1f, 0.1f);
			rect.localScale = updateScale;
			yield return new WaitForSeconds (0.01f);
		}
			
		rect.localPosition = movedPosition;
		while (!rect.localScale.Equals(originScale)) {
			Vector3 updateScale = rect.localScale + new Vector3 (0.1f, 0.1f, 0.1f);
			rect.localScale = updateScale;
			Debug.Log ("back:" + rect.localScale);
			yield return new WaitForSeconds (0.01f);
		}	
	}

	IEnumerator ShowImage(){
		for (; ; ) {
			Color c = new Color (rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a + 0.01f);
			rawImage.color = c;
			if (rawImage.color.a >= 0.95f) {
				break;
			}
			yield return new WaitForSeconds (0.01f);
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
