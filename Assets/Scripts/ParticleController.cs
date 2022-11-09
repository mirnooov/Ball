using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private void OnEnable()
    {
        StartCoroutine(PauseBeforeDestroy());
    }

    public void PlayParticles()
    {
        _particleSystem.Play();
    }

    private IEnumerator PauseBeforeDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
