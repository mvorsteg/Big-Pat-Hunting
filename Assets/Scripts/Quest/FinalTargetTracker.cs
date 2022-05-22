using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class FinalTargetTracker : MonoBehaviour
{   
    private class TargetInfo
    {
        public Animal animal;
        public float startingDistance;
        public bool valid;

        public TargetInfo(Animal animal, bool valid)
        {
            this.animal = animal;
            this.startingDistance = 0;
            this.valid = valid;
        }
    }

    private bool isTracking = false;
    private float startingDist;

    private Dictionary<EntityType, List<TargetInfo>> targetDict;
    private List<EntityType> currentTrackingTargets;

    [SerializeField]
    private MeshCollider worldBoundary;
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private float textDelayTime = 2f;
    [SerializeField]
    private TextMeshProUGUI trackerText;

    private UnityAction<object> onAnimalDeath;
    private UnityAction<object> onAnimalFlee;
    private UnityAction<object> onAnimnalExitBoundary;

    private void Awake()
    {
        targetDict = new Dictionary<EntityType, List<TargetInfo>>();
        currentTrackingTargets = new List<EntityType>();

        progressSlider.gameObject.SetActive(false);

        onAnimalDeath = new UnityAction<object>(OnAnimalDeath);
        onAnimalFlee = new UnityAction<object>(OnAnimalFlee);
        onAnimnalExitBoundary = new UnityAction<object>(OnAnimalExitBoundary);
    }

    private void OnEnable()
    {
        Messenger.Subscribe(MessageIDs.AnimalDeath, OnAnimalDeath);
        Messenger.Subscribe(MessageIDs.AnimalFlee, OnAnimalFlee);
        Messenger.Subscribe(MessageIDs.AnimalExitBoundary, OnAnimalExitBoundary);
    }

    private void OnDisable()
    {
        Messenger.Unsubscribe(MessageIDs.AnimalDeath, OnAnimalDeath);
        Messenger.Unsubscribe(MessageIDs.AnimalFlee, OnAnimalFlee);
        Messenger.Unsubscribe(MessageIDs.AnimalExitBoundary, OnAnimalExitBoundary);
    }

    public void AddTarget(Animal target)
    {
        List<TargetInfo> targetList;
        if (!targetDict.TryGetValue(target.type, out targetList))
        {
            targetList = new List<TargetInfo>();
            targetDict[target.type] = targetList;
        }
        targetList.Add(new TargetInfo(target, false));
    }

    public void UpdateTarget(Animal target, bool valid)
    {
        List<TargetInfo> targetList;
        int idleTargets = 0;
        if (targetDict.TryGetValue(target.type, out targetList))
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                TargetInfo t = targetList[i];
                if (t.animal == target)
                {
                    targetList[i].valid = valid;
                }
                if (t.valid && t.animal.IsAlive && t.animal.State != Entity.AIState.Flee)
                {
                    idleTargets += 1;
                }
            }
            if (idleTargets == 0)
            {
                StartCoroutine(StartTracking(target.type));
            }
        }
    }

    private IEnumerator StartTracking(EntityType type)
    {
        if (!currentTrackingTargets.Contains(type))
        {
            Debug.Log("Tracking " + type.ToString() + "!");
            if (!isTracking)
            {
                trackerText.gameObject.SetActive(true);
                yield return new WaitForSeconds(textDelayTime);
                trackerText.gameObject.SetActive(false);
                progressSlider.gameObject.SetActive(true);
                isTracking = true;
            }
            currentTrackingTargets.Add(type);
            List<TargetInfo> targetList = targetDict[type];
            foreach (TargetInfo ti in targetList)
            {
                // set initial distance
                float dist = Vector3.Distance(ti.animal.transform.position, ti.animal.Destination);
                ti.startingDistance = dist;
                Debug.DrawLine(ti.animal.transform.position, ti.animal.Destination, Color.magenta, 100f);
            }
            // now continuously update tracker until all deer escape
            int numTargets = 1;
            while (numTargets > 0)
            {
                float totalCurrDistance = 0;
                float totalStartingDistance = 0;
                numTargets = 0;
                
                foreach (TargetInfo ti in targetList)
                {
                    if (ti.valid)
                    {
                        // set current distance
                        float dist = Vector3.Distance(ti.animal.transform.position, ti.animal.Destination);
                        totalCurrDistance += dist;
                        totalStartingDistance += ti.startingDistance;
                        numTargets++;
                    }
                }
                float trackerProgress;
                if (totalStartingDistance == 0)
                {
                    trackerProgress = 1f; // avoid div by 0
                }
                else
                {
                    trackerProgress = Mathf.Clamp01(1 - (totalCurrDistance / totalStartingDistance));
                }
                progressSlider.value = trackerProgress;
                yield return null;
            }
            progressSlider.value = progressSlider.maxValue;
        }
    }

    private void OnAnimalDeath(object data)
    {
        Animal animal = (Animal)data;
        UpdateTarget(animal, false);
    }

    private void OnAnimalFlee(object data)
    {
        Animal animal = (Animal)data;
        UpdateTarget(animal, true);
    }

    private void OnAnimalExitBoundary(object data)
    {
        Animal animal = (Animal)data;
        UpdateTarget(animal, false);
    }

}