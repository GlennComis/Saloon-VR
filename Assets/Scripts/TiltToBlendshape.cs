using UnityEngine;

public class TiltToBlendshape : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // Assign your SkinnedMeshRenderer in the Inspector
    public int blendShapeIndex = 0; // Blendshape index you want to animate (0 for the first blendshape)
    public float tiltThreshold = 55f; // Tilt threshold in degrees (both positive and negative)
    public float minBlendSpeed = 20f; // Minimum speed at which the blendshape weight changes
    public float maxBlendSpeed = 60f; // Maximum speed for blendshape transition at extreme tilts

    private float currentWeight = 0f; // Current blendshape weight

    void Update()
    {
        // Get the rotation angles of the object
        float xRotation = transform.eulerAngles.x;
        float zRotation = transform.eulerAngles.z;

        // Normalize the angles to range [-180, 180] for easier comparison
        if (xRotation > 180f) xRotation -= 360f;
        if (zRotation > 180f) zRotation -= 360f;

        // Calculate the maximum tilt exceeding the threshold in either direction
        float xTiltExcess = Mathf.Max(0f, Mathf.Abs(xRotation) - tiltThreshold);
        float zTiltExcess = Mathf.Max(0f, Mathf.Abs(zRotation) - tiltThreshold);

        // Determine the largest tilt excess to calculate blend speed
        float maxTiltExcess = Mathf.Max(xTiltExcess, zTiltExcess);

        // Calculate the blend speed proportionally based on tilt excess, clamping between minBlendSpeed and maxBlendSpeed
        float blendSpeed = Mathf.Lerp(minBlendSpeed, maxBlendSpeed, maxTiltExcess / (180f - tiltThreshold));

        // Check if either rotation angle exceeds the threshold (positive or negative)
        bool isTilting = Mathf.Abs(xRotation) > tiltThreshold || Mathf.Abs(zRotation) > tiltThreshold;

        // Adjust blendshape weight based on tilt
        if (isTilting)
        {
            currentWeight = Mathf.MoveTowards(currentWeight, 100f, blendSpeed * Time.deltaTime);
        }
        /*else
        {
            currentWeight = Mathf.MoveTowards(currentWeight, 0f, blendSpeed * Time.deltaTime);
        }*/

        // Apply the blendshape weight
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, currentWeight);
    }
}
