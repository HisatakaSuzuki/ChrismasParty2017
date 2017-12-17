using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    List<string> comments = new List<string>();

    // Use this for initialization
    void Start () {
        string path = "";

        using (StreamReader sr = new StreamReader("presentinfo.csv"))
        {
            int presentCount = 0;
            while (!sr.EndOfStream)
            {
                var tmp = sr.ReadLine();
                string[] data = tmp.Split(',');
                Debug.Log(data[0]);
                Debug.Log(data[1]);
                Debug.Log(data[2]);
                presentTexture[presentCount] = ReadTexture(Application.dataPath+"/Textures/Presents/17F_Present/"  + data[0], 600, 600); //パスからtextureの読み込み
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
