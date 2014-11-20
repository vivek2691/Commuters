using UnityEngine;
using System.Collections;

public class PopUpAverageStats : MonoBehaviour
{
    public Camera camera;
    public Camera nguiCamera;
    public GameObject UI_AverageNeighborhoodStats;
    public GameObject UI_PersonStats;
    GameObject[] neighborhoods;
    
    void Start()
    {
        // at start we don't want to show the neighborhood average stats ui
        DisableAverageStatsUI();
        DisablePersonStatsUI();
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
            CheckPersonHit(ray);
            CheckAverageStatCloseMark(rayNgui);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(2).GetChild(0).GetComponent<UILabel>().text = "65";
        }
    }

    //deactive ui average neighbourhood stats
    private void DisableAverageStatsUI()
    {
        UI_AverageNeighborhoodStats.gameObject.active = false;
    }

    private void DisablePersonStatsUI()
    {
        UI_PersonStats.gameObject.active = false;
    }

    //activate ui average neighborhood stats. Set the text of values of the clicked neighboor on this UI
    private void ActivateAverageStatsUI(GameObject neighborhood)
    {
        UI_AverageNeighborhoodStats.gameObject.active = true;

        //set all the text values here for eg to set the happiness factor
        // --> UI_AverageNeighborhoodStats.gameObject.transform.GetChild(2).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetHappinessFactor().ToString();
        UI_AverageNeighborhoodStats.gameObject.transform.GetChild(2).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageHappiness().ToString();

        //for average wealth
        UI_AverageNeighborhoodStats.gameObject.transform.GetChild(3).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageWealth().ToString();

        //for average health
        UI_AverageNeighborhoodStats.gameObject.transform.GetChild(4).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageHealth().ToString();
    }

    private void ActivatePersonStatsUI(GameObject person)
    {
        UI_PersonStats.gameObject.active = true;

        //set all person stats values here
        UI_PersonStats.gameObject.transform.GetChild(2).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().GetHappiness().ToString();

        //for average wealth
        UI_PersonStats.gameObject.transform.GetChild(3).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().GetWealth().ToString();

        //for average health
        UI_PersonStats.gameObject.transform.GetChild(4).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().GetHealth().ToString();
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

    //check if the person obj is clicked  to display person avg stats
    private void CheckPersonHit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500))
        {
            if (hit.collider.tag == "Person")
            {
                ActivatePersonStatsUI(hit.collider.gameObject);
            }
        }
    }

    //close the average stat box if clicked on the close mark
    private void CheckAverageStatCloseMark(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500))
        {
            if (hit.collider.tag == "closemarkavgstats")
            {
                DisableAverageStatsUI();
            }
            else if (hit.collider.tag == "closemarkpersonstats")
            {
                DisablePersonStatsUI();
            }
        }
    }
}
