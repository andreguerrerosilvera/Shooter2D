using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Class which handles camera movement
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    /// <summary>
    ///     Enum to determine camera movement styles
    /// </summary>
    public enum CameraStyles
    {
        Locked,
        Overhead,
        Free
    }

    [Header("GameObject References")] [Tooltip("The target to follow with this camera")]
    public Transform target;

    [Header("CameraMovement")]
    [Tooltip("The way this camera moves:\n" +
             "\tLocked: Camera cannot follow mouse, it stays locked onto the target.\n" +
             "\tScroll: Camera stays within the max scroll distance of the target, but follows the mouse\n" +
             "\tFree: Camera follows the mouse, regardless of the target position")]
    public CameraStyles cameraMovementStyle = CameraStyles.Locked;

    [Tooltip("The distance between the target position and the mouse to move the camera to in \"Free\" mode.")]
    [Range(0, 0.75f)]
    public float freeCameraMouseTracking = 0.5f;

    [Tooltip("The maximum distance away from the target that the camera can move")]
    public float maxDistanceFromTarget = 5.0f;

    [Tooltip("The z coordinate to use for the camera position")]
    public float cameraZCoordinate = -10.0f;

    [Header("Input Actions & Controls")] [Tooltip("The input action(s) that map to where the camera looks")]
    public InputAction lookAction;

    // The camera being controlled by this script
    private Camera playerCamera;

    /// <summary>
    ///     Description:
    ///     When the script starts up, get the camera component to use
    ///     Inputs:
    ///     none
    ///     Returns:
    ///     void (no return)
    /// </summary>
    private void Start()
    {
        playerCamera = GetComponent<Camera>();
    }

    /// <summary>
    ///     Description:
    ///     Standard Unity function that is called every frame
    ///     Inputs: none
    ///     Returns:
    ///     void (no return)
    /// </summary>
    private void Update()
    {
        SetCameraPosition();
    }

    /// <summary>
    ///     Standard Unity function called whenever the attached gameobject is enabled
    /// </summary>
    private void OnEnable()
    {
        lookAction.Enable();
    }

    /// <summary>
    ///     Standard Unity function called whenever the attached gameobject is disabled
    /// </summary>
    private void OnDisable()
    {
        lookAction.Disable();
    }

    /// <summary>
    ///     Description:
    ///     Sets the camera's position according to the settings
    ///     Input:
    ///     none
    ///     Return:
    ///     void (no return)
    /// </summary>
    private void SetCameraPosition()
    {
        if (target != null)
        {
            var targetPosition = GetTargetPosition();
            var mousePosition = GetPlayerMousePosition();
            var desiredCameraPosition = ComputeCameraPosition(targetPosition, mousePosition);

            transform.position = desiredCameraPosition;
        }
    }

    /// <summary>
    ///     Description:
    ///     Gets the follow target's position
    ///     Inputs:
    ///     none
    ///     Returns:
    ///     Vector3
    /// </summary>
    /// <returns>Vector3: The position of the target assigned to this camera controller.</returns>
    public Vector3 GetTargetPosition()
    {
        if (target != null) return target.position;
        return transform.position;
    }

    /// <summary>
    ///     Description:
    ///     Finds and returns the mouse position
    ///     Inputs:
    ///     none
    ///     Returns:
    ///     Vector3
    /// </summary>
    /// <returns>Vector3: The position of the player's mouse in world coordinates</returns>
    public Vector3 GetPlayerMousePosition()
    {
        if (cameraMovementStyle == CameraStyles.Locked) return Vector3.zero;
        return playerCamera.ScreenToWorldPoint(lookAction.ReadValue<Vector2>());
    }

    /// <summary>
    ///     Description:
    ///     Takes the target's position and mouse position, and returns the desired position of the camera
    ///     Inputs:
    ///     Vector3 targetPosition, Vector3 offsetPosition
    ///     Returns:
    ///     Vector3
    /// </summary>
    /// <param name="targetPosition"> The position of the target the camera is following. </param>
    /// <param name="mousePosition"> The position of the mouse in world space used to determine distance from the target. </param>
    /// <returns>Vector3: The position the camera should be at</returns>
    public Vector3 ComputeCameraPosition(Vector3 targetPosition, Vector3 mousePosition)
    {
        var result = Vector3.zero;
        switch (cameraMovementStyle)
        {
            case CameraStyles.Locked:
                result = transform.position;
                break;
            case CameraStyles.Overhead:
                result = targetPosition;
                break;
            case CameraStyles.Free:
                var desiredPosition = Vector3.Lerp(targetPosition, mousePosition, freeCameraMouseTracking);
                var difference = desiredPosition - targetPosition;
                difference = Vector3.ClampMagnitude(difference, maxDistanceFromTarget);
                result = targetPosition + difference;
                break;
        }

        result.z = cameraZCoordinate;
        return result;
    }
}