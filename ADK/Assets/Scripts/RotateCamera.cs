using UnityEngine;
public class RotateCamera : MonoBehaviour
{
    [Range(0,15)]
    [SerializeField] private float _rotationSpeed = 5f;
    private Transform _rotationsY;

    private void Start()
    {
        _rotationsY = GetComponent<Transform>();
    }

    private void Update()
    {      
        _rotationsY.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
}