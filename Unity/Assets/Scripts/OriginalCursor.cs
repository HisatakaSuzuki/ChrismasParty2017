using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OriginalCursor : MonoBehaviour {
	public Texture2D cursorImage;
	public GameObject snowPrefab;


	Vector3 prev_mousePos;
	public GameObject canvas;

	public float snowInstantiateRate = 10.0f;

	GameObject[] snows = new GameObject[100];
	int snowIndex = 0;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor (cursorImage, new Vector2 (cursorImage.width / 2, cursorImage.height / 2), CursorMode.Auto);
		prev_mousePos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (snowIndex >= snows.Length)
			snowIndex = 0;

		if (Vector3.Distance (prev_mousePos, Input.mousePosition) >= snowInstantiateRate) {
			if (snows [snowIndex] != null) {
				Destroy(snows[snowIndex]);
			}
			snows [snowIndex++] = Instantiate (snowPrefab, prev_mousePos, Quaternion.identity,canvas.transform) as GameObject;
		}
		prev_mousePos = Input.mousePosition;

		foreach (var s in snows) {
			if (s != null) {
				if (s.GetComponent<RectTransform> ().position.y < -540) {
					Destroy (s);
				}
			}
		}
	}
}
