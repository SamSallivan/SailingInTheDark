using UnityEngine;

public class LegacyCameraSway : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Quaternion lastRotation = Quaternion.identity;

    private void LateUpdate()
    {
        base.transform.rotation = lastRotation;
        base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.identity, Time.deltaTime * speed);
        lastRotation = base.transform.rotation;
    }
}
 