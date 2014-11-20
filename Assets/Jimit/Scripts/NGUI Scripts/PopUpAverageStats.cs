using UnityEngine;
using System.Collections;

public class PopUpAverageStats : MonoBehaviour
{
    public Camera camera;
    public Camera nguiCamera;
    public GameObject UI_AverageNeighborhoodStats;
    GameObject[] neighborhoods;
    
    void Start()
    {
        // at start we don't want to show the neighborhood average stats ui
        DisableAverageStatsUI();
    }

    // Update is called once per frame
    void Update()
    {
        //at every update we check for a mouse click and if so ray cast and see if its hit a neighborhood
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Ray rayNgui = nguiCamera.ScreenPointToRay(Input.mousePosition);
            CheckNeighborhoodHit(ray);
            CheckAverageStatCloseMark(rayNgui);
        }
    }

    //deactive ui average neighbourhood stats
    private void DisableAverageStatsUI()
    {
        UI_AverageNeighborhoodStats.gameObject.active = false;
    }

    //activate ui average neighborhood stats. Set the text of values of that neighboor on this UI
    private void ActivateAverageStatsUI(GameObject neighborhood)
    {
        UI_AverageNeighborhoodStats.gameObject.active = true;
    }

    //check if the neighborhood is clicked by mouse button down
    private void CheckNeighborhoodHit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500))
        {
            if (hit.collider.tag == "Neighbourhood")
            {
                ActivateAverageStatsUI(hit.collider.gameObject);
            }
        }
    }

    //close the average stat box if clicked on the close mark
    private void CheckAverageStatCloseMark(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500))
        {
            Debug.Log(" hitting ");
            if (hit.collider.tag == "closemark")
            {
                Debug.Log(" close mark hit ");
                DisableAverageStatsUI();
            }
        }
    }
}
