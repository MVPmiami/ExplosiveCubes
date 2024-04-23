using System;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefub;

    private int _minCubeCount;
    private int _maxCubeCount;

    private void Start()
    {
        _minCubeCount = 2;
        _maxCubeCount = 7;

        Cube.SeparateToCubes += GenerateCubes;
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
        }
    }

    private void OnDestroy()
    {
        Cube.SeparateToCubes -= GenerateCubes;
    }

    public void GenerateCubes(Cube cube)
    {
        int cubeCount = UnityEngine.Random.Range(_minCubeCount, _maxCubeCount);
        Vector3 size = new Vector3(cube.transform.localScale.x / 2f, cube.transform.localScale.y / 2f, cube.transform.localScale.z / 2f);

        for (int i = 0; i < cubeCount; i++)
        {
            Cube newCubeObject = Instantiate(_cubePrefub, cube.transform.position, Quaternion.identity);
            Cube newCube = newCubeObject.GetComponent<Cube>();
            newCube.SetChanceToSeparate(cube.ChanceToSeparate / 2f);
            newCube.SetID(cube.ID);
            newCubeObject.transform.localScale = size;
        }
    }
}
