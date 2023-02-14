using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class LegacyPlayerController : MonoBehaviour
{
    [SerializeField]
    private float acceleration = 1f;

    [SerializeField]
    private float deaccerlation = 1f;

    [Space]
    [SerializeField]
    private Transform camTransform;

    [SerializeField]
    private float raycastDistance = 0.2f;

    [SerializeField]
    private float raycastRadius = 0.3f;

    [SerializeField]
    private LayerMask raycastLayer;

    [SerializeField]
    private float headHeight = 0.7f;

    [SerializeField]
    private float footHeight = -0.7f;

    [SerializeField]
    private float collisionBuffer = 0.2f;

    private bool attached;

    private Transform attachedTransform;

    private Vector2 moveInput;

    private Vector3 localVelo;

    private float t;

    private float footstepT;

    private bool init;

    public InputActionAsset inputActions;

    public bool Docking
    {
        get;
        private set;
    }

    private void Start()
    {
        //playerInput = ((PlayerInput)inputSO.Input).Player;
    }
    private void Awake()
    {
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        if (attached)
        {
            return;
        }
        Vector3 normalized = camTransform.TransformDirection(new Vector3(moveInput.x, 0f, moveInput.y)).normalized;
        normalized -= Vector3.Dot(normalized, base.transform.up) * base.transform.up;
        localVelo += base.transform.InverseTransformDirection(normalized.normalized) * acceleration * Time.fixedDeltaTime;
        localVelo = Vector3.Lerp(localVelo, Vector3.zero, Time.fixedDeltaTime * deaccerlation);
        if (localVelo != Vector3.zero)
        {
            Vector3 direction = base.transform.TransformDirection(localVelo);
            if (Physics.SphereCast(base.transform.position + base.transform.up * headHeight, raycastRadius, direction, out RaycastHit hitInfo, raycastDistance, raycastLayer))
            {
                Vector3 vector = base.transform.InverseTransformDirection(hitInfo.normal);
                vector.y = 0f;
                localVelo += vector.normalized * (1f / Mathf.Max(0.1f, hitInfo.distance)) * collisionBuffer * Time.fixedDeltaTime;
            }
            if (Physics.SphereCast(base.transform.position + base.transform.up * footHeight, raycastRadius, direction, out RaycastHit hitInfo2, raycastDistance, raycastLayer))
            {
                Vector3 vector2 = base.transform.InverseTransformDirection(hitInfo2.normal);
                vector2.y = 0f;
                localVelo += vector2.normalized * (1f / Mathf.Max(0.1f, hitInfo2.distance)) * collisionBuffer * Time.fixedDeltaTime;
            }
            base.transform.localPosition += localVelo * Time.fixedDeltaTime;
        }
    }

    private void LateUpdate()
    {
        if (attached)
        {
            t += Time.deltaTime * 2f;
            base.transform.position = Vector3.Lerp(base.transform.position, attachedTransform.position, t);
            base.transform.rotation = Quaternion.Slerp(base.transform.rotation, attachedTransform.rotation, t);
            return;
        }
        //moveInput = GetComponent<PlayerInput>().Movement.ReadValue<Vector2>();
        moveInput = inputActions.FindActionMap("Player").FindAction("Move").ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            footstepT += Time.deltaTime * 2.16f;
            if (footstepT >= 1f)
            {
            }
        }
        else
        {
            footstepT = 0.5f;
        }
    }

    public void CanUseEquipment(bool v)
    {
    }

    public void Attach(Transform attach)
    {
        t = 0f;
        attached = true;
        attachedTransform = attach;
    }

    public void UnAttach()
    {
        attached = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.1f, 0.1f, 0.9f, 0.8f);
        Gizmos.DrawSphere(base.transform.position + base.transform.up * headHeight, raycastRadius);
        Gizmos.DrawSphere(base.transform.position + base.transform.up * footHeight, raycastRadius);
    }
}
