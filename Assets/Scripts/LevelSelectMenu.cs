using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI regionName, regionDesc;
    [SerializeField]
    private Image previewImage;
    [SerializeField]
    private TextMeshProUGUI previewText;

    [SerializeField]
    private Transform buttonParent;
    public GameObject buttonPrefab;
    [SerializeField]
    private Color incompleteColor, completeColor, failedColor;

    [SerializeField]
    private GameObject debriefBoardObj;
    [SerializeField]
    private GameObject levelLoadObj;
    private DebriefBoard debriefBoard;

    private RecordSerializer recordSerializer;

    [SerializeField]
    private RegionDefinition[] regions;
    private RegionDefinition activeRegion;

    private UnityAction<LevelDefinition> onLevelClick;

    private void Awake()
    {
        recordSerializer = FindObjectOfType<RecordSerializer>();
        onLevelClick = new UnityAction<LevelDefinition>(OnLevelClick);

        debriefBoard = debriefBoardObj.GetComponent<DebriefBoard>();
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
        LevelLoader.LevelStates[LevelLoader.LevelIDs.GR1] = LevelLoader.LevelState.Complete;

        activeRegion = regions[regionId];
        regionName.text = activeRegion.regionName;

        for (int i = 0; i < activeRegion.levels.Length; i++)
        {
            LevelDefinition activeLevel = activeRegion.levels[i];
            
            // add delegates to button
            Button newButton = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
            newButton.onClick.AddListener(delegate { onLevelClick(activeLevel); } );

            EventTrigger newTrigger = newButton.GetComponent<EventTrigger>();

            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener( (eventData) => { OnLevelPointerEnter(activeLevel); } );
            newTrigger.triggers.Add(pointerEnter);

            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener( (eventData) => { OnLevelPointerExit(); } );
            newTrigger.triggers.Add(pointerExit);

            if (LevelLoader.LevelStates[activeLevel.levelID] == LevelLoader.LevelState.Failed)
            {
                newButton.interactable = false;
            }

            TextMeshProUGUI newButtonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            newButtonText.text = activeLevel.siteName;

            switch (LevelLoader.LevelStates[activeLevel.levelID])
            {
                case LevelLoader.LevelState.Incomplete :
                    newButtonText.color = incompleteColor;
                    break;
                case LevelLoader.LevelState.Complete :
                    newButtonText.color = completeColor;
                    break;
                case LevelLoader.LevelState.Failed :
                    newButtonText.color = failedColor;
                    break;
            }       
        }
    }

    private void PopulateDebriefBoard(LevelDefinition level)
    {
        debriefBoard.Clear();

        LevelRecord record;
        if (recordSerializer.ReadQuest(level.levelID.ToString(), out record))
        {
            foreach (KillQuest.KillInfo info in record.entries)
            {
                debriefBoard.AddRow(info);
            }
        }
    }

    public void OnLevelClick(LevelDefinition level)
    {
        // if level is not yet complete, load it
        if (LevelLoader.LevelStates[level.levelID] == LevelLoader.LevelState.Incomplete)
        {
            LevelLoader.LoadLevel(level.levelID);
        }
        // if level is already complete, review the log
        else if (LevelLoader.LevelStates[level.levelID] == LevelLoader.LevelState.Complete)
        {
            levelLoadObj.SetActive(false);
            debriefBoardObj.SetActive(true);
            PopulateDebriefBoard(level);
        }
    }

    public void OnLevelPointerEnter(LevelDefinition level)
    {
        //Debug.Log("entered " + level.name);
        previewImage.sprite = level.siteImage;
        previewText.text = level.siteDescription;
    }

    public void OnLevelPointerExit()
    {
        //Debug.Log("exited");
    }

    
}