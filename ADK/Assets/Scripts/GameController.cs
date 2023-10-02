using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private CubePosition _newCube = new CubePosition(0, 1, 0);
    [SerializeField] private float _ñubeReplacementRate = 0.5f;
    [SerializeField] private Transform _cubeToPlace;
    [SerializeField] private GameObject _cubeToCreate, _allCubes;
    private Rigidbody _allCubesRb;
    private bool _isLose;

    private List<Vector3> allCubesPosition = new List<Vector3>
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
    };

    private Coroutine _showCubePlase;

    private void Start()
    {
        _allCubesRb = _allCubes.GetComponent<Rigidbody>();
        _showCubePlase = StartCoroutine(ShowCubePlase());
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && _cubeToPlace != null)
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                return;
#endif
            GameObject newCubes = Instantiate(_cubeToCreate,_cubeToPlace.position, Quaternion.identity) as GameObject;
            newCubes.transform.SetParent(_allCubes.transform);
            _newCube.VectorAcceptance(_cubeToPlace.position);
            allCubesPosition.Add(_newCube.VectorReturn());

            _allCubesRb.isKinematic = true;
            _allCubesRb.isKinematic = false;
            SpawnPositions();
        }

        if (!_isLose && _allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(_cubeToPlace.gameObject);
            _isLose = true;
            StopCoroutine(_showCubePlase); 
        }
    }

    IEnumerator ShowCubePlase()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(_ñubeReplacementRate);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(_newCube.X + 1, _newCube.Y, _newCube.Z)) && _newCube.X + 1 != _cubeToPlace.position.x)
        {
            positions.Add(new Vector3(_newCube.X + 1, _newCube.Y, _newCube.Z));
        }

        if (IsPositionEmpty(new Vector3(_newCube.X - 1, _newCube.Y, _newCube.Z)) && _newCube.X - 1 != _cubeToPlace.position.x)
        {
            positions.Add(new Vector3(_newCube.X - 1, _newCube.Y, _newCube.Z));
        }

        if (IsPositionEmpty(new Vector3(_newCube.X, _newCube.Y + 1, _newCube.Z)) && _newCube.Y + 1 != _cubeToPlace.position.y)
        {
            positions.Add(new Vector3(_newCube.X, _newCube.Y + 1, _newCube.Z));
        }

        if (IsPositionEmpty(new Vector3(_newCube.X, _newCube.Y - 1, _newCube.Z)) && _newCube.Y - 1 != _cubeToPlace.position.y)
        {
            positions.Add(new Vector3(_newCube.X, _newCube.Y - 1, _newCube.Z));
        }

        if (IsPositionEmpty(new Vector3(_newCube.X, _newCube.Y, _newCube.Z + 1)) && _newCube.Z + 1 != _cubeToPlace.position.z)
        {
            positions.Add(new Vector3(_newCube.X, _newCube.Y, _newCube.Z + 1));
        }

        if (IsPositionEmpty(new Vector3(_newCube.X, _newCube.Y, _newCube.Z - 1)) && _newCube.Z - 1 != _cubeToPlace.position.z)
        {
            positions.Add(new Vector3(_newCube.X, _newCube.Y, _newCube.Z - 1));
        }

        _cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
    }

    private bool IsPositionEmpty(Vector3 targetPosition)
    {
        if (targetPosition.y == 0)
        {
            return false;
        }

        foreach (Vector3 position in allCubesPosition)
        {
            if (position.x == targetPosition.x && position.y == targetPosition.y && position.z == targetPosition.z)
            {
                return false;
            }
        }
        return true;
    }
}

struct CubePosition
{
    public int X, Y, Z;

    public CubePosition(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 VectorReturn() // get
    {
        while (true)
        {
            return new Vector3(X, Y, Z);
        }
    }

    public void VectorAcceptance(Vector3 position) // set
    {
        X = Convert.ToInt32(position.x);
        Y = Convert.ToInt32(position.y);
        Z = Convert.ToInt32(position.z);
    }
}