using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snow : MonoBehaviour {

    public Image snow;
    public float meltSpeed = 1.0f;
    public GameObject snowObject;
    public RectTransform snowTransform;

    static float wind = 0.0f;
    static float windTimer = 0.0f;

    public float resetTime = 30.0f;

	// Use this for initialization
	void Start () {
        snowObject = this.gameObject;
        snow = this.GetComponent<Image>();
        snowTransform = this.GetComponent<RectTransform>();
        if (wind == 0.0f)
        {
            wind = Random.Range(-1.0f, 1.0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        windTimer += Time.deltaTime;
        Color snowColor = snow.color;
        Color c = new Color(snowColor.r, snowColor.g, snowColor.b, snowColor.a - (meltSpeed*Time.deltaTime));
        snowObject.GetComponent<Image>().color = c;

        Vector3 scale = snowTransform.localScale;
        Vector3 s = new Vector3(scale.x - (meltSpeed * Time.deltaTime), scale.y - (meltSpeed * Time.deltaTime), scale.z - (meltSpeed * Time.deltaTime));
        snowObject.GetComponent<RectTransform>().localScale = s;

        if(windTimer > resetTime){
            windTimer = .0f;
            wind = Random.Range(-1.0f, 1.0f);
        }

        Vector3 position = snowTransform.position;
        Vector3 p = new Vector3(position.x + wind, position.y - Mathf.Abs(wind), position.z);
        snowObject.GetComponent<RectTransform>().position = p;

        if (snow.color.a <= 0.0f)
        {
            Destroy(snowObject);
        }
    }
}
