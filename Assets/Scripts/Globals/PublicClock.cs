using UnityEngine;
using System.Collections;



/// <summary>
/// Maintains the Game Clock. Follows a singleton pattern with the variable clock being the instance.
/// Call PublicClock.clock.getTime() to get the current time in hours
/// Author : Vivek Kotecha
/// </summary>
public class PublicClock : MonoBehaviour {


	public static PublicClock clock;

	public float timeLapsePerFrame = 0.2f; // How much time passes every update()
	public int daysPassed = 0;

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
		//print (daysPassed);
		if(timeMin==PublicConstants.MINUTES_PER_DAY)
		{
			timeMin=0;
			daysPassed += 1;
		}
			
		timeMin += timeLapsePerFrame;
	}

	public float GetTime()
	{
		return Mathf.Floor(timeMin/60);
	}

	public int GetDays()
	{
		return daysPassed;
	}

	void OnGUI(){
		//GUI.Label(new Rect(10,10,100,20),GetTime().ToString());
	}
}
