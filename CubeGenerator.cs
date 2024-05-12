using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefub;

    private int _minCubeCount;
    private int _maxCubeCount;
    private float _reductionFactor;
    private float _reductionSeparateFactor;
    private float _initCubeSeparateChance;
    private float _initExplosionRadius;
    private float _initExplosionForce;

    private void Start()
    {
        _initCubeSeparateChance = 100;
        _initExplosionRadius = 5;
        _initExplosionForce = 250;
        _minCubeCount = 2;
        _maxCubeCount = 7;
        _reductionFactor = 0.5f;
        _reductionSeparateFactor = 2f;

        GenerateCubes();
    }

    private void GenerateCubes(Cube cube)
    {
        int cubeCount = UnityEngine.Random.Range(_minCubeCount, _maxCubeCount);

        for (int i = 0; i < cubeCount; i++)
        {
            Cube newCube = Instantiate(_cubePrefub, cube.transform.position, Quaternion.identity);
            newCube.SetParentProperties(cube.ChanceToSeparate / _reductionSeparateFactor, cube.ExplosionRadius * _reductionSeparateFactor, cube.ExplosionForce * _reductionSeparateFactor);
            newCube.transform.localScale = Vector3.Scale(cube.transform.localScale, new Vector3(_reductionFactor, _reductionFactor, _reductionFactor));
            newCube.CubeSeparated += GenerateCubes;
        }
    }

    private void GenerateCubes()
    {
        Vector3[] startCubePositions = { new Vector3(-1.950885f, 1.5f, -3.99027f), new Vector3(1.245849f, 1.5f, -3.99027f) };

        foreach (var cubePosition in startCubePositions)
        {
            Cube cube = Instantiate(_cubePrefub, cubePosition, Quaternion.identity);
            cube.SetParentProperties(_initCubeSeparateChance, _initExplosionRadius, _initExplosionForce);
            cube.CubeSeparated += GenerateCubes;
        }
    }
}