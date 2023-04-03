using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class I_OnBoard : Interactable
{
    public float boardingTimer;
    public bool boarding;
    public Vector3 targetPos;
    public Transform targetTransform;
    public LayerMask boatMask;

    public override IEnumerator InteractionEvent()
    {
        if (!boarding && !PlayerController.instance.isNonPhysics)
        {
            boarding = true;
            boardingTimer = 0.75f;
            PlayerController.instance.rb.isKinematic = true;
            PlayerController.instance.playerCollider.enabled = false;
            Physics.Raycast(PlayerController.instance.transform.position,
                Vector3.Normalize(BoatController.instance.transform.position - PlayerController.instance.transform.position), out RaycastHit hitInfo, 5, boatMask);
            targetPos = new Vector3(hitInfo.point.x, BoatController.instance.transform.position.y + 0.75f, hitInfo.point.z);
            targetPos = targetTransform.position;

            triggerZone.triggered = false;
        }

        yield return null;
    }

    public void FixedUpdate()
    {

        if (boarding)
        {
            if (boardingTimer > 0)
            {
                boardingTimer -= Time.fixedDeltaTime;
                PlayerController.instance.transform.position =
                    Vector3.Lerp(PlayerController.instance.transform.position, targetPos, Time.fixedDeltaTime * 5);
                PlayerController.instance.rb.isKinematic = true;
                //PlayerController.instance.AttachToBoat(BoatController.instance.transform);
            }
            else
            {
                boarding = false;
                PlayerController.instance.playerCollider.enabled = true;
                PlayerController.instance.rb.isKinematic = false;
            }
        }
    }
}