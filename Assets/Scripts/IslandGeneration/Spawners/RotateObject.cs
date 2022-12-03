using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private GameObject otherIsland;

    private void OnMouseDrag()
    {
        float xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed; 

        Vector3 rotateY = new Vector3(0, xAxisRotation, 0);
        transform.Rotate(rotateY, Space.Self);

        //change the rotation of the other island to match
        otherIsland.transform.rotation = transform.rotation;

    }
}
