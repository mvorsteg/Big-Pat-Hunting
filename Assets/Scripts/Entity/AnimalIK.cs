using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator))]
public class AnimalIK : MonoBehaviour
{

    [System.Serializable]
    public class BoneData
    {
        public Transform bone;
        public Vector3 neutralRotation;
        public Vector3 maxRotation;
    }

    [System.Serializable]
    public class LegData
    {
        public BoneData topBone;
        public BoneData midBone;
        public BoneData toe;

        public float liftLength;
        public float liftAmount;
        public float toeOffset = 0.07f;
    }

    [System.Serializable]
    public class NeckData
    {
        public BoneData head;
        public BoneData[] neck;
    }

    [SerializeField]
    private Transform leftArm;
    [SerializeField]
    private Transform leftLeg;
    [SerializeField]
    private Transform rightArm;
    [SerializeField]
    private Transform rightLeg;

    [SerializeField]
    private Transform neckBase;

    public LegData[] legs;

    [SerializeField]
    private Transform root;
    private Animal animal;
    private NavMeshAgent agent;

    private float angle;
    private float speed;

    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        animal = GetComponent<Animal>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        speed = agent.velocity.magnitude;
    }

    private void LateUpdate()
    {
        if (animal.IsAlive)
        {
            UpdateLegBones();
            SolveLegIK();
        }
    }


    private void UpdateLegBones()
    {
        RaycastHit armHit, legHit;

        if (Physics.Raycast(leftArm.position, Vector3.down, out armHit, 10, mask) && Physics.Raycast(leftLeg.position, Vector3.down, out legHit, 10, mask))
        {
            Debug.DrawLine(leftArm.position, armHit.point);
            Debug.DrawLine(leftLeg.position, legHit.point);
            float l = Vector3.Distance(legHit.point, armHit.point);
            float h = legHit.point.y - armHit.point.y;

            angle = Mathf.Lerp(angle, Mathf.Asin(h / l) * 180 / Mathf.PI, Time.deltaTime);

            root.localEulerAngles = new Vector3(angle, 0, 0);

            leftArm.localEulerAngles += new Vector3(angle, 0, 0);
            rightArm.localEulerAngles += new Vector3(-angle, 0, 0);

            leftLeg.localEulerAngles += new Vector3(0, 0, -angle);
            rightLeg.localEulerAngles += new Vector3(0, 0, -angle);

            neckBase.localEulerAngles += new Vector3(0, 0, angle);
        }
    }

    private void SolveLegIK()
    {
        RaycastHit toeHit;
        float prevToeLocation;
        float currToeLocation;
        Vector3 raycastStart;
        float ikAmount = Mathf.Clamp((1 - speed), 0, 1);

        foreach (LegData leg in legs)
        {
            prevToeLocation = leg.toe.bone.transform.position.y;

            leg.topBone.bone.localEulerAngles += (leg.topBone.maxRotation - leg.topBone.neutralRotation) * leg.liftAmount;
            leg.midBone.bone.localEulerAngles += (leg.midBone.maxRotation - leg.midBone.neutralRotation) * leg.liftAmount;

            leg.liftLength = leg.toe.bone.transform.position.y - prevToeLocation;
            currToeLocation = leg.toe.bone.transform.position.y;

            raycastStart = leg.toe.bone.transform.position + Vector3.up;
            if (Physics.Raycast(raycastStart, Vector3.down, out toeHit, 10, mask))
            {
                Debug.DrawLine(raycastStart, toeHit.point, Color.red);

                if (currToeLocation < toeHit.point.y + leg.toeOffset)
                {
                    leg.liftAmount += 0.01f;
                }
                else
                {
                    leg.liftAmount -= 0.01f;
                }
                leg.liftAmount = Mathf.Clamp(leg.liftAmount, 0, ikAmount);
            }
        }
    }
}