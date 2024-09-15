using UnityEngine;

public class TreeChop : MonoBehaviour
{
    public GameObject logPrefab;
    public GameObject leavesPrefab;
    public Vector3 leafPoint;
    public Vector3 logPoint;
    public AudioClip chopSound;
    public AudioSource audioSource;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Axe"))
        {
            // Play chop sound
            audioSource.PlayOneShot(chopSound);

            // Spawn log
            Instantiate(logPrefab, transform.position + logPoint, transform.rotation);

            // Spawn leaves
            Instantiate(leavesPrefab, transform.position + leafPoint, transform.rotation);
        }
    }
}
