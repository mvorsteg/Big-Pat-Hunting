using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class QuestManager : MonoBehaviour
{
    public Player player;
    public GameObject playerSpawn;

    [SerializeField]
    private TextMeshProUGUI levelTitleText, levelDetailText, anyKeyText;
    private GameObject levelTextParent;
    
    public TextMeshProUGUI dayCompleteText;
    public DebriefBoard debriefBoard;

    public Transform questTextParent;
    public GameObject questTextPrefab;

    public GameObject scoreTextParent;
    private TextMeshProUGUI scoreText;

    public Color disableColor;

    [SerializeField]
    private LevelDefinition levelDef;
    public  List<Quest> quests;            // list of quests that will be tracked
    private List<TextMeshProUGUI> texts;    // list of description texts associated with these quests

    private List<KillQuest.KillInfo> killLog;         // list of every animal the player has killed
    private RecordSerializer recordSerializer;

    private EntityManager entityManager;
    private CutsceneManager cutsceneManager;
    private FinalTargetTracker targetTracker;
    private bool isBulletTime = false;    

    // private UnityAction onDayComplete;
    private UnityAction onBulletTimeStart;
    private UnityAction onBulletTimeEnd;
    private UnityAction onAnyKeyPressed;
    private UnityAction<object> onQuestComplete;
    private UnityAction<object> onQuestFailed;
    private UnityAction<object> onAnimalFlee;
    private UnityAction<object> onAnimalEnterBoundary;
    private UnityAction<object> onAnimalExitBoundary;

    public static QuestManager instance;    // there can only be one

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   

        texts = new List<TextMeshProUGUI>();
        killLog = new List<KillQuest.KillInfo>();
        recordSerializer = FindObjectOfType<RecordSerializer>();
        scoreText = scoreTextParent.GetComponent<TextMeshProUGUI>();
        //levelTitleText = tripTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //levelDetailText = tripTextObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        levelTextParent = levelTitleText.transform.parent.gameObject;
        anyKeyText.gameObject.SetActive(false);
        entityManager = FindObjectOfType<EntityManager>();
        cutsceneManager = FindObjectOfType<CutsceneManager>();
        targetTracker = FindObjectOfType<FinalTargetTracker>();

        //initialize listeners
        onBulletTimeStart = new UnityAction(OnBulletTimeStart);
        onBulletTimeEnd = new UnityAction(OnBulletTimeEnd);
        onAnyKeyPressed = new UnityAction(OnAnyKeyPressed);
        onQuestFailed = new UnityAction<object>(OnQuestFailed);
        onQuestComplete = new UnityAction<object>(OnQuestComplete);
        onAnimalFlee = new UnityAction<object>(OnAnimalFlee);
        onAnimalEnterBoundary = new UnityAction<object>(OnAnimalEnterBoundary);
        onAnimalExitBoundary = new UnityAction<object>(OnAnimalExitBoundary);
    }

    private void Start()
    {
        foreach (Quest q in quests)
        {
            q.Initialize(); // need to reset scriptable objects
            AddQuest(q);
        }

        // keep track of how many targets we have in the scene
        Animal[] activeAnimals = FindObjectsOfType<Animal>();
        Dictionary<EntityType, int> typeDict = new Dictionary<EntityType, int>();
        for (int i = 0; i < activeAnimals.Length; i++)
        {
            EntityType type = activeAnimals[i].type;
            int count = 0;
            typeDict.TryGetValue(type, out count);
            typeDict[type] = count + 1; 
        }
        // track relevant animals
        foreach (Quest q in quests)
        {
            if (q is KillQuest)
            {
                EntityType targetType = ((KillQuest)q).target;
                foreach (Animal animal in activeAnimals)
                {
                    if (animal.type == targetType)
                    {
                        targetTracker.AddTarget(animal);
                    }
                }
            }
        }
        Messenger.SendMessage(MessageIDs.LevelStart);
    }

    private void OnEnable()
    {
        Messenger.Subscribe(MessageIDs.BulletTimeStart, onBulletTimeStart);
        Messenger.Subscribe(MessageIDs.BulletTimeEnd, onBulletTimeEnd);
        Messenger.Subscribe(MessageIDs.QuestComplete, onQuestComplete);
        Messenger.Subscribe(MessageIDs.QuestFailed, onQuestFailed);
        Messenger.Subscribe(MessageIDs.AnimalFlee, onAnimalFlee);
        Messenger.Subscribe(MessageIDs.AnimalEnterBoundary, onAnimalEnterBoundary);
        Messenger.Subscribe(MessageIDs.AnimalExitBoundary, onAnimalExitBoundary);
    }

    private void OnDisable()
    {
        Messenger.Unsubscribe(MessageIDs.BulletTimeStart, onBulletTimeStart);
        Messenger.Unsubscribe(MessageIDs.BulletTimeEnd, onBulletTimeEnd);
        Messenger.Unsubscribe(MessageIDs.AnyKeyPressed, onAnyKeyPressed);
        Messenger.Unsubscribe(MessageIDs.QuestComplete, onQuestComplete);
        Messenger.Unsubscribe(MessageIDs.QuestFailed, onQuestFailed);
        Messenger.Unsubscribe(MessageIDs.AnimalFlee, onAnimalFlee);
        Messenger.Unsubscribe(MessageIDs.AnimalEnterBoundary, onAnimalEnterBoundary);
        Messenger.Unsubscribe(MessageIDs.AnimalExitBoundary, onAnimalExitBoundary);
    }

    public static void EndDay()
    {
        Messenger.SendMessage(MessageIDs.DayComplete);
        instance.debriefBoard.gameObject.SetActive(true);
        instance.cutsceneManager.PlayNightCutscene();
        foreach (KillQuest.KillInfo info in instance.killLog)
        {
            instance.debriefBoard.AddRow(info);
        }
    }

    /// <summary>
    /// Adds a new quest that is tracked
    /// </summary>
    /// <param name="quest">The quest being added</param>
    public static void AddQuest(Quest quest)
    {
        TextMeshProUGUI questText = Instantiate(instance.questTextPrefab, instance.questTextParent).GetComponent<TextMeshProUGUI>();
        questText.text = quest.GetShortDescription();
        instance.texts.Add(questText);
    }

    /// <summary>
    /// Updates the text of a quest already being tracked
    /// </summary>
    /// <param name="quest">The quest to update</param>
    public static void UpdateQuest(Quest quest)
    {
        int idx = instance.quests.IndexOf(quest);
        instance.texts[idx].text = quest.GetShortDescription();
    }

    /// <summary>
    /// Registers a kill made by the player, and checks all known KillQuests to see if the kill counts for any
    /// </summary>
    /// <param name="entity">The entity that has been killed</param>
    /// <param name="info">The HitInfo from the hit that killed the entity</param>
    public static void RegisterKill(Entity entity, HitInfo info)
    {   
        float score = CalculateScore(entity, info, 1.0f);
        instance.killLog.Add(new KillQuest.KillInfo(entity.type, score, entity.transform.localScale.x, info.distance, info.bodyArea));
        //instance.scoreText.text = "+" + score.ToString("F0");
        //instance.StartCoroutine(instance.ScoreTextFade());
        for (int i = 0; i < instance.quests.Count; i++)
        {
            if (instance.quests[i] is KillQuest)
            {
                KillQuest kq = (KillQuest)instance.quests[i];
                if (kq.target == entity.type)
                {
                    // if the quest is completed, decrease i
                    if (kq.RegisterKill(entity))
                    {
                        //i--;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns true if and only if dealing a certain amount of damage to a certain entity
    /// will end the day. This requires exactly 1 kill quest to be active.
    /// </summary>
    /// <param name="entity">The entity in question</param>
    /// <param name="damage">The damage being dealt to the entity</param>
    /// <returns>If the shot will end the day</returns>
    public static bool IsGoingToBeLastShot(Entity entity, float damage)
    {
        if (instance.quests.Count == 1)
        {
            if (instance.quests[0] is KillQuest)
            {
                KillQuest kq = (KillQuest)instance.quests[0];
                if (kq.target == entity.type)
                {
                    if (kq.NumAvailableTargets <= 1)
                    {
                        if (entity.Health <= damage)
                        {
                            return true;
                        }
                    }
                }
            }   
        }
        return false;
    }

    /// <summary>
    /// Congregates a list of all required entities from all active quests
    /// </summary>
    /// <returns>A Dictionary &lt;EntityType, int&gt; containing all entities required for quests</returns>
    public static Dictionary<EntityType, int> GetRequiredEntities()
    {
        Dictionary<EntityType, int> reqs = new Dictionary<EntityType, int>();
        foreach (Quest q in instance.quests)
        {
            if (q is KillQuest)
            {
                KillQuest kq = (KillQuest)q;
                Dictionary<EntityType, int> kqReqs = kq.GetRequiredEntities();
                foreach (EntityType k in kqReqs.Keys)
                {
                    if (!reqs.ContainsKey(k))
                    {
                        reqs[k] = 0;
                    }
                    reqs[k] += kqReqs[k];
                }
            }
        }
        return reqs;
    }

    /// <summary>
    /// Calculates the score the player receives for killing an entity with parameters
    /// </summary>
    /// <param name="entity">The entity that was killed</param>
    /// <param name="info">The HitInfo associated with the kill</param>
    /// <param name="bonusMultiplier">A bonus multiplier that may be awarded</param>
    /// <returns>The total score</returns>
    public static float CalculateScore(Entity entity, HitInfo info, float bonusMultiplier)
    {
        float score = entity.type.baseValue;
        // apdjust score depending on which part of body was hit
        switch(info.bodyArea)
        {
            case BodyArea.Vitals :
                score *= 2.0f;
                break;
            case BodyArea.Head :
                score *= 1.5f;
                break;
            case BodyArea.Neck :
                score *= 1.25f;
                break;
            case BodyArea.Leg :
                score *= 0.75f;
                break;
            case BodyArea.Foot :
                score *= 0.5f;
                break;
            default :
                break;
        }
        // adjust score depending on the distance of the shot
        if (info.distance >= 500f)
        {
            score *= 3f;
        }
        else if (info.distance >= 300f)
        {
            score *= 2.5f;
        }
        else if (info.distance >= 200f)
        {
            score *= 2f;
        }
        else if (info.distance >= 100f)
        {
            score *= 1.5f;
        }
        else if (info.distance >= 50f)
        {
            score *= 1.25f;
        }
        
        return score * bonusMultiplier;
    }

    private IEnumerator ScoreTextFade()
    {
        Color startColor = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1);
        Color endColor = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0);
        scoreText.color = startColor;

        yield return new WaitForSeconds(2f);

        float timeElapsed = 0f;
        float maxTime = 0.5f;
        while (timeElapsed < maxTime)
        {
            timeElapsed += Time.deltaTime;
            scoreText.color = Color.Lerp(startColor, endColor, timeElapsed / maxTime);
            yield return null;
        }
    }

    private IEnumerator ShowTripScreen(HuntingTrip trip, int day)
    {
        levelTitleText.text = "Day " + (day + 1) + " of " + trip.days.Length;
        levelDetailText.text = trip.Location;
        levelTextParent.SetActive(true);
        levelTitleText.color = new Color(levelTitleText.color.r, levelTitleText.color.g, levelTitleText.color.b, 1);
        levelDetailText.color = new Color(levelDetailText.color.r, levelDetailText.color.g, levelDetailText.color.b, 1);
        

        //Image textDayBg = tripTextObj.GetComponent<Image>();
        //textDayBg.color = new Color(textDayBg.color.r, textDayBg.color.g, textDayBg.color.b, 1);

        float elapsedTime = 0f;
        float displayTime = 0f;
        float fadeTime = 2f;

        yield return null;

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(displayTime);

        Time.timeScale = 1f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            //textDayBg.color = new Color(textDayBg.color.r, textDayBg.color.g, textDayBg.color.b, 1 - (elapsedTime / fadeTime));
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            levelTitleText.color = new Color(levelTitleText.color.r, levelTitleText.color.g, levelTitleText.color.b, 1 - (elapsedTime / fadeTime));
            levelDetailText.color = new Color(levelDetailText.color.r, levelDetailText.color.g, levelDetailText.color.b, 1 - (elapsedTime / fadeTime));
            yield return null;
        }
        levelTextParent.SetActive(false);
    }

    private IEnumerator ShowEndScreen(string titleText, string subText)
    {
        // TODO Animate Text
        yield return new WaitUntil(() => !isBulletTime);
        levelTitleText.text = titleText;
        levelDetailText.text = subText;
        levelTextParent.SetActive(true);
        recordSerializer.WriteQuest("GR-1.xml", killLog, "GR-1");
        StartCoroutine(AnyKeyEnable(2f));
    }

    private IEnumerator AnyKeyEnable(float delaySeconds)
    {
        yield return new WaitForSecondsRealtime(delaySeconds);
        anyKeyText.gameObject.SetActive(true);
        Messenger.Subscribe(MessageIDs.AnyKeyPressed, onAnyKeyPressed);
    }

    private void OnBulletTimeStart()
    {   
        isBulletTime = true;    
    }

    private void OnBulletTimeEnd()
    {
        isBulletTime = false;
    }

    private void OnAnyKeyPressed()
    {
        Messenger.Unsubscribe(MessageIDs.AnyKeyPressed, onAnyKeyPressed);
        LevelLoader.LoadLevel(LevelLoader.LevelIDs.LevelSelect);
    }

    private void OnQuestComplete(object data)
    {
        Quest quest = (Quest)data;
        Debug.Log("Quest " + quest.GetShortDescription() + " complete!");

        int x = ((KillQuest)quest).TotalKills;
        string subtext = "You got " + x + (x != 1 ? " bucks!" : " buck!");
        StartCoroutine(ShowEndScreen("Hunt Complete", subtext));

        LevelLoader.LevelStates[levelDef.levelID] = LevelLoader.LevelState.Complete;

        Messenger.SendMessage(MessageIDs.LevelEnd);
    }

    private void OnQuestFailed(object data)
    {
        Quest quest = (Quest)data;
        Debug.Log("Quest " + quest.GetShortDescription() + " failed!");

        StartCoroutine(ShowEndScreen("Hunt Failed", "You let em all get away!"));
        
        LevelLoader.LevelStates[levelDef.levelID] = LevelLoader.LevelState.Failed;

        Messenger.SendMessage(MessageIDs.LevelEnd);
    }

    private void OnAnimalFlee(object data)
    {
        // Animal animal = (Animal)data;
        // foreach (Quest q in quests)
        // {
        //     if (q is KillQuest)
        //     {
        //         KillQuest kq = (KillQuest)q;
        //         if (kq.target == animal.type)
        //         {
        //             targetTracker.AddTarget(animal);
        //             if (fleeingTargets.Count >= kq.NumAvailableTargets)
        //             {
        //                 Messenger.SendMessage(MessageIDs.ShowTargetTracker, fleeingTargets)
        //             }
        //         }
        //     }
        // }
    }

    private void OnAnimalEnterBoundary(object data)
    {
        Animal animal = (Animal)data;
        {
            foreach (Quest q in quests)
            {
                if (q is KillQuest)
                {
                    KillQuest kq = (KillQuest)q;
                    if (kq.target == animal.type)
                    {
                        kq.NumAvailableTargets += 1;
                        Debug.Log(animal.type.name + " entered.\n" + kq.NumAvailableTargets + " remaining.");
                    }
                }
            }
        }
    }

    private void OnAnimalExitBoundary(object data)
    {
        Animal animal = (Animal)data;
        {
            foreach (Quest q in quests)
            {
                if (q is KillQuest)
                {
                    KillQuest kq = (KillQuest)q;
                    if (kq.target == animal.type)
                    {
                        kq.NumAvailableTargets -= 1;
                        //fleeingTargets.Remove(animal);
                        Debug.Log(animal.type.name + " exited.\n" + kq.NumAvailableTargets + " remaining.");
                    }
                }
            }
        }
    }
}