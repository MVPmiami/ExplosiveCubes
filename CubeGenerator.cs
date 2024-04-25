using System;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefub;

    private int _minCubeCount;
    private int _maxCubeCount;
    private float _reductionFactor;
    private float _reductionSeparateFactor;

    private void Start()
    {
        _minCubeCount = 2;
        _maxCubeCount = 7;
        _reductionFactor = 0.5f;
        _reductionSeparateFactor = 2f;

        InitCubes();
    }

    public void GenerateCubes(Cube cube)
    {
        int cubeCount = UnityEngine.Random.Range(_minCubeCount, _maxCubeCount);

        for (int i = 0; i < cubeCount; i++)
        {
            Cube newCube = Instantiate(_cubePrefub, cube.transform.position, Quaternion.identity);
            newCube.SetParentParams(cube.ID, cube.ChanceToSeparate / _reductionSeparateFactor, this);
            newCube.transform.localScale = Vector3.Scale(cube.transform.localScale, new Vector3(_reductionFactor, _reductionFactor, _reductionFactor));
        }
    }

    private void InitCubes()
    {
        Vector3[] startCubePositions = { new Vector3(-1.950885f, 1.5f, -3.99027f), new Vector3(1.245849f, 1.5f, -3.99027f) };

        foreach (var cubePosition in startCubePositions)
        {
            Cube cube = Instantiate(_cubePrefub, cubePosition, Quaternion.identity);
            cube.SetParentParams(Guid.NewGuid(), 100f, this);
        }
    }
}
