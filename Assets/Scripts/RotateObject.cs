using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private GameObject otherIsland;

    private void OnMouseDrag()
    {
        float xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;

        transform.Rotate(Vector3.down, xAxisRotation);

        //change the rotation of the other island to match
        otherIsland.transform.rotation = transform.rotation;
    }
}
