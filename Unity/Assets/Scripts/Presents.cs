using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Presents : MonoBehaviour {
	public Texture2D[] presentTexture;

	RawImage rawImage;
	Color color;

	bool alphaFlag;

	// Use this for initialization
	void Start () {
		rawImage = this.GetComponent<RawImage> ();
		rawImage.texture = presentTexture [0];
		color = new Color (rawImage.color.r, rawImage.color.g, rawImage.color.b, .0f);
		rawImage.color = color;

		alphaFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.STATE.PresentShow) {
			if (!alphaFlag) {
				color.a = 1.0f;
				rawImage.color = color;
				alphaFlag = true;
			}

		} else if (GameManager.state == GameManager.STATE.Wait) {
			if (alphaFlag) {
				color.a = 0.0f;
				rawImage.color = color;
				alphaFlag = false;
			}
		}
	}
		
}
