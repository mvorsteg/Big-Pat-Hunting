using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/HuntingTrip")]
public class HuntingTrip : ScriptableObject
{

    [System.Serializable]
    public struct Day
    {
        public System.DayOfWeek day;
        [SerializeField]
        [RequireInterface(typeof(IQuest))]
        private UnityEngine.Object[] questObjs;

        public IQuest[] GetQuests()
        {
            IQuest[] qs = new IQuest[questObjs.Length];
            for (int i = 0; i < questObjs.Length; i++)
            {
                Object qo = Instantiate<Object>(questObjs[i]);
                qs[i] = (IQuest)qo;
            }
            return qs;
        }
    }

    private int numDays;
    private int currDay;

    public Day[] days;

    
}