using System;
using UnityEngine;

[RequireComponent(typeof(Cube), typeof(Explode))]
public class Cube : MonoBehaviour
{
    private const string Color = "_Color";

    [SerializeField] private ParticleSystem _effect;

    private Explode _explode;
    private Renderer _renderer;
    private Color _color;
    private float _explosionRadius;
    private float _explosionForce;
    private float _chanceToSeparate;
    private int _minChancePercent;
    private int _maxChancePercent;

    public event Action<Cube> CubeSeparated;
    public float ChanceToSeparate => _chanceToSeparate;
    public float ExplosionRadius => _explosionRadius;
    public float ExplosionForce => _explosionForce;

    private void Start()
    {
        _explode = GetComponent<Explode>();
        _minChancePercent = 0;
        _maxChancePercent = 100;
        _renderer = GetComponent<Renderer>();
        _color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        _renderer.material.SetColor(Color, _color);
    }

    private void OnMouseUpAsButton()
    {
        if (EvaluateExplosivity())
        {
            Instantiate(_effect, transform.position, transform.rotation);
            _explode?.InitExplode(this);
        }
        else
        {
            CubeSeparated?.Invoke(this);
        }

        Destroy(gameObject);
    }

    public void SetParentProperties(float chanceToSeparate, float explosionRadius, float explosionForce)
    {
        _chanceToSeparate = chanceToSeparate;
        _explosionRadius = explosionRadius;
        _explosionForce = explosionForce;
    }

    private bool EvaluateExplosivity()
    {
        int randomChance = UnityEngine.Random.Range(_minChancePercent, _maxChancePercent);

        return randomChance > _chanceToSeparate;
    }
}