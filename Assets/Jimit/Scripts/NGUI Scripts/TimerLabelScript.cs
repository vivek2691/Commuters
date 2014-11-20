using UnityEngine;
using System.Collections;

public class TimerLabelScript : MonoBehaviour {

    float timer = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer = PublicClock.clock.GetTime();
       GetComponent<UILabel>().text = timer.ToString();
	}
}
