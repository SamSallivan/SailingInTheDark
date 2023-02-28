using UnityEngine;
using Crest;
using Enviro;

namespace DeveloperTools
{
    /// <summary>
    /// Synchronize Enviro weather with Crest ocean gerstner waves. 
    /// Change the wave weight depending on the weather.
    /// </summary>
    public class EnviroWeatherCrestSync : MonoBehaviour
    {
        public ShapeGerstner waveGen;
        public ShapeFFT waveGen2;

        private float currentWeightTarget;

        public float calmWeight = 0.1f;
        public float mediumWeight = 0.3f;
        public float hardWeight = 0.6f;
        public float stormWeight = 1f;

        public float changeSpeed = 1f;

        void Start()
        {
            EnviroManager.instance.OnWeatherChanged += (EnviroWeatherType type) =>
            {
                DoOnWeatherChange(type);
            };
        }

        void DoOnWeatherChange(EnviroWeatherType type)
        {
            if (type.name == "Clear Sky")
            {
                currentWeightTarget = calmWeight;
            }
            else if (type.name == "Cloudy 1" || type.name == "Cloudy 2" || type.name == "Cloudy 3" || type.name == "Foggy")
            {
                currentWeightTarget = mediumWeight;
            }
            else if (type.name == "Rain" || type.name == "Snow")
            {
                currentWeightTarget = hardWeight;
            }
            else if (type.name == "Storm")
            {
                currentWeightTarget = stormWeight;
            }

        }

        void Update()
        {
            if(waveGen != null)
                waveGen._weight = Mathf.MoveTowards(waveGen._weight, currentWeightTarget, changeSpeed * Time.deltaTime);
            if (waveGen2 != null)
                waveGen2._weight = Mathf.MoveTowards(waveGen2._weight, currentWeightTarget, changeSpeed * Time.deltaTime);
        }
    }
}