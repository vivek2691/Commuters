using UnityEngine;
using System.Collections;

public class PopUpMessageScript : MonoBehaviour {

    float timer = 0.0f;
    bool messageUp = false;
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 5.0f && !messageUp)
        {
            timer = 0.0f;
            GetComponent<UILabel>().text = " A NEW UPGRADE MESSAGE ";
            messageUp = true;
        }

        if (messageUp)
        {
            StartCoroutine("hideMessage");
        }
	}

    IEnumerator hideMessage()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<UILabel>().text = "";
        messageUp = false;
    }
}
