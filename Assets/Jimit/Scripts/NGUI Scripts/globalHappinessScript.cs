using UnityEngine;
using System.Collections;

public class globalHappinessScript : MonoBehaviour {

    public BPlayer bplayerScript;
    float happniess = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        happniess = bplayerScript.GetGlobalAverageHappiness();
        GetComponent<UILabel>().text = ((int)happniess).ToString();
	}
}
