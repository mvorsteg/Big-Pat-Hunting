
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI regionName, regionDesc;
    [SerializeField]
    private Image previewImage;
    
    [SerializeField]
    private Transform buttonParent;
    public GameObject buttonPrefab;

    [SerializeField]
    private RegionDefinition[] regions;

    private UnityAction<LevelDefinition> onLevelClick;

    private void Awake()
    {
        onLevelClick = new UnityAction<LevelDefinition>(OnLevelClick);
    }

    private void OnEnable()
    {
        SetupMenuForLevelGroup(LevelLoader.currRegionId);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void SelectLevel(LevelLoader.LevelIDs levelID)
    {
        LevelLoader.LoadLevel(levelID);
    }

    private void SetupMenuForLevelGroup(int regionId)
    {
        RegionDefinition activeRegion = regions[regionId];
        regionName.text = activeRegion.regionName;

        for (int i = 0; i < activeRegion.levels.Length; i++)
        {
            LevelDefinition activeLevel = activeRegion.levels[i];
            Button newButton = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
            newButton.onClick.AddListener(delegate { onLevelClick(activeLevel); } );
            if (LevelLoader.LevelStates[activeLevel.levelID] != LevelLoader.LevelState.Incomplete)
            {
                newButton.interactable = false;
            }

            Text newButtonText = newButton.GetComponentInChildren<Text>();
            newButtonText.text = activeLevel.siteName;
            
        }
    }

    public void OnLevelClick(LevelDefinition level)
    {
        LevelLoader.LoadLevel(level.levelID);
    }

    public void OnLevelPointerEnter(LevelDefinition level)
    {

    }

    public void OnLevelPointerExit()
    {

    }

    
}