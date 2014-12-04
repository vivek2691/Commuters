using UnityEngine;
using System.Collections;

public class PopUpStats : MonoBehaviour
{
    public Camera camera;
    public Camera nguiCamera;

    //for neighborhood
    public GameObject UI_AverageNeighborhoodStats;
    public GameObject UI_nAvgStatsBox;
    public GameObject UI_nPeopleStatsBox;
    public GameObject UI_nUpgradesBox;
    public BPlayer BPlayerScript;
    public GameObject UI_GlobalParameters;
    public GameObject UI_HoverLabel;
    public GameObject UI_PopMessage;

    //for person
    public GameObject UI_PersonStats;

    //for vertex and edges
    public GameObject VertexUpgradesBox;
    public GameObject EdgeUpgradesBox;
    GameObject[] neighborhoods;

    public enum NeighborhoodStatsTab { avgstats = 1, peoplestats, upgrades };
    public enum MouseClickStats { firstClick = 1, secondClick };
    public enum VertexUpgradeType { BusStation = 1, RailStation, BikeShop, CarShop };
    public enum EdgeUpgradeType { unImproved = 1, FootPath, BikeTrail, SpeedRoad, SpeedBoulevard, SpeedRail };
    NeighborhoodStatsTab neighborhoodStatsTab;
    MouseClickStats mouseClickStats;
    VertexUpgradeType vertexUpgradeType;
    EdgeUpgradeType edgeUpgradeType;
    private GameObject currentNeighborhood = null;

    //booleans for vertex upgrades
    private bool isBusUpgraded = false;
    private bool isRailUpgrade = false;
    private bool isCarUpgraded = false;
    private bool isBikeUpgraded = false;
    private bool isVertexUpgradeClicked = false;

    //booleans for edge upgrades
    private bool isFoothPath = false;
    private bool isBikeTrail = false;
    private bool isSpeedRoad = false;
    private bool isBoulevard = false;
    private bool isSpeedRail = false;
    private bool isEdgeUpgradeClicked = false;

    //floats
    private float waitTime = 1.0f;
    private float currTime;
    private bool startPopUpMessageTimer = false;
    private float popUpMessageTimer;

    //money
    private int money;

    //particle prefab
    public GameObject partcile;

    //side materials
    public Material DirtRoadMaterial;
    public Material FootpathMat;
    public Material BoulevardMat;
    public Material SpeedRoadMat;
    public Material SideBikeLaneMat;
    public Material SideRailwayMat;

    //floats
    public float moneyUpgradeTimer;

    //booleans
    private bool isNeighborHoodBoxOpen = false;
    public GameObject currentActiveNeighborhood;

    void Start()
    {
        neighborhoodStatsTab = NeighborhoodStatsTab.avgstats;
        mouseClickStats = MouseClickStats.firstClick;
        vertexUpgradeType = VertexUpgradeType.BusStation;
        edgeUpgradeType = EdgeUpgradeType.unImproved;
        // at start we don't want to show the neighborhood average stats ui
        DisableAverageStatsUI();
        DisablePersonStatsUI();
        DeactivateInitialVertexUpgrades();
        DeactivateInitialEdgeUpgrades();
        DisableVertexUpgradesBox();
        DisableEdgeUpgradesBox();
    }

    // Update is called once per frame
    void Update()
    {
        //at every update we check for a mouse click and if so ray cast and see if its hit a neighborhood
        if (Input.GetMouseButtonDown(0) && !isVertexUpgradeClicked)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Ray rayNgui = nguiCamera.ScreenPointToRay(Input.mousePosition);
            CheckNeighborhoodHit(ray);
            CheckPersonHit(ray);
            CheckAverageStatCloseMark(rayNgui);
        }

        CheckMouseInput();

        //money upgrade
        moneyUpgradeTimer += Time.deltaTime;
        if (moneyUpgradeTimer >= 4.0f)
        {
            moneyUpgradeTimer = 0.0f;
            UpgradeMoneyText();
        }

