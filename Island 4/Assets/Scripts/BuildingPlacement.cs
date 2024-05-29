using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildingPrefabs; //List of all possible building prefabs
    [SerializeField] private List<GameObject> highlightObject; //The highlight object for valid placement
    [SerializeField] private List<GameObject> invalidHighlightObject; //The highlight object for invalid placement
    [SerializeField] private LayerMask placementLayerMask; //The layer mask to define valid placement areas
    [SerializeField] private LayerMask collisionLayerMask; //The layer mask to define which layers to check for collisions
    [SerializeField] private Transform parentTransform; //The parent transform under which buildings will be placed

    private bool isPlacing = false;
    private float rotationSpeed = 70f; //Speed of rotation

    [HideInInspector] public List<GameObject> placedBuildings = new List<GameObject>(); //List to keep track of placed buildings
    private GameObject selectedBuildingPrefab; //The currently selected building prefab
    private int BuildingNumber;

    void Start()
    {
        LoadBuildings();
    }

    public void PlaceDownBuilding(int BuildingNumber)
    {
        isPlacing = true;
        highlightObject[BuildingNumber].SetActive(true);
        invalidHighlightObject[BuildingNumber].SetActive(true);
        selectedBuildingPrefab = buildingPrefabs[BuildingNumber];
        this.BuildingNumber = BuildingNumber;
    }

    void Update()
    {
        if (isPlacing)
        {
            HandlePlacement();
            RotateBuilding();
        }
    }

    void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayerMask))
        {
            //Move the highlight objects to follow the mouse
            highlightObject[BuildingNumber].transform.position = hit.point;
            invalidHighlightObject[BuildingNumber].transform.position = hit.point;

            bool canPlace = !IsColliding(hit.point);

            highlightObject[BuildingNumber].SetActive(canPlace);
            invalidHighlightObject[BuildingNumber].SetActive(!canPlace);

            //Place the building on mouse click if placement is valid
            if (canPlace && Input.GetMouseButtonDown(0))
            {
                if (selectedBuildingPrefab != null)
                {
                    GameObject placedBuilding = Instantiate(selectedBuildingPrefab, hit.point, highlightObject[BuildingNumber].transform.rotation, parentTransform);
                    placedBuildings.Add(placedBuilding);
                    FindObjectOfType<AudioManager>().Play("Place Building");
                    SaveSystem.SaveBuildings(this);
                    isPlacing = false;
                    highlightObject[BuildingNumber].SetActive(false);
                    invalidHighlightObject[BuildingNumber].SetActive(false);
                }
            }
        }
        else
        {
            //Hide the highlight objects if not hovering over a valid area
            highlightObject[BuildingNumber].SetActive(false);
            invalidHighlightObject[BuildingNumber].SetActive(false);
        }
    }

    void RotateBuilding()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            highlightObject[BuildingNumber].transform.Rotate(Vector3.up, scroll * rotationSpeed);
            invalidHighlightObject[BuildingNumber].transform.Rotate(Vector3.up, scroll * rotationSpeed);
        }
    }

    bool IsColliding(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, highlightObject[BuildingNumber].transform.localScale / 2, highlightObject[BuildingNumber].transform.rotation, collisionLayerMask);
        foreach (var collider in colliders)
        {
            if (collider.gameObject != highlightObject[BuildingNumber] && collider.gameObject != invalidHighlightObject[BuildingNumber])
            {
                return true;
            }
        }
        return false;
    }

    public void LoadBuildings()
    {
        BuildingData data = SaveSystem.LoadBuildings();
        if (data != null)
        {
            foreach (var buildingInfo in data.buildings)
            {
                GameObject prefab = buildingPrefabs.Find(p => p.name == buildingInfo.prefabName);
                if (prefab)
                {
                    GameObject building = Instantiate(prefab, buildingInfo.GetPosition(), buildingInfo.GetRotation(), parentTransform);
                    placedBuildings.Add(building);
                }
            }
        }
    }
}
