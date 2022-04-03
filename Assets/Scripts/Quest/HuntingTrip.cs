using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/HuntingTrip")]
public class HuntingTrip : ScriptableObject
{

    [System.Serializable]
    public struct Day
    {
        public System.DayOfWeek day;
        [SerializeField]
        private Quest[] quests;

        public Quest[] GetQuests()
        {
            return quests;
        }
    }

    public int numDays;
    public int currDay;

    public Day[] days;

    public string Location;

    
}