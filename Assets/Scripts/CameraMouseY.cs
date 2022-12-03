using UnityEngine;
using Cinemachine;

public class CameraMouseY : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera[] virtualCamera = new CinemachineVirtualCamera[2];
    [SerializeField] float sensitivity = 1f;

    private CinemachineComponentBase componentBase01;
    private CinemachineComponentBase componentBase02;

    private void Start()
    {
        float defaultYOffsetValue = 10f;

        if (componentBase01 == null)
        {
            componentBase01 = virtualCamera[0].GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase01 as CinemachineFramingTransposer).m_TrackedObjectOffset.y = defaultYOffsetValue;
        }

        if (componentBase02 == null)
        {
            componentBase02 = virtualCamera[1].GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase02 as CinemachineFramingTransposer).m_TrackedObjectOffset.y = defaultYOffsetValue;
        }

    }

    private void OnMouseDrag()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 _trackedObjectOffset = virtualCamera[0].GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;


        if (_trackedObjectOffset.y > 20f)
        {
            (componentBase01 as CinemachineFramingTransposer).m_TrackedObjectOffset.y = 20f;
            (componentBase02 as CinemachineFramingTransposer).m_TrackedObjectOffset.y = 20f;
        }

        if (_trackedObjectOffset.y <= 20f)
        {
            if (componentBase01 is CinemachineFramingTransposer)
            {
                (componentBase01 as CinemachineFramingTransposer).m_TrackedObjectOffset.y -= mouseY;
            }

            if (componentBase02 is CinemachineFramingTransposer)
            {
                (componentBase02 as CinemachineFramingTransposer).m_TrackedObjectOffset.y -= mouseY;
            }

            if (_trackedObjectOffset.y < 10f)
            {
                (componentBase01 as CinemachineFramingTransposer).m_TrackedObjectOffset.y = 10f;
                (componentBase02 as CinemachineFramingTransposer).m_TrackedObjectOffset.y = 10f;
            }

        }
    }
}
