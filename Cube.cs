using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class Cube : MonoBehaviour
{
    private const string Color = "_Color";

    [SerializeField] private ParticleSystem _effect;

    private float _explosionRadius;
    private float _explosionForce;
    private Renderer _renderer;
    private Color _color;
    private float _chanceToSeparate;
    private int _minChancePercent;
    private int _maxChancePercent;
    private Guid _id;

    public float ChanceToSeparate => _chanceToSeparate;
    public Guid ID => _id;
    public event Action<Cube> CubeExploded;
    public float ExplosionRadius => _explosionRadius;
    public float ExplosionForce => _explosionForce;

    private void Start()
    {
        _minChancePercent = 0;
        _maxChancePercent = 100;
        _renderer = GetComponent<Renderer>();
        _color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        _renderer.material.SetColor(Color, _color);
    }

    private void OnMouseUpAsButton()
    {
        if (GetExplodeStatus())
        {
            Instantiate(_effect, transform.position, transform.rotation);
            Destroy(gameObject);
            Explode();
        }
        else
        {
            CubeExploded?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void SetParentParams(Guid id, float chanceToSeparate,float explosionRadius, float explosionForce)
    {
        _chanceToSeparate = chanceToSeparate;
        _id = id;
        _explosionRadius = explosionRadius;
        _explosionForce = explosionForce;
    }

    private bool GetExplodeStatus()
    {
        int randomChance = UnityEngine.Random.Range(_minChancePercent, _maxChancePercent);

        return randomChance > _chanceToSeparate;
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