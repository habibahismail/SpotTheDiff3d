using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] virtualCamera = new CinemachineVirtualCamera[2];
    [SerializeField] float sensitivity = 10f;

    [SerializeField] private IslandSizeEventChannelSO islandSizeChangeEventChannel;

    private CinemachineComponentBase componentBase01;
    private CinemachineComponentBase componentBase02;
    private float cameraDistance;
    private float startingCameraDistance = 35f;
    private readonly float maxCameraDistance = 50f;
    private readonly float minCameraDistance = 2f;

    private void Awake()
    {
        islandSizeChangeEventChannel.OnEventRaised += UpdateNewCameraDistance;
    }

    private void Start()
    {
        startingCameraDistance = 35f;

        if (componentBase01 == null)
        {
            componentBase01 = virtualCamera[0].GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase01 as CinemachineFramingTransposer).m_CameraDistance = startingCameraDistance;
        }

        if (componentBase02 == null)
        {
            componentBase02 = virtualCamera[1].GetCinemachineComponent(CinemachineCore.Stage.Body);
            (componentBase02 as CinemachineFramingTransposer).m_CameraDistance = startingCameraDistance;
        }

    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;

            float _cameraDistance = virtualCamera[0].GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;

            if (_cameraDistance > maxCameraDistance)
            {
                UpdateCameraDistanceValue(maxCameraDistance);
            }

            if ( _cameraDistance <= maxCameraDistance )
            {
                if (componentBase01 is CinemachineFramingTransposer)
                {
                    (componentBase01 as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
                }

                if (componentBase02 is CinemachineFramingTransposer)
                {
                    (componentBase02 as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
                }

                if (_cameraDistance < minCameraDistance)
                {
                    UpdateCameraDistanceValue(minCameraDistance);
                }

            }
        }

    }

    private void UpdateCameraDistanceValue(float newValue)
    {
        (componentBase01 as CinemachineFramingTransposer).m_CameraDistance = newValue;
        (componentBase02 as CinemachineFramingTransposer).m_CameraDistance = newValue;
    }

    private void UpdateNewCameraDistance(SIZE islandSize)
    {
        float _cameraDistance = 35f;

        switch ((int)islandSize)
        {
            case 0: _cameraDistance = 50f; break;
            case 1: _cameraDistance = 35f; break;
            case 2: _cameraDistance = 20f; break;
            default: break;

        }

        UpdateCameraDistanceValue(_cameraDistance);
    }

}
