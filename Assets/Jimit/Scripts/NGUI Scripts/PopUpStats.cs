using UnityEngine;
using System.Collections;

public class PopUpStats : MonoBehaviour
{
    public Camera camera;
    public Camera nguiCamera;
    public GameObject UI_AverageNeighborhoodStats;
    public GameObject UI_nAvgStatsBox;
    public GameObject UI_nPeopleStatsBox;
    public GameObject UI_PersonStats;
    GameObject[] neighborhoods;

    public enum NeighborhoodStatsTab { avgstats = 1, peoplestats };
    NeighborhoodStatsTab neighborhoodStatsTab;
    private GameObject currentNeighborhood = null;

    void Start()
    {
        neighborhoodStatsTab = NeighborhoodStatsTab.avgstats;
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
    public void ActivateAverageStatsUI(GameObject neighborhood)
    {
        UI_AverageNeighborhoodStats.gameObject.active = true;
        int TabNo = getAvgStatsCurrentTab();
        currentNeighborhood = neighborhood;
        ActivateAvgStatsTab(TabNo, neighborhood);
    }

    private void ActivateAvgStatsTab(int tabNo, GameObject neighborhood)
    {
        //activate average stats of neighborhood, 1st Tab
        if (tabNo == 1)
        {
            UI_nAvgStatsBox.gameObject.active = true;
            UI_nPeopleStatsBox.gameObject.active = false;
            //average happiness
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageHappiness().ToString();

            //for average wealth
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageWealth().ToString();

            //for average health
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageHealth().ToString();
        }

        //activate people stats of neighborhood, 2nd Tab
        else
        {
            UI_nAvgStatsBox.gameObject.active = false;
            UI_nPeopleStatsBox.gameObject.active = true;
        }
    }

    //on click event on neighborhood avg stats tab
    public void OnNAvgStatsTabClick()
    {
        setAvgStatsTab(1);
        int tabNo = getAvgStatsCurrentTab();
        if (currentNeighborhood != null)
        {
            ActivateAvgStatsTab(tabNo, currentNeighborhood);
        }
    }

    //on click event on person avg stats tab
    public void OnNPersonStatsTabClick()
    {
        setAvgStatsTab(2);
        int tabNo = getAvgStatsCurrentTab();
        if (currentNeighborhood != null)
        {
            ActivateAvgStatsTab(tabNo, currentNeighborhood);
        }
    }

    public void OnNAvgStatsPersonTabClick()
    {
        setAvgStatsTab(2);
        int tabNo = getAvgStatsCurrentTab();
        if (currentNeighborhood != null)
        {
            ActivateAvgStatsTab(tabNo, currentNeighborhood);
        }
    }

    private void ActivatePersonStatsUI(GameObject person)
    {
        UI_PersonStats.gameObject.active = true;

        //set all person stats values here
        UI_PersonStats.gameObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().GetHappiness().ToString();

        //for average wealth
        UI_PersonStats.gameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().GetWealth().ToString();

        //for average health
        UI_PersonStats.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().GetHealth().ToString();
    }


    //check if the neighborhood is clicked by mouse button down
    private void CheckNeighborhoodHit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500))
        {
            if (hit.collider.tag == "Neighbourhood")
            {
                //send message to avgStatsButtonScript so that it has the neighborhood information
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
                setAvgStatsTab(1);
            }
            else if (hit.collider.tag == "closemarkpersonstats")
            {
                DisablePersonStatsUI();
            }
        }
    }

    //------------------------------- helper functions ---------------------------------
    public void setAvgStatsTab(int tabNo)
    {
        if (tabNo == 1)
            neighborhoodStatsTab = NeighborhoodStatsTab.avgstats;
        if( tabNo == 2)
            neighborhoodStatsTab = NeighborhoodStatsTab.peoplestats;
    }

    public int getAvgStatsCurrentTab()
    {
        return (int)neighborhoodStatsTab;
    }

}
