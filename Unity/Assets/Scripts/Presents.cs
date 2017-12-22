using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Presents : MonoBehaviour {
	public Texture2D[] presentTexture;
    public GameObject commentObj; //Comment用のオブジェクト

    RawImage rawImage;
	RectTransform rect;
	Color color;

	bool showFlag;
	float ratio;
	public float max = 600.0f;
    public Vector3 originalPosition;
    public Vector3 movedPosition;

    List<string> comments = new List<string>();

    // Use this for initialization
    void Start()
    {
        presentTexture = new Texture2D[GameManager.presentNum];
        using (StreamReader sr = new StreamReader("presentinfo.csv"))
        {
            int presentCount = 0;
            while (!sr.EndOfStream)
            {
                var tmp = sr.ReadLine();
                string[] data = tmp.Split(',');
                presentTexture[presentCount] = ReadTexture(Application.dataPath + "/Textures/Presents/" + data[0], 600, 600); //パスからtextureの読み込み
                GameManager.luckyPersonNum.Add(int.Parse(data[1])); //貰える人数
                comments.Add(data[2]); //コメント
                presentCount++;
            }
        }

        rawImage = this.GetComponent<RawImage>();
        rawImage.texture = presentTexture[0];

        showFlag = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.state == GameManager.STATE.Howto)
        {
            commentObj.GetComponent<Text>().text = "";
        }

		if (GameManager.state == GameManager.STATE.PresentShow) {//プレゼントの紹介
			if (showFlag) {
				showFlag = false;
				Color prev = new Color (rawImage.color.r, rawImage.color.g, rawImage.color.b, 0.0f);
				rect.localPosition = originalPosition;
				rawImage.color = prev;
			}
            commentObj.GetComponent<Text>().text = comments[GameManager.presentIndex];
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
        else if (GameManager.state == GameManager.STATE.End)
        {
            Destroy(rawImage);
            Destroy(commentObj);
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

    byte[] ReadPngFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    Texture2D ReadTexture(string path, int width, int height)
    {
        byte[] readBinary = ReadPngFile(path);

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readBinary);

        return texture;
    }

}