        if (startPopUpMessageTimer)
        {
            popUpMessageTimer += Time.deltaTime;
            if (popUpMessageTimer >= 2.0f)
            {
                popUpMessageTimer = 0.0f;
                UI_PopMessage.GetComponent<UILabel>().text = " ";
            }
        }

        //for upgrades when stats box is open real time
        if (isNeighborHoodBoxOpen && currentNeighborhood != null)
        {
            //Debug.Log(" BOX OPEN ");
            UpgradeAvgStatsTabRealTime();
            UpgradePeopleStatsTabRealTime();
            UpgradeBoxStatsTabRealTime();
        }
    }

    //upgrade the money text value
    private void UpgradeMoneyText()
    {
        UI_GlobalParameters.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = ((int)BPlayerScript.money).ToString();
    }

    //deactive ui average neighbourhood stats
    private void DisableAverageStatsUI()
    {
        isNeighborHoodBoxOpen = false;
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
            UI_nUpgradesBox.gameObject.active = false;
            //average happiness

            //for real time changes in values
            isNeighborHoodBoxOpen = true;
            
            //happiness
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageHappiness().ToString();

            //for average wealth
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageWealth().ToString();

            //for average health
            UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetAverageHealth().ToString();
        }

        //activate people stats of neighborhood, 2nd Tab
        if (tabNo == 2)
        {
            //for real time changes in values
            isNeighborHoodBoxOpen = true;
           
            UI_nAvgStatsBox.gameObject.active = false;
            UI_nPeopleStatsBox.gameObject.active = true;
            UI_nUpgradesBox.gameObject.active = false;
        }

        //activate 3rd tab
        if (tabNo == 3)
        {
            //for real time changes in values
            isNeighborHoodBoxOpen = true;

            UI_nAvgStatsBox.gameObject.active = false;
            UI_nPeopleStatsBox.gameObject.active = false;
            UI_nUpgradesBox.gameObject.active = true;

            //get all the values for that neighborhood here

            //has bus stop
            UI_nUpgradesBox.gameObject.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.BusStop).ToString();

            //has train station
            UI_nUpgradesBox.gameObject.transform.GetChild(1).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.TrainStation).ToString();

            //Bike Shop
            UI_nUpgradesBox.gameObject.transform.GetChild(2).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.BikeShop).ToString();

            //Car Shop
            UI_nUpgradesBox.gameObject.transform.GetChild(3).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.CarShop).ToString();

            //no of bikes
            UI_nUpgradesBox.gameObject.transform.GetChild(4).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetNumberOfVehicles(Vehicle.Bike).ToString();

            //no of cars
            UI_nUpgradesBox.gameObject.transform.GetChild(5).GetChild(0).GetComponent<UILabel>().text = neighborhood.GetComponent<BNeighbourhood>().GetNumberOfVehicles(Vehicle.Car).ToString();
        }
    }

    private void UpgradeAvgStatsTabRealTime()
    {
        //happiness
        UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().GetAverageHappiness().ToString();

        //for average wealth
        UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().GetAverageWealth().ToString();

        //for average health
        UI_AverageNeighborhoodStats.gameObject.transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().GetAverageHealth().ToString();
    }

    private void UpgradePeopleStatsTabRealTime()
    {

    }

    private void UpgradeBoxStatsTabRealTime()
    {
        //has bus stop
        UI_nUpgradesBox.gameObject.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.BusStop).ToString();

        //has train station
        UI_nUpgradesBox.gameObject.transform.GetChild(1).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.TrainStation).ToString();

        //Bike Shop
        UI_nUpgradesBox.gameObject.transform.GetChild(2).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.BikeShop).ToString();

        //Car Shop
        UI_nUpgradesBox.gameObject.transform.GetChild(3).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().CheckIfAlreadyUpgraded(VertexUpgrades.CarShop).ToString();

        //no of bikes
        UI_nUpgradesBox.gameObject.transform.GetChild(4).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().GetNumberOfVehicles(Vehicle.Bike).ToString();

        //no of cars
        UI_nUpgradesBox.gameObject.transform.GetChild(5).GetChild(0).GetComponent<UILabel>().text = currentNeighborhood.GetComponent<BNeighbourhood>().GetNumberOfVehicles(Vehicle.Car).ToString();
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

    //on click event for upgrades tab
    public void OnNUpgradesTabClick()
    {
        setAvgStatsTab(3);
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

        //has bike
        UI_PersonStats.gameObject.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().hasVehicle(Vehicle.Bike).ToString();

        //has car
        UI_PersonStats.gameObject.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<UILabel>().text = person.GetComponent<BPerson>().hasVehicle(Vehicle.Car).ToString();
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

    //************** TOOL BOX FUNCTIONS *******************

    private void DisableVertexUpgradesBox()
    {
        VertexUpgradesBox.active = false;
    }

    private void DisableEdgeUpgradesBox()
    {
        EdgeUpgradesBox.active = false;
    }

    private void ActivateVertexUpgradesBox()
    {
        VertexUpgradesBox.active = true;
    }

    private void ActivateEdgeUpgradesBox()
    {
        EdgeUpgradesBox.active = true;
    }

    //************* ON BUTTON CLICKS ****************
    public void OnVertexBoxClick()
    {
        ActivateVertexUpgradesBox();
        CheckVertexUpgrades();
        DisableEdgeUpgradesBox();
    }

    public void OnEdgeBoxClick()
    {
        ActivateEdgeUpgradesBox();
        CheckEdgeUpgrades();
        DisableVertexUpgradesBox();
    }

    public void onCloseMarkClick()
    {
        if (VertexUpgradesBox.active == true)
        {
            DisableVertexUpgradesBox();
        }
        else
        {
            DisableEdgeUpgradesBox();
        }
    }

    // ******** vertex button clicks ***********
    public void OnBusStopButtonClick()
    {
        if (isBusUpgraded)
        {
            isVertexUpgradeClicked = true;
            vertexUpgradeType = VertexUpgradeType.BusStation;
        }
    }

    public void OnRailStationButtonClick()
    {
        if (isRailUpgrade)
        {
            isVertexUpgradeClicked = true;
            vertexUpgradeType = VertexUpgradeType.RailStation;
        }
    }

    public void OnBikeShopClick()
    {
        if (isBikeUpgraded)
        {
            isVertexUpgradeClicked = true;
            vertexUpgradeType = VertexUpgradeType.BikeShop;
        }
    }

    public void OnCarShopClick()
    {
        if (isCarUpgraded)
        {
            isVertexUpgradeClicked = true;
            vertexUpgradeType = VertexUpgradeType.CarShop;
        }
    }
    // ******** vertex button click ends *********

    // ******** edge button click starts *********
    public void OnEdgeUnImprovedButtonClick()
    {
        isEdgeUpgradeClicked = true;
        edgeUpgradeType = EdgeUpgradeType.unImproved;
    }

    public void OnEdgeFootPathButtonClick()
    {
        if (isFoothPath)
        {
            isEdgeUpgradeClicked = true;
            edgeUpgradeType = EdgeUpgradeType.FootPath;
        }
    }

    public void OnEdgeBikeTrailClick()
    {
        if (isBikeTrail)
        {
            isEdgeUpgradeClicked = true;
            edgeUpgradeType = EdgeUpgradeType.BikeTrail;
        }
    }

    public void OnEdgeSpeedRoadClick()
    {
        if (isSpeedRoad)
        {
            isEdgeUpgradeClicked = true;
            edgeUpgradeType = EdgeUpgradeType.SpeedRoad;
        }
    }

    public void OnEdgeSpeedBoulevardClick()
    {
        if (isBoulevard)
        {
            isEdgeUpgradeClicked = true;
            edgeUpgradeType = EdgeUpgradeType.SpeedBoulevard;
        }
    }

    public void OnEdgeRailClick()
    {
        if (isSpeedRail)
        {
            isEdgeUpgradeClicked = true;
            edgeUpgradeType = EdgeUpgradeType.SpeedRail;
        }
    }
    // ******** edge button click ends **********
    //************* BUTTON CLICKS END *****************

    //******** ON HOVER EVENTS **************
    public void onUnImprovedHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "UNIMPROVED, $0";
    }

    public void onUnImprovedHoverOut()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onFootPathHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "FOOTPATH, $10";
    }

    public void onFootPathUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onBikeTrailHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "BIKETRAIL, $20";
    }

    public void onBikeTrailUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onSpeedRoadHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "SPEEDROAD, $50";
    }

    public void onSpeedRoadUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onSpeedBoulevardHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "BOULEVARD, $150";
    }

    public void onSpeedBoulevardUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onSpeedRailHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "SPEEDRAIL, $300";
    }

    public void onSpeedRailUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onBusStationHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "BUS-STATION, $10";
    }

    public void onBusStationUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onRailStationHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "RAIL-STATION, $20";
    }

    public void onRailStationUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onBikeShopHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "BIKE-SHOP, $40";
    }

    public void onBikeShopUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    public void onCarShopHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = "CAR-SHOP, $100";
    }

    public void onCarShopUnHover()
    {
        UI_HoverLabel.GetComponent<UILabel>().text = " ";
    }

    //********** ON HOVER EVENTS END **********

    //we want the buttons of the vertex upgrades to be off because initially player cannot buy those upgrades
    private void DeactivateInitialVertexUpgrades()
    {
        GameObject[] vertexUpgrades = GameObject.FindGameObjectsWithTag("vertexUpgrade");
        foreach (GameObject vertexUpgrade in vertexUpgrades)
        {
            vertexUpgrade.GetComponent<UIButton>().enabled = false;
        }
    }

    private void CheckVertexUpgrades()
    {
        money = BPlayerScript.money;
        //make bus available at 20
        if (money >= 10 && money < 20)
        {
            if (!isBusUpgraded)
            {
                GameObject busStation = GameObject.Find("BusStation");
                if (busStation.GetComponent<UIButton>().enabled == false)
                {
                    busStation.GetComponent<UIButton>().enabled = true;
                    busStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBusUpgraded = true;
                }
            }
        }

        //make train pass available
        else if (money >= 20 && money < 40)
        {
            if (!isRailUpgrade)
            {
                GameObject railStation = GameObject.Find("RailStation");
                if (railStation.GetComponent<UIButton>().enabled == false)
                {
                    railStation.GetComponent<UIButton>().enabled = true;
                    railStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isRailUpgrade = true;
                }
            }

            if (!isBusUpgraded)
            {
                GameObject busStation = GameObject.Find("BusStation");
                if (busStation.GetComponent<UIButton>().enabled == false)
                {
                    busStation.GetComponent<UIButton>().enabled = true;
                    busStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBusUpgraded = true;
                }
            }
        }

        //make bike available
        else if (money >= 40 && money < 100)
        {
            if (!isRailUpgrade)
            {
                GameObject railStation = GameObject.Find("RailStation");
                if (railStation.GetComponent<UIButton>().enabled == false)
                {
                    railStation.GetComponent<UIButton>().enabled = true;
                    railStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isRailUpgrade = true;
                }
            }

            if (!isBusUpgraded)
            {
                GameObject busStation = GameObject.Find("BusStation");
                if (busStation.GetComponent<UIButton>().enabled == false)
                {
                    busStation.GetComponent<UIButton>().enabled = true;
                    busStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBusUpgraded = true;
                }
            }

            if (!isBikeUpgraded)
            {
                GameObject bikeShop = GameObject.Find("BikeShop");
                if (bikeShop.GetComponent<UIButton>().enabled == false)
                {
                    bikeShop.GetComponent<UIButton>().enabled = true;
                    bikeShop.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBikeUpgraded = true;
                }
            }
        }

        //make car available
        else if (money >= 100)
        {
            if (!isRailUpgrade)
            {
                GameObject railStation = GameObject.Find("RailStation");
                if (railStation.GetComponent<UIButton>().enabled == false)
                {
                    railStation.GetComponent<UIButton>().enabled = true;
                    railStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isRailUpgrade = true;
                }
            }

            if (!isBusUpgraded)
            {
                GameObject busStation = GameObject.Find("BusStation");
                if (busStation.GetComponent<UIButton>().enabled == false)
                {
                    busStation.GetComponent<UIButton>().enabled = true;
                    busStation.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBusUpgraded = true;
                }
            }

            if (!isBikeUpgraded)
            {
                GameObject bikeShop = GameObject.Find("BikeShop");
                if (bikeShop.GetComponent<UIButton>().enabled == false)
                {
                    bikeShop.GetComponent<UIButton>().enabled = true;
                    bikeShop.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBikeUpgraded = true;
                }
            }

            if (!isCarUpgraded)
            {
                GameObject carShop = GameObject.Find("CarShop");
                if (carShop.GetComponent<UIButton>().enabled == false)
                {
                    carShop.GetComponent<UIButton>().enabled = true;
                    carShop.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isCarUpgraded = true;
                }
            }
        }
    }

    //disable initial edge upgrades for the player
    private void DeactivateInitialEdgeUpgrades()
    {
        GameObject[] vertexUpgrades = GameObject.FindGameObjectsWithTag("edgeUpgrade");
        foreach (GameObject vertexUpgrade in vertexUpgrades)
        {
            vertexUpgrade.GetComponent<UIButton>().enabled = false;
        }
    }

    //upgrade the edges based on the money
    private void CheckEdgeUpgrades()
    {
        //for foothpath
        money = BPlayerScript.money;
        if (money >= 10 && money < 20)
        {
            if (!isFoothPath)
            {
                GameObject footPath = GameObject.Find("FootPath");
                if (footPath.GetComponent<UIButton>().enabled == false)
                {
                    footPath.GetComponent<UIButton>().enabled = true;
                    footPath.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isFoothPath = true;
                }
            }
        }

        //for bike trails
        if (money >= 20 && money < 50)
        {
            if (!isFoothPath)
            {
                GameObject footPath = GameObject.Find("FootPath");
                if (footPath.GetComponent<UIButton>().enabled == false)
                {
                    footPath.GetComponent<UIButton>().enabled = true;
                    footPath.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isFoothPath = true;
                }
            }

            if (!isBikeTrail)
            {
                GameObject bikeTrail = GameObject.Find("BikeTrail");
                if (bikeTrail.GetComponent<UIButton>().enabled == false)
                {
                    bikeTrail.GetComponent<UIButton>().enabled = true;
                    bikeTrail.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBikeTrail = true;
                }
            }
        }

        //for speed road
        if (money >= 50 && money < 150)
        {
            if (!isFoothPath)
            {
                GameObject footPath = GameObject.Find("FootPath");
                if (footPath.GetComponent<UIButton>().enabled == false)
                {
                    footPath.GetComponent<UIButton>().enabled = true;
                    footPath.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isFoothPath = true;
                }
            }

            if (!isBikeTrail)
            {
                GameObject bikeTrail = GameObject.Find("BikeTrail");
                if (bikeTrail.GetComponent<UIButton>().enabled == false)
                {
                    bikeTrail.GetComponent<UIButton>().enabled = true;
                    bikeTrail.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBikeTrail = true;
                }
            }

            if (!isSpeedRoad)
            {
                GameObject speedRoad = GameObject.Find("SpeedRoad");
                if (speedRoad.GetComponent<UIButton>().enabled == false)
                {
                    speedRoad.GetComponent<UIButton>().enabled = true;
                    speedRoad.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isSpeedRoad = true;
                }
            }
        }

        //for boulevard road
        if (money >= 150 && money <= 200)
        {
            if (!isFoothPath)
            {
                GameObject footPath = GameObject.Find("FootPath");
                if (footPath.GetComponent<UIButton>().enabled == false)
                {
                    footPath.GetComponent<UIButton>().enabled = true;
                    footPath.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isFoothPath = true;
                }
            }

            if (!isBikeTrail)
            {
                GameObject bikeTrail = GameObject.Find("BikeTrail");
                if (bikeTrail.GetComponent<UIButton>().enabled == false)
                {
                    bikeTrail.GetComponent<UIButton>().enabled = true;
                    bikeTrail.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBikeTrail = true;
                }
            }

            if (!isSpeedRoad)
            {
                GameObject speedRoad = GameObject.Find("SpeedRoad");
                if (speedRoad.GetComponent<UIButton>().enabled == false)
                {
                    speedRoad.GetComponent<UIButton>().enabled = true;
                    speedRoad.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isSpeedRoad = true;
                }
            }

            if (!isBoulevard)
            {
                GameObject boulevard = GameObject.Find("SpeedBoulevard");
                if (boulevard.GetComponent<UIButton>().enabled == false)
                {
                    boulevard.GetComponent<UIButton>().enabled = true;
                    boulevard.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                    isBoulevard = true;
                }
            }
        }

        //activate all edge upgrades
        else if (money >= 300)
        {
            GameObject[] edgeUpgrades = GameObject.FindGameObjectsWithTag("edgeUpgrade");
            foreach (GameObject edge in edgeUpgrades)
            {
                if (edge.GetComponent<UIButton>().enabled == false)
                {
                    edge.GetComponent<UIButton>().enabled = true;
                    edge.transform.GetChild(0).GetComponent<UISprite>().enabled = false;
                }
            }
            isSpeedRail = true;
            isBoulevard = true;
            isSpeedRoad = true;
            isBikeTrail = true;
            isFoothPath = true;
        }
    }

    //check for mouse inputs first click and second click
    private void CheckMouseInput()
    {
        switch ((int)mouseClickStats)
        {
            case (int)MouseClickStats.firstClick:
                if (isVertexUpgradeClicked)
                {
                  //  Debug.Log(" FIRST CLICK ON VERTEX UPGRADE ");
                    //hide both UI BOXES
                    DisablePersonStatsUI();
                    DisableAverageStatsUI();
                    currTime = waitTime;
                    mouseClickStats = MouseClickStats.secondClick;
                }
                else if (isEdgeUpgradeClicked)
                {
                  //  Debug.Log(" FIRST CLICK ON EDGE UPGRADE ");
                    DisablePersonStatsUI();
                    DisableAverageStatsUI();
                    currTime = waitTime;
                    mouseClickStats = MouseClickStats.secondClick;
                }
                break;
            case (int)MouseClickStats.secondClick:
                currTime -= Time.deltaTime;
                if (currTime <= 0)
                {
                    //if mouse button down and clicked on a neighborhood
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (isVertexUpgradeClicked)
                        {
                            if (Physics.Raycast(ray, out hit, 500))
                            {
                                if (hit.collider.tag == "Neighbourhood")
                                {
                                    isVertexUpgradeClicked = false;
                                    mouseClickStats = MouseClickStats.firstClick;

                                    //call the function to BNeighborhood here based on vertexUpgrade type
                                    //once the values are passed to BNeighborhood, we can retrieve those values from AvgStatsUIBox by clicking on its 3rd Tab                                   
                                    if ((int)vertexUpgradeType == 1)
                                    {
                                       // Debug.Log(" UPGRADE TO BUS STOP ON THIS NEIGHBORHOOD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO BUS STOP ON NEIGHBORHOOD ";
                                        Instantiate(partcile, hit.collider.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                                        hit.collider.GetComponent<BNeighbourhood>().AddUpgrade(VertexUpgrades.BusStop);
                                    }
                                    else if ((int)vertexUpgradeType == 2)
                                    {
                                       // Debug.Log(" UPGRADE TO RAIL STATION ON THIS NEIGHBORHOOD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO RAIL STATION ON NEIGHBORHOOD ";
                                        Instantiate(partcile, hit.collider.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                                        hit.collider.GetComponent<BNeighbourhood>().AddUpgrade(VertexUpgrades.TrainStation);
                                    }
                                    else if ((int)vertexUpgradeType == 3)
                                    {
                                        //Debug.Log(" UPGRADE TO BIKE SHOP ON THIS NEIGHBORHOOD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO BIKE SHOP ON NEIGHBORHOOD ";
                                        Instantiate(partcile, hit.collider.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                                        hit.collider.GetComponent<BNeighbourhood>().AddUpgrade(VertexUpgrades.BikeShop);
                                    }
                                    else
                                    {
                                       // Debug.Log(" UPGRADE TO CAR SHOP ON THIS NEIGHBORHOOD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO CAR SHOP ON NEIGHBORHOOD ";
                                        Instantiate(partcile, hit.collider.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                                        hit.collider.GetComponent<BNeighbourhood>().AddUpgrade(VertexUpgrades.CarShop);
                                    }
                                }
                            }
                        }
                        else if (isEdgeUpgradeClicked)
                        {
                            if (Physics.Raycast(ray, out hit, 500))
                            {
                                if (hit.collider.tag == "Edge")
                                {
                                    isEdgeUpgradeClicked = false;
                                    mouseClickStats = MouseClickStats.firstClick;

                                    //perform all the edge upgrades here
                                    if ((int)edgeUpgradeType == 1)
                                    {
                                        //Debug.Log(" UPGRADE TO UNIMPROVED ROAD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO IMPROVED ROAD ";
                                        hit.collider.gameObject.renderer.material = DirtRoadMaterial;
                                    }
                                     if ((int)edgeUpgradeType == 2)
                                    {
                                        //Debug.Log(" UPGRADE TO FOOTHPATH ROAD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO BUS FOOTPATH ";
                                        hit.collider.gameObject.renderer.material = FootpathMat;
                                    }
                                     if ((int)edgeUpgradeType == 3)
                                    {
                                       // Debug.Log(" UPGRADE TO BIKE TRAIL ROAD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO BIKE TRAIL ";
                                        hit.collider.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                                        hit.collider.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                                        hit.collider.gameObject.transform.GetChild(0).renderer.material = SideBikeLaneMat;
                                        hit.collider.gameObject.transform.GetChild(1).renderer.material = SideBikeLaneMat;
                                    }
                                     if ((int)edgeUpgradeType == 4)
                                    {
                                      //  Debug.Log(" UPGRADE TO SPEED ROAD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO ROAD  ";
                                        hit.collider.gameObject.renderer.material = SpeedRoadMat;
                                    }
                                     if ((int)edgeUpgradeType == 5)
                                    {
                                      //  Debug.Log(" UPGRADE TO SPEED BOULEVARD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO BOULEVARD ";
                                        hit.collider.gameObject.renderer.material = BoulevardMat;
                                    }
                                    if ((int)edgeUpgradeType == 6)
                                    {
                                       // Debug.Log(" UPGRADE TO RAIL ROAD ");
                                        startPopUpMessageTimer = true;
                                        UI_PopMessage.GetComponent<UILabel>().text = " UPGRADE TO RAIL ROAD ";
                                        hit.collider.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                                        hit.collider.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                                        hit.collider.gameObject.transform.GetChild(0).renderer.material = SideRailwayMat;
                                        hit.collider.gameObject.transform.GetChild(1).renderer.material = SideRailwayMat;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isVertexUpgradeClicked = false;
                            isEdgeUpgradeClicked = false;
                            mouseClickStats = MouseClickStats.firstClick;
                        }
                    }
                }
                break;
        }
    }

    //------------------------------- helper functions ---------------------------------
    public void setAvgStatsTab(int tabNo)
    {
        if (tabNo == 1)
            neighborhoodStatsTab = NeighborhoodStatsTab.avgstats;
        if (tabNo == 2)
            neighborhoodStatsTab = NeighborhoodStatsTab.peoplestats;
        if (tabNo == 3)
            neighborhoodStatsTab = NeighborhoodStatsTab.upgrades;

    }

    public int getAvgStatsCurrentTab()
    {
        return (int)neighborhoodStatsTab;
    }

    public int getVertexUpgradeType()
    {
        return (int)vertexUpgradeType;
    }
}
