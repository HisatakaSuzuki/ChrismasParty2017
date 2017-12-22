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

	public Color hoverColor;


	bool prev_isAtari;

	public Text text;
	RectTransform rectTransform;
	Outline textOutline;

	public float waitTime = 0.01f;
	float restrictionTime = 0.0f;

	bool isMouseEnter;
	bool isHoverAnimation;

	Coroutine coroutine;

	// Use this for initialization
	void Start () {
		isAtari = false;
		prev_isAtari = isAtari;
		text = this.gameObject.GetComponent<Text> ();
		isMouseEnter = false;
		rectTransform = this.gameObject.GetComponent<RectTransform> ();
		textOutline = this.gameObject.GetComponent<Outline> ();
		isHoverAnimation = false;
	//	objectName = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Input.mousePosition + "," + rectTransform.position+ "::"+isMouseOver());

		//HoverAnimation (isMouseOver);
		restrictionTime += Time.deltaTime;
		if (prev_isAtari != isAtari) {
			prev_isAtari = isAtari;
			if (isAtari)
				StartCoroutine (ChangeColor (selectedColor));
			else
				StartCoroutine (ChangeColor (non_selectedColor));
		}

		isMouseEnter = isMouseOver ();

		if (isMouseEnter && rectTransform.localScale.x == 1.0f) {
			if (coroutine != null) {
				Debug.Log (coroutine);
				StopCoroutine (coroutine);
			}
			Debug.Log ("MouseEnter");
			coroutine = StartCoroutine (HoverAnimation (true,hoverColor));
		}
		if (!isMouseEnter && rectTransform.localScale.x == 1.15f) {
			if (coroutine != null) {
				Debug.Log (coroutine);
				StopCoroutine (coroutine);
			}
				
			Debug.Log ("MouseExit");
			coroutine = StartCoroutine (HoverAnimation (false,text.color));
		}

		SelectNumber ();
	}

	bool isMouseOver(){
		float minx = rectTransform.position.x - 150;
		float maxx = rectTransform.position.x + 150;
		float miny = rectTransform.position.y - 100;
		float maxy = rectTransform.position.y + 100;
		Vector3 mousePos = Input.mousePosition;

		if (minx <= mousePos.x && miny <= mousePos.y && maxx >= mousePos.x && maxy >= mousePos.y) {
			return true;
		} else {
			return false;
		}
	}

	void SelectNumber(){
		if (Input.GetMouseButtonUp (0)) {
			if (isMouseEnter) {
				Debug.Log ("Is Selected");
				if (restrictionTime > 1.5f) {
					isAtari = !isAtari;
					restrictionTime = 0.0f;
				}
			}
		}
	}


	IEnumerator ChangeColor(Color c){
		for (;;) {
			Color src = text.color;
			Color result = Color.Lerp (src, c, Mathf.PingPong (Time.time, 1));
			text.color = result;
			textOutline.effectColor = result;
			if (Vector4.Distance (result, c) == 0) {
				break;
			}
			yield return new WaitForSeconds (waitTime);
		}
	}

	IEnumerator HoverAnimation(bool isEnter, Color outline){
		for (;;) {
			Vector3 scale = rectTransform.localScale;
			float dist = (isEnter) ? 1.15f : 1.0f;
			Vector3 updateScale = new Vector3 (Mathf.Lerp (scale.x, dist, Mathf.PingPong (Time.time, 1)),
				Mathf.Lerp (scale.y, dist, Mathf.PingPong (Time.time, 1)),
				Mathf.Lerp (scale.z, dist, Mathf.PingPong (Time.time, 1)));
			rectTransform.localScale = updateScale;	
			Vector2 worldToScreenPosition = Camera.main.WorldToScreenPoint (rectTransform.localPosition);

			Color src = textOutline.effectColor;
			Color result = Color.Lerp(src,outline,Mathf.PingPong (Time.time, 1));
			textOutline.effectColor = result;

			if (Vector3.Distance (updateScale, scale) == 0 && Vector4.Distance(result,outline) == 0) {
				break;
			}
			yield return new WaitForSeconds (waitTime);
		}
	}


}
