using UnityEngine;
using System.Collections;

public class TimerLabelScript : MonoBehaviour {

    float timer = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    timer = (int)Time.timeSinceLevelLoad;
    GetComponent<UILabel>().text = timer.ToString();
	}
}
