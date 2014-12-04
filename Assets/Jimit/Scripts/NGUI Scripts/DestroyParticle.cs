using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Destroy(gameObject, 0.5f);
	}
}
