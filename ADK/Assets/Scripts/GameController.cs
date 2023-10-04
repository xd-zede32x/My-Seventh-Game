using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField] private Color[] _bGColor;
    [SerializeField] private Transform _cubeToPlace;
    [SerializeField] private GameObject[] _canvasStartPage;
    [SerializeField] private float _ñubeReplacementRate = 0.5f;
    [SerializeField] private GameObject _cubeToCreate, _allCubes;

    private Color _toCameraColor;
    private Transform _mainCamera;
    private Rigidbody _allCubesRb;
    private Coroutine _showCubePlase;
    private bool _isLose, _firstCube;
    private int _prevCountMaxHorizontal;
    private float _cameraMoveToYPosition, _cameraMoveSpeed = 2f;
    private CubePosition _newCube = new CubePosition(0, 1, 0);

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

    private void Start()
    {
        _toCameraColor = Camera.main.backgroundColor;
        _mainCamera = Camera.main.transform;
        _cameraMoveToYPosition = 10f + _newCube.Y - 1f;

        _allCubesRb = _allCubes.GetComponent<Rigidbody>();
        _showCubePlase = StartCoroutine(ShowCubePlase());
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && _cubeToPlace != null && _allCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif

            if (!_firstCube)
            {
                _firstCube = true;
                foreach (GameObject obj in _canvasStartPage)
                {
                    Destroy(obj);
                }
            }

            GameObject _newCubes = Instantiate(_cubeToCreate, _cubeToPlace.position, Quaternion.identity) as GameObject;
            _newCubes.transform.SetParent(_allCubes.transform);
            _newCube.VectorAcceptance(_cubeToPlace.position);
            allCubesPosition.Add(_newCube.VectorReturn());

            _allCubesRb.isKinematic = true;
            _allCubesRb.isKinematic = false;

            SpawnPositions();
            MoveCameraChaneBg();
        }

        if (!_isLose && _allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(_cubeToPlace.gameObject);
            _isLose = true;
            StopCoroutine(_showCubePlase);
        }

        _mainCamera.localPosition = Vector3.MoveTowards(_mainCamera.localPosition, new Vector3(_mainCamera.localPosition.x, _cameraMoveToYPosition, _mainCamera.localPosition.z), _cameraMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != _toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, _toCameraColor, Time.deltaTime / 1);
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
        #region SpawnCubs
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

        if (positions.Count > 1)
        {
            _cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        }

        else if (positions.Count == 0)
        {
            _isLose = true;
        }

        else
        {
            _cubeToPlace.position = positions[0];
        }
        #endregion
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

    private void MoveCameraChaneBg()
    {
        int _maxX = 0, _maxY = 0, _maxZ = 0, maxHorizontal;

        foreach (Vector3 position in allCubesPosition)
        {
            if (Mathf.Abs(Convert.ToInt32(position.x)) > _maxX)
            {
                _maxX = Convert.ToInt32(position.x);
            }

            if (Convert.ToInt32(position.y) > _maxY)
            {
                _maxY = Convert.ToInt32(position.y);
            }

            if (Mathf.Abs(Convert.ToInt32(position.z)) > _maxZ)
            {
                _maxZ = Convert.ToInt32(position.z);
            }

            _cameraMoveToYPosition = 10f + _newCube.Y - 1f;
            maxHorizontal = _maxX > _maxZ ? _maxX : _maxZ;

            if (maxHorizontal % 3 == 0 && _prevCountMaxHorizontal != maxHorizontal)
            {
                _mainCamera.localPosition -= new Vector3(0, 0, 0.2f);
                _prevCountMaxHorizontal = maxHorizontal;
            }
        }

        if (_maxY >= 10)
        {
            _toCameraColor = _bGColor[2];
        }

        else if (_maxY >= 5)
        {
            _toCameraColor = _bGColor[1];
        }

        else if (_maxY >= 2)
        {
            _toCameraColor = _bGColor[0];
        }
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