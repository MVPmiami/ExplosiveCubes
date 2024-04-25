using System;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefub;

    private int _minCubeCount;
    private int _maxCubeCount;
    private float _reductionFactor;

    private void Start()
    {
        _minCubeCount = 2;
        _maxCubeCount = 7;
        _reductionFactor = 0.5f;

        InitGenerateCubes();
    }

    private void InitGenerateCubes()
    {
        Vector3[] startCubePositions = { new Vector3(-1.950885f, 1.5f, -3.99027f), new Vector3(1.245849f, 1.5f, -3.99027f) };

        foreach (var cubePosition in startCubePositions)
        {
            Cube cube = Instantiate(_cubePrefub, cubePosition, Quaternion.identity);
            cube.SetChanceToSeparate(100f);
            cube.SetID(Guid.NewGuid());
            cube.SetCubeGenerator(this);
        }
    }

    public void GenerateCubes(Cube cube)
    {
        int cubeCount = UnityEngine.Random.Range(_minCubeCount, _maxCubeCount);
        
        for (int i = 0; i < cubeCount; i++)
        {
            Cube newCubeObject = Instantiate(_cubePrefub, cube.transform.position, Quaternion.identity);
            Cube newCube = newCubeObject.GetComponent<Cube>();
            newCube.SetChanceToSeparate(cube.ChanceToSeparate / _reductionFactor);
            newCube.SetID(cube.ID);
            newCube.SetCubeGenerator(this);
            newCube.transform.localScale = Vector3.Scale(cube.transform.localScale, new Vector3(_reductionFactor, _reductionFactor, _reductionFactor));
        }
    }
}
