using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LotteryNumber : MonoBehaviour{
	public bool isAtari;
	//public string objectName;

	public Color non_selectedColor;
	public Color selectedColor;

	bool prev_isAtari;

	Text text;

	public float waitTime = 0.01f;
	float restrictionTime = 0.0f;

	// Use this for initialization
	void Start () {
		isAtari = false;
		prev_isAtari = isAtari;
		text = this.gameObject.GetComponent<Text> ();
	//	objectName = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		restrictionTime += Time.deltaTime;
		if (prev_isAtari != isAtari) {
			prev_isAtari = isAtari;
			if (isAtari)
				StartCoroutine (ChangeColor (selectedColor));
			else
				StartCoroutine (ChangeColor (non_selectedColor));
		}
	}

	IEnumerator ChangeColor(Color c){
		for (;;) {
			yield return new WaitForSeconds (waitTime);
			Color src = text.color;
			Color result = Color.Lerp (src, c, Mathf.PingPong (Time.time, 1));
			text.color = result;
			Vector4 resultV = result;
			Vector4 cV = c;
			if (Vector4.Distance (resultV, cV) == 0) {
				Debug.Log ("Finish");
				break;
			}
		}
	}

	public void OnSelect(){
		if (restrictionTime > 1.0f) {
			isAtari = !isAtari;
			restrictionTime = 0.0f;
		}
	}
}
