using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class Cube : MonoBehaviour
{
    private const string Color = "_Color";

    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _effect;

    private Renderer renderer;
    private Color _color;
    private float _chanceToSeparate;
    private int _minChancePercent;
    private int _maxChancePercent;
    private Guid _id;
    private CubeGenerator _cubeGenerator;

    public float ChanceToSeparate => _chanceToSeparate;
    public Guid ID => _id;

    private void Start()
    {
        _minChancePercent = 0;
        _maxChancePercent = 100;
        renderer = GetComponent<Renderer>();
        _color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        renderer.material.SetColor(Color, _color);
    }

    private void OnMouseUpAsButton()
    {
        if (GetExplodeStatus())
        {
            _cubeGenerator.GenerateCubes(this);
            Instantiate(_effect, transform.position, transform.rotation);
            Destroy(gameObject);
            Explode();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetParentParams(Guid id, float chanceToSeparate, CubeGenerator cubeGenerator)
    {
        _chanceToSeparate = chanceToSeparate;
        _id = id;
        _cubeGenerator = cubeGenerator;
    }

    private bool GetExplodeStatus()
    {
        int randomChance = UnityEngine.Random.Range(_minChancePercent, _maxChancePercent);

        return randomChance < _chanceToSeparate;
    }

    private void Explode()
    {
        foreach (Rigidbody cube in GetExplodableObjects())
            cube.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        List<Rigidbody> cubes = new List<Rigidbody>();

        foreach (Collider hit in hits)
        {
            Rigidbody rigidbody = hit.attachedRigidbody;

            if (rigidbody != null)
            {
                Cube cube = rigidbody.GetComponent<Cube>();

                if (cube != null && cube.ID == _id)
                    cubes.Add(rigidbody);
            }
        }

        return cubes;
    }
}
