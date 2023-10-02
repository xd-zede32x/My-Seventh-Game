using System;
using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private CubePosition newCube = new CubePosition(0, 1, 0);
    [SerializeField] private float _ñubeReplacementRate = 0.5f;

    private void Start()
    {
        StartCoroutine(ShowCubePlase());
    }

    IEnumerator ShowCubePlase()
    {
        while (true)
        {
            SpaenPositions();
            yield return new WaitForSeconds(_ñubeReplacementRate);
        }
    }

    private void SpaenPositions()
    {

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

    public Vector3 VectorReturn()
    {
        while (true)
        {
            return new Vector3(X, Y, Z);
        }
    }

    public void VectorAcceptance(Vector3 position)
    {
        X = Convert.ToInt32(position.x);
        Y = Convert.ToInt32(position.y);
        Z = Convert.ToInt32(position.z);
    }
}