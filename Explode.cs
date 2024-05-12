using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public void InitExplode(Cube explosiveCube)
    {
        List<Rigidbody> cubes = GetExplodableObjects(explosiveCube);

        foreach (Rigidbody cube in cubes)
            cube.AddExplosionForce(explosiveCube.ExplosionForce, transform.position, explosiveCube.ExplosionRadius);
    }

    private List<Rigidbody> GetExplodableObjects(Cube explosiveCube)
    {
        Collider[] hits = Physics.OverlapSphere(explosiveCube.transform.position, explosiveCube.ExplosionRadius);
        List<Rigidbody> cubes = new List<Rigidbody>();
        
        foreach (Collider hit in hits)
        {
            Rigidbody rigidbody = hit.attachedRigidbody;

            if (rigidbody != null)
            {
                if (rigidbody.TryGetComponent(out Cube cube))
                    cubes.Add(rigidbody);
            }
        }

        return cubes;
    }
}