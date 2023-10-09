using UnityEngine;
public class IsEnabled : MonoBehaviour
{
    [SerializeField] private int _needToUnlock;
    [SerializeField] private Material _oceanicMaterial;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Score") < _needToUnlock)
        {
            GetComponent<MeshRenderer>().material = _oceanicMaterial;
        }
    }
}