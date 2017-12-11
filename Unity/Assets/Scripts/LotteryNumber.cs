using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LotteryNumber : MonoBehaviour{
	public bool isAtari;
	public string objectName;

	// Use this for initialization
	void Start () {
		isAtari = false;
		objectName = this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnSelect(){
		isAtari = true;
	}
}
