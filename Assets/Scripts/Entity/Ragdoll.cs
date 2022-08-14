using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private bool isRagdoll;
    
    [SerializeField]
    private GameObject[] ragdollBones;
    [SerializeField]
    private Collider[] additionaNonRagdollColliders, additionalRagdollColliders;
    [SerializeField]
    private Rigidbody[] additionalNonRagdollRigidbodies, additionalRagdollRigidbodies;
    
    private Animator anim;

    public bool IsRagdoll { get => isRagdoll; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetRagdoll(bool value)
    {
        Debug.Log("Setting ragdoll " + value);
        isRagdoll = value;
        anim.enabled = !value;

        foreach (Collider collider in additionaNonRagdollColliders)
        {
            collider.enabled = !value;
        }
        foreach (Collider collider in additionalRagdollColliders)
        {    
            collider.enabled = value;
        }

        foreach (Rigidbody rb in additionalNonRagdollRigidbodies)
        {
            rb.isKinematic = value;
        }
        foreach (Rigidbody rb in additionalRagdollRigidbodies)
        {
            rb.isKinematic = !value;
        }

        foreach (GameObject bone in ragdollBones)
        {
            if (bone.TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = value;
            }
            if (bone.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = !value;
            }
        }
    }
}