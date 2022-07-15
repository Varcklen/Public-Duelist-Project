using UnityEngine;
using Project.Singleton.ConfigurationParticleNS;
using System.Linq;

namespace Project.Particles
{
    /// <summary>
    /// The class is the base class for all particles in the game.
    /// </summary>
    public class Particle : MonoBehaviour
    {
        public static ParticleSystem Create(ParticleType particleType, Vector3 position)
        {
            var gameObject = ConfigurationParticle.Instance.Particles.FirstOrDefault(x => x.ParticleType == particleType).Particle;
            GameObject particleObject = Instantiate(gameObject, position, Quaternion.identity);
            var particleSystem = particleObject.GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(particleObject, particleSystem.main.duration);
            return particleSystem;
        }

        public static ParticleSystem Create(ParticleType particleType, Transform position)
        {
            return Create(particleType, position.position);
        }
    }

    public enum ParticleType
    {
        Explode
    }
}