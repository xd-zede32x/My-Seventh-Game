using UnityEngine;
public class CameraShake : MonoBehaviour
{
    private Vector3 _origrnPosition;
    private Transform _cameraTransform;
    private float _shakeDurection = 1f, _shakeAmount = 0.3f, _decreaseFactor = 1.5f;

    private void Start()
    {
        _cameraTransform = GetComponent<Transform>();
        _origrnPosition = _cameraTransform.localPosition;
    }

    private void Update()
    {
        if (_shakeDurection > 0)
        {
            _cameraTransform.localPosition = _origrnPosition + Random.insideUnitSphere * _shakeAmount;
            _shakeDurection -= Time.deltaTime * _decreaseFactor;
        }

        else
        {
            _shakeDurection = 0;
            _cameraTransform.localPosition = _origrnPosition;
        }
    } 
}