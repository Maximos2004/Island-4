using UnityEngine;
using UnityEngine.UI;

public class BuildingPicker : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Sprite[] FarmSprites = new Sprite[5];
    [SerializeField] private Sprite[] HouseSprites = new Sprite[5];
    [SerializeField] private Sprite[] TowerSprites = new Sprite[5];
    [SerializeField] private Image FarmI, HouseI, TowerI;
    [SerializeField] private BuildingPlacement buildingPlacement;
    [SerializeField] private GameObject BuildingPlacerUI;

    private void OnEnable()
    {
        int index = GetSpriteIndex();
        FarmI.sprite = FarmSprites[index];
        HouseI.sprite = HouseSprites[index];
        TowerI.sprite = TowerSprites[index];
    }

    private int GetSpriteIndex()
    {
        if (gameManager.WinStreak < 2)
            return 0;
        if (gameManager.WinStreak < 4)
            return 1;
        if (gameManager.WinStreak < 6)
            return 2;
        if (gameManager.WinStreak < 10)
            return 3;
        return 4;
    }

    public void FarmButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        buildingPlacement.PlaceDownBuilding(GetSpriteIndex());
        BuildingPlacerUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HouseButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        buildingPlacement.PlaceDownBuilding(GetSpriteIndex() + 5);
        BuildingPlacerUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void TowerButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        buildingPlacement.PlaceDownBuilding(GetSpriteIndex() + 10);
        BuildingPlacerUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
