using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Enviro
{

    [Serializable]
    public class EnviroZoneWeather
    {
        public bool showEditor;
        public EnviroWeatherType weatherType;
        public float probability;

    }
    [AddComponentMenu("Enviro 3/Weather Zone")]
    public class EnviroZone : MonoBehaviour
    {

        public EnviroWeatherType currentWeatherType;
        public EnviroWeatherType nextWeatherType;

        public bool autoWeatherChanges = true;
        public float weatherChangeIntervall = 2f;
        public double nextWeatherUpdate;

        public List<EnviroZoneWeather> weatherTypeList = new List<EnviroZoneWeather>();
        public Vector3 zoneScale = Vector3.one;
        public Color zoneGizmoColor;

        private BoxCollider zoneCollider;

        private Collider m_tempCollider;
        public float blendDistance = 0f;

        void Start()
        {
            zoneCollider = gameObject.AddComponent<BoxCollider>();
            zoneCollider.isTrigger = true;
            UpdateZoneScale ();
        }

        private void OnEnable()
        {
            m_tempCollider = GetComponent<Collider>();
        }

        public void UpdateZoneScale ()
        {
            //zoneCollider.size = zoneScale;
        }

        // Adds a new weather type to the zone.
        public void AddWeatherType(EnviroWeatherType wType)
        {
            EnviroZoneWeather weatherTypeEntry = new EnviroZoneWeather();
            weatherTypeEntry.weatherType = wType;
            weatherTypeList.Add(weatherTypeEntry);
        }

        // Removes a weather type from the zone.
        public void RemoveWeatherZoneType(EnviroZoneWeather wType)
        {
            weatherTypeList.Remove(wType);
        }

        // Changes the weather of the zone instantly.
        public void ChangeZoneWeatherInstant (EnviroWeatherType type)
        {
            if(EnviroManager.instance != null && currentWeatherType != type)
            {
                EnviroManager.instance.NotifyZoneWeatherChanged(type,this);
            }
            
            currentWeatherType = type;
        }

        // Changes the weather of the zone to the type for next weather update.
        public void ChangeZoneWeather (EnviroWeatherType type)
        {
            nextWeatherType = type;
        }

        private void ChooseNextWeatherRandom ()
        {
            float rand = UnityEngine.Random.Range(0f,100f);
            bool nextWeatherFound = false;

            for (int i = 0; i < weatherTypeList.Count; i++)
            {
                if(rand <= weatherTypeList[i].probability)
                {
                    ChangeZoneWeather(weatherTypeList[i].weatherType);
                    nextWeatherFound = true;
                    return;
                }
            }

            if(!nextWeatherFound)
               ChangeZoneWeather(currentWeatherType);
        }

        private void UpdateZoneWeather()
        {
            if(EnviroManager.instance.Time != null)
            {
               double currentDate = EnviroManager.instance.Time.GetDateInHours();

               if(currentDate >= nextWeatherUpdate)
               {
                 if(nextWeatherType != null)
                    ChangeZoneWeatherInstant(nextWeatherType);
                 else
                    ChangeZoneWeatherInstant(currentWeatherType);
                 
                 //Get next weather
                 //ChooseNextWeatherRandom ();
                 nextWeatherUpdate = currentDate + weatherChangeIntervall;
               }
            }
        }

        void Update()
        {
            if (EnviroManager.instance == null || EnviroManager.instance.Weather == null)
                return;

            if(autoWeatherChanges)
                UpdateZoneWeather();

            //Forces the weather change in Enviro when this zone is currently the active one.
            //if(EnviroManager.instance.Weather.currentZone == this && EnviroManager.instance.Weather.targetWeatherType != currentWeatherType)
               //EnviroManager.instance.Weather.ChangeWeather(currentWeatherType);
        }

        void OnTriggerEnter (Collider col)
        {
            if (EnviroManager.instance == null || EnviroManager.instance.Weather == null)
                return;

            //Change Weather to Zone Weather:
            if (col.gameObject.GetComponent<EnviroManager>())
            {
                //EnviroManager.instance.Weather.currentZone = this;
            }

            //EnviroManager.instance.Weather.ChangeWeather(currentWeatherType);

            EnviroManager.instance.NotifyWeatherChanged(currentWeatherType);
            Debug.Log(currentWeatherType);
        }

        void OnTriggerExit (Collider col)
        {
             if (EnviroManager.instance == null || EnviroManager.instance.Weather == null)
                 return;
        
             if(col.gameObject.GetComponent<EnviroManager>())
                EnviroManager.instance.Weather.currentZone = null;

             EnviroManager.instance.NotifyWeatherChanged(EnviroManager.instance.Weather.targetWeatherType);
             Debug.Log(EnviroManager.instance.Weather.targetWeatherType);
        }

        /*void OnDrawGizmos () 
        {
            Gizmos.color = zoneGizmoColor;
            
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawCube(Vector3.zero, new Vector3(zoneScale.x, zoneScale.y, zoneScale.z));
        }*/
        private void OnDrawGizmos()
        {
            m_tempCollider = GetComponent<Collider>();
            if (m_tempCollider == null)
                return;

            if (m_tempCollider.enabled)
            {
                var scale = transform.lossyScale;
                var invScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);

                // We'll just use scaling as an approximation for volume skin. It's far from being
                // correct (and is completely wrong in some cases). Ultimately we'd use a distance
                // field or at least a tesselate + push modifier on the collider's mesh to get a
                // better approximation, but the current Gizmoz system is a bit limited and because
                // everything is dynamic in Unity and can be changed at anytime, it's hard to keep
                // track of changes in an elegant way (which we'd need to implement a nice cache
                // system for generated volume meshes).
                var type = m_tempCollider.GetType();
                if (type == typeof(BoxCollider))
                {
                    var c = (BoxCollider)m_tempCollider;
                    Gizmos.color = zoneGizmoColor;
                    Gizmos.DrawCube(c.center, c.size);
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(c.center, c.size + invScale * blendDistance * 4f);
                }
                else if (type == typeof(SphereCollider))
                {
                    var c = (SphereCollider)m_tempCollider;
                    Gizmos.DrawSphere(c.center, c.radius);
                    Gizmos.DrawWireSphere(c.center, c.radius + invScale.x * blendDistance * 2f);
                }
                else if (type == typeof(MeshCollider))
                {
                    var c = (MeshCollider)m_tempCollider;

                    // Only convex mesh collider are allowed
                    if (!c.convex)
                        c.convex = true;

                    // Mesh pivot should be centered or this won't work
                    Gizmos.DrawMesh(c.sharedMesh);
                    Gizmos.DrawWireMesh(c.sharedMesh, Vector3.zero, Quaternion.identity,
                        Vector3.one + invScale * blendDistance * 4f);
                }
            }

            m_tempCollider = null;
        }
    }
}
