using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour 
{
    [SerializeField] private XRNode inputSource;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundLayer;

    private float additionalHeight = 0.2f;
    private float fallSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        rig = this.GetComponent<XRRig>();

    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    void FixedUpdate()
    {
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        direction = (direction * Time.fixedDeltaTime) * movementSpeed;

        if(IsGrounded() == true)
        {
            fallSpeed = 0;
        }
        else
        {
            fallSpeed += gravity * Time.fixedDeltaTime;
        }

        characterController.Move(new Vector3(direction.x , fallSpeed * Time.fixedDeltaTime, direction.z));
    }

    private void CapsuleFollowHeadset()
    {
        characterController.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = this.transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }

    private bool IsGrounded()
    {
        Vector3 rayStart = this.transform.TransformPoint(characterController.center);
        float rayLength = characterController.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, characterController.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
