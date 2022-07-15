using UnityEngine;
using Project.Singleton.ScriptableObjects;
using System;
using System.Collections.Generic;
using Project.Particles;

namespace Project.Singleton.ConfigurationParticleNS
{
	[CreateAssetMenu(fileName ="ConfigurationParticle", menuName = "Scriptable Objects/Singleton/ConfigurationParticle")]
	public class ConfigurationParticle : SingletonScriptableObject<ConfigurationParticle>
	{
		[SerializeField] private List<ParticleSource> _particles;
		public List<ParticleSource> Particles => _particles;
	}

    [Serializable]
	public struct ParticleSource
    {
		[SerializeField] private GameObject _particle;
		public GameObject Particle => _particle;
		[SerializeField] private ParticleType _particleType;
		public readonly ParticleType ParticleType => _particleType;
	}
}