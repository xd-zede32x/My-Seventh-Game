using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Color[] _bGColor;
    [SerializeField] private Transform _cubeToPlace;
    [SerializeField] private GameObject _allCubes, _vfx;
    [SerializeField] private GameObject[] _cubesToCreate;
    [SerializeField] private GameObject[] _canvasStartPage;
    [SerializeField] private float _ñubeReplacementRate = 0.5f;

    private Color _toCameraColor;
    private Transform _mainCamera;
    private Rigidbody _allCubesRb;
    private Coroutine _showCubePlase;
    private bool _isLose, _firstCube;
    private int _prevCountMaxHorizontal;
    private CubePosition _newCube = new CubePosition(0, 1, 0);
    private float _cameraMoveToYPosition, _cameraMoveSpeed = 2f;
    private List<GameObject> _posibleCubeToCreate = new List<GameObject>();

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
        #region ShopCubes
        if (PlayerPrefs.GetInt("Score") < 5)
        {
            _posibleCubeToCreate.Add(_cubesToCreate[0]);
        }

        else if (PlayerPrefs.GetInt("Score") < 10)
        {
            AddPosibleCubes(2);
        }

        else if (PlayerPrefs.GetInt("Score") < 15)
        {
            AddPosibleCubes(3);
        }

        else if (PlayerPrefs.GetInt("Score") < 20)
        {
            AddPosibleCubes(4);
        }

        else if (PlayerPrefs.GetInt("Score") < 25)
        {
            AddPosibleCubes(5);
        }

        else if (PlayerPrefs.GetInt("Score") < 30)
        {
            AddPosibleCubes(6);
        }

        else if (PlayerPrefs.GetInt("Score") < 35)
        {
            AddPosibleCubes(7);
        }

        else if (PlayerPrefs.GetInt("Score") < 40)
        {
            AddPosibleCubes(8);
        }

        else if (PlayerPrefs.GetInt("Score") < 50)
        {
            AddPosibleCubes(9);
        }

        else
        {
            AddPosibleCubes(10);
        }

        #endregion
        _scoreText.text = "<size=50>Best:</size> " + PlayerPrefs.GetInt("Score") + " <size=40>Score:</size>0\r\n\r\n";

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

            GameObject _createCube = null;

            if (_posibleCubeToCreate.Count == 1)
            { 
                _createCube = _posibleCubeToCreate[0];
            }

            else  
            {
                _createCube = _posibleCubeToCreate[UnityEngine.Random.Range(0, _posibleCubeToCreate.Count)];
            }
                   

            GameObject _newCubes = Instantiate(_createCube, _cubeToPlace.position, Quaternion.identity) as GameObject;
            _newCubes.transform.SetParent(_allCubes.transform);
            _newCube.VectorAcceptance(_cubeToPlace.position);
            allCubesPosition.Add(_newCube.VectorReturn());

            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            GameObject _newVfx = Instantiate(_vfx, _cubeToPlace.transform.position, Quaternion.identity) as GameObject;
            Destroy(_newVfx, 2f);

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
        }

        _maxY--;
        if (PlayerPrefs.GetInt("Score") < _maxY)
        {
            PlayerPrefs.SetInt("Score", _maxY);
        }

        _scoreText.text = "<size=50>Best:</size> " + PlayerPrefs.GetInt("Score") + " <size=40>Score:</size> " + _maxY + " \r\n\r\n";

        _cameraMoveToYPosition = 10f + _newCube.Y - 1f;
        maxHorizontal = _maxX > _maxZ ? _maxX : _maxZ;

        if (maxHorizontal % 3 == 0 && _prevCountMaxHorizontal != maxHorizontal)
        {
            _mainCamera.localPosition -= new Vector3(0, 0, 0.2f);
            _prevCountMaxHorizontal = maxHorizontal;
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

    private void AddPosibleCubes(int till)
    {
        for (int i = 0; i < till; i++)
        {
            _posibleCubeToCreate.Add(_cubesToCreate[i]);
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