using UnityEngine;
using System.Collections;
using AssemblyCSharp;


/// <summary>
/// Maintains the Game Clock. Follows a singleton pattern with the variable clock being the instance.
/// Call PublicClock.clock.getTime() to get the current time in hours
/// Author : Vivek Kotecha
/// </summary>
public class PublicClock : MonoBehaviour {


	public static PublicClock clock;

	public float timeLapsePerFrame = 0.2f; // How much time passes every update()


	float timeMin = 0;	

	// Use this for initialization
	void Start () {
		if(clock==null){
			clock = this;
		}
		else{
			GameObject.Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(timeMin==AssemblyCSharp.PublicConstants.MINUTES_PER_DAY)
			timeMin=0;
		timeMin += timeLapsePerFrame;
	}

	public float getTime()
	{
		return Mathf.Floor(timeMin/60);
	}

	void OnGUI(){
		GUI.Label(new Rect(10,10,100,20),getTime().ToString());
	}
}
