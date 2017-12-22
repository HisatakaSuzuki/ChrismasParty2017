using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OriginalCursor : MonoBehaviour {
	public Texture2D cursorImage;
	public GameObject snowPrefab;


	public Vector3 prev_mousePos;
	public GameObject canvas;

	public float snowInstantiateRate = 1.0f;
    public float meltTime = 1.0f;

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
			//snows [snowIndex++] = Instantiate (snowPrefab, prev_mousePos, Quaternion.identity,canvas.transform) as GameObject;
            Instantiate(snowPrefab, prev_mousePos, Quaternion.identity, canvas.transform);
            prev_mousePos = Input.mousePosition;
		}

        //foreach (var s in snows)
        //{
        //    if (s != null)
        //    {
        //        Color src = s.GetComponent<Image>().color;
        //        Color c = new Color(src.r, src.g, src.b, src.a - (meltTime * Time.deltaTime));
        //        s.GetComponent<Image>().color = c;
        //        if (s.GetComponent<Image>().color.a <= 0.0f)
        //        {
        //            Destroy(s);
        //        }
        //    }
        //}

        //foreach (var s in snows) {
        //    if (s != null) {
        //        if (s.GetComponent<RectTransform> ().position.y < -540) {
        //            Destroy (s);
        //        }
        //    }
        //}
	}
}
