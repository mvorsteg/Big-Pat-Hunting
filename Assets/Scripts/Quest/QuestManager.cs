using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public struct KillInfo
    {
        public EntityType type;
        public float score;
        public float scale;
        public float distance;
        public BodyArea bodyArea;

        public KillInfo(EntityType type, float score, float scale, float distance, BodyArea bodyArea)
        {
            this.type = type;
            this.score = score;
            this.scale = scale;
            this.distance = distance;
            this.bodyArea = bodyArea;
        }
    }

    public Player player;
    public GameObject playerSpawn;

    public GameObject tripTextObj;
    private TextMeshProUGUI tripDayText;
    private TextMeshProUGUI tripLocationText;
    
    public TextMeshProUGUI dayCompleteText;
    public DebriefBoard debriefBoard;

    public Transform questTextParent;
    public GameObject questTextPrefab;

    public GameObject scoreTextParent;
    private TextMeshProUGUI scoreText;

    public Color disableColor;

    public HuntingTrip huntingTrip;

    private List<IQuest> quests;            // list of quests that will be tracked
    private List<TextMeshProUGUI> texts;    // list of description texts associated with these quests

    private List<KillInfo> killLog;         // list of every animal the player has killed
    
    private EntityManager entityManager;
    private CutsceneManager cutsceneManager;
    
    // private UnityAction onDayComplete;

    public static QuestManager instance;    // there can only be one

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   
        quests = new List<IQuest>();
        texts = new List<TextMeshProUGUI>();
        killLog = new List<KillInfo>();
        scoreText = scoreTextParent.GetComponent<TextMeshProUGUI>();
        tripDayText = tripTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tripLocationText = tripTextObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        entityManager = GetComponent<EntityManager>();
        cutsceneManager = GetComponent<CutsceneManager>();

        //initialize listeners
    }

    private void Start()
    {
        huntingTrip.currDay = 0;
        StartDay(huntingTrip, 0);
    }

    public static void StartDay(HuntingTrip trip, int day)
    {
        foreach (IQuest q in trip.days[day].GetQuests())
        {
            AddQuest(q);
        }
        instance.StartCoroutine(instance.ShowTripScreen(trip, day));
        instance.entityManager.SpawnBaseAnimals(GetRequiredEntities());
    }

    public static void ResetDay()
    {
        ClearDay();
        StartDay(instance.huntingTrip, instance.huntingTrip.currDay);   
    }

    public static void NextDay()
    {
        ClearDay();
        instance.huntingTrip.currDay++;
        StartDay(instance.huntingTrip, instance.huntingTrip.currDay);
    }

    public static void EndDay()
    {
        EventManager.TriggerEvent("DayComplete");
        instance.debriefBoard.gameObject.SetActive(true);
        instance.cutsceneManager.PlayNightCutscene();
        foreach (KillInfo info in instance.killLog)
        {
            instance.debriefBoard.AddRow(info);
        }
    }

    public static void ClearDay()
    {
        instance.killLog.Clear();
        instance.player.ResetPlayer();
        instance.player.transform.GetComponent<PlayerMovement>().MoveToPosition(instance.playerSpawn.transform.position);
        instance.player.transform.rotation = instance.playerSpawn.transform.rotation;
        instance.entityManager.DeleteAllAnimals();

        for (int i = 0; i < instance.questTextParent.childCount; i++)
        {
            Destroy(instance.questTextParent.GetChild(0).gameObject);
        }
    }    

    /// <summary>
    /// Adds a new quest that is tracked
    /// </summary>
    /// <param name="quest">The quest being added</param>
    public static void AddQuest(IQuest quest)
    {
        TextMeshProUGUI questText = Instantiate(instance.questTextPrefab, instance.questTextParent).GetComponent<TextMeshProUGUI>();
        questText.text = quest.GetShortDescription();
        instance.quests.Add(quest);
        instance.texts.Add(questText);
    }

    /// <summary>
    /// Updates the text of a quest already being tracked
    /// </summary>
    /// <param name="quest">The quest to update</param>
    public static void UpdateQuest(IQuest quest)
    {
        int idx = instance.quests.IndexOf(quest);
        instance.texts[idx].text = quest.GetShortDescription();
    }

    /// <summary>
    /// Completes a quest by removing it from the list and greying out the color of its text
    /// </summary>
    /// <param name="quest">The quest to remove</param>
    public static void CompleteQuest(IQuest quest)
    {
        int idx = instance.quests.IndexOf(quest);
        instance.quests.Remove(quest);
        instance.texts[idx].color = instance.disableColor;
        instance.texts.RemoveAt(idx);
        if (instance.quests.Count == 0)
        {
            instance.StartCoroutine(instance.EndDayCoroutine());
        }
    }

    /// <summary>
    /// Registers a kill made by the player, and checks all known KillQuests to see if the kill counts for any
    /// </summary>
    /// <param name="entity">The entity that has been killed</param>
    /// <param name="info">The HitInfo from the hit that killed the entity</param>
    public static void RegisterKill(Entity entity, HitInfo info)
    {   
        float score = CalculateScore(entity, info, 1.0f);
        instance.killLog.Add(new KillInfo(entity.type, score, entity.transform.localScale.x, info.distance, info.bodyArea));
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
                        i--;
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
                    if (kq.TotalKills == kq.requiredKills - 1)
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
        foreach (IQuest q in instance.quests)
        {
            Dictionary<EntityType, int> qReqs = q.GetRequiredEntities();
            foreach (EntityType k in qReqs.Keys)
            {
                if (!reqs.ContainsKey(k))
                {
                    reqs[k] = 0;
                }
                reqs[k] += qReqs[k];
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
        tripDayText.text = "Day " + (day + 1) + " of " + trip.days.Length;
        tripLocationText.text = trip.Location;
        tripTextObj.SetActive(true);
        tripDayText.color = new Color(tripDayText.color.r, tripDayText.color.g, tripDayText.color.b, 1);
        tripLocationText.color = new Color(tripLocationText.color.r, tripLocationText.color.g, tripLocationText.color.b, 1);
        

        Image textDayBg = tripTextObj.GetComponent<Image>();
        textDayBg.color = new Color(textDayBg.color.r, textDayBg.color.g, textDayBg.color.b, 1);

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
            textDayBg.color = new Color(textDayBg.color.r, textDayBg.color.g, textDayBg.color.b, 1 - (elapsedTime / fadeTime));
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            tripDayText.color = new Color(tripDayText.color.r, tripDayText.color.g, tripDayText.color.b, 1 - (elapsedTime / fadeTime));
            tripLocationText.color = new Color(tripLocationText.color.r, tripLocationText.color.g, tripLocationText.color.b, 1 - (elapsedTime / fadeTime));
            yield return null;
        }
    }

    private IEnumerator EndDayCoroutine()
    {
        yield return new WaitForSeconds(2);

        EndDay();
    }

}