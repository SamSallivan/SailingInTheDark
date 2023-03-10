using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Enviro
{
    [Serializable]
    public class EnviroWeather 
    {
        public List<EnviroWeatherType> weatherTypes = new List<EnviroWeatherType>();
        public float cloudsTransitionSpeed = 1f;
        public float fogTransitionSpeed = 1f;
        public float lightingTransitionSpeed = 1f;
        public float effectsTransitionSpeed = 1f;
        public float auroraTransitionSpeed = 1f;
        public float environmentTransitionSpeed = 1f;
        public float audioTransitionSpeed = 1f;
    }

    [Serializable]
    [ExecuteInEditMode]
    public class EnviroWeatherModule : EnviroModule
    {
        public Enviro.EnviroWeather Settings;
        public EnviroWeatherModule preset;
        public EnviroWeatherType targetWeatherType;
        public EnviroZone currentZone;

        private bool instantTransition = false;

        public BoxCollider triggerCollider;
        public Rigidbody triggerRB;

        public override void Enable()
        {
            if (EnviroManager.instance == null)
                return;

            if (targetWeatherType == null && Settings.weatherTypes.Count > 0)
                targetWeatherType = Settings.weatherTypes[0];

            Setup();
        }

        public override void Disable()
        {
            if (EnviroManager.instance == null)
                return;

            Cleanup();
        }

        private void Setup()
        {
            if (EnviroManager.instance.gameObject.GetComponent<BoxCollider>() == null)
                triggerCollider = EnviroManager.instance.gameObject.AddComponent<BoxCollider>();
            else
                triggerCollider = EnviroManager.instance.gameObject.GetComponent<BoxCollider>();

            triggerCollider.isTrigger = true;
            triggerCollider.size = new Vector3(0.1f, 0.1f, 0.1f);

            if (EnviroManager.instance.gameObject.GetComponent<Rigidbody>() == null)
                triggerRB = EnviroManager.instance.gameObject.AddComponent<Rigidbody>();
            else
                triggerRB = EnviroManager.instance.gameObject.GetComponent<Rigidbody>();

            triggerRB.isKinematic = true;
        }

        private void Cleanup()
        {
            if (triggerCollider != null)
                DestroyImmediate(triggerCollider);

            if (triggerRB != null)
                DestroyImmediate(triggerRB);
        }

        /// Adds weather type to the list or creates a new one.
        public void CreateNewWeatherType()
        {
            EnviroWeatherType type = EnviroWeatherTypeCreation.CreateMyAsset();
            Settings.weatherTypes.Add(type);
        }

        /// Removes the weather type from the list.
        public void RemoveWeatherType(EnviroWeatherType type)
        {
            Settings.weatherTypes.Remove(type);
        }

        //Cleans the list from null entries.
        public void CleanupList()
        {
            for (int i = 0; i < Settings.weatherTypes.Count; i++)
            {
                if (Settings.weatherTypes[i] == null)
                    Settings.weatherTypes.RemoveAt(i);
            }
        }

        private IEnumerator InstantTransition()
        {
            yield return null;
            instantTransition = false;
        }

        // Update Method
        public override void UpdateModule()
        {
            if (EnviroManager.instance == null)
                return;

            if (currentZone == null)
            {
                //Instant changes when not playing or instant change is triggered
                if (!Application.isPlaying || instantTransition)
                {
                    if (targetWeatherType != null)
                    {
                        BlendVolumetricCloudsOverride(1f);
                        BlendFlatCloudsOverride(1f);
                        BlendLightingOverride(1f);
                        BlendEffectsOverride(1f);
                        BlendAuroraOverride(1f);
                        BlendFogOverride(1f);
                        BlendAudioOverride(1f);
                        BlendEnvironmentOverride(1f);
                        BlendLightningOverride(1f);
                    }

                    if (instantTransition)
                        instantTransition = false;
                }
                else
                {
                    if (targetWeatherType != null)
                    {
                        BlendVolumetricCloudsOverride(Settings.cloudsTransitionSpeed * Time.deltaTime);
                        BlendFlatCloudsOverride(Settings.cloudsTransitionSpeed * Time.deltaTime);
                        BlendLightingOverride(Settings.lightingTransitionSpeed * Time.deltaTime);
                        BlendEffectsOverride(Settings.effectsTransitionSpeed * Time.deltaTime);
                        BlendAuroraOverride(Settings.auroraTransitionSpeed * Time.deltaTime);
                        BlendFogOverride(Settings.fogTransitionSpeed * Time.deltaTime);
                        BlendAudioOverride(Settings.audioTransitionSpeed * Time.deltaTime);
                        BlendEnvironmentOverride(Settings.environmentTransitionSpeed * Time.deltaTime);
                        BlendLightningOverride(1f);
                    }
                }
            }
            CalculateWeatherZones();
        }

        private void BlendLightingOverride(float blendTime)
        {
            EnviroLightingModule lighting = EnviroManager.instance.Lighting;

            if (lighting != null)
            {
                lighting.Settings.directLightIntensityModifier = Mathf.Lerp(
                    lighting.Settings.directLightIntensityModifier,
                    targetWeatherType.lightingOverride.directLightIntensityModifier, blendTime);
                lighting.Settings.ambientIntensityModifier = Mathf.Lerp(lighting.Settings.ambientIntensityModifier,
                    targetWeatherType.lightingOverride.ambientIntensityModifier, blendTime);
            }
        }

        private void BlendFogOverride(float blendTime)
        {
            EnviroFogModule fog = EnviroManager.instance.Fog;

            if (fog != null)
            {
                fog.Settings.fogDensity = Mathf.Lerp(fog.Settings.fogDensity, targetWeatherType.fogOverride.fogDensity,
                    blendTime);
                fog.Settings.fogHeightFalloff = Mathf.Lerp(fog.Settings.fogHeightFalloff,
                    targetWeatherType.fogOverride.fogHeightFalloff, blendTime);
                fog.Settings.fogHeight = Mathf.Lerp(fog.Settings.fogHeight, targetWeatherType.fogOverride.fogHeight,
                    blendTime);

                fog.Settings.fogDensity2 = Mathf.Lerp(fog.Settings.fogDensity2,
                    targetWeatherType.fogOverride.fogDensity2, blendTime);
                fog.Settings.fogHeightFalloff2 = Mathf.Lerp(fog.Settings.fogHeightFalloff2,
                    targetWeatherType.fogOverride.fogHeightFalloff2, blendTime);
                fog.Settings.fogHeight2 = Mathf.Lerp(fog.Settings.fogHeight2, targetWeatherType.fogOverride.fogHeight2,
                    blendTime);

                fog.Settings.fogColorBlend = Mathf.Lerp(fog.Settings.fogColorBlend,
                    targetWeatherType.fogOverride.fogColorBlend, blendTime);

                fog.Settings.scattering = Mathf.Lerp(fog.Settings.scattering, targetWeatherType.fogOverride.scattering,
                    blendTime);
                fog.Settings.extinction = Mathf.Lerp(fog.Settings.extinction, targetWeatherType.fogOverride.extinction,
                    blendTime);
                fog.Settings.anistropy = Mathf.Lerp(fog.Settings.anistropy, targetWeatherType.fogOverride.anistropy,
                    blendTime);

#if ENVIRO_HDRP
                fog.Settings.fogAttenuationDistance =
 Mathf.Lerp(fog.Settings.fogAttenuationDistance, targetWeatherType.fogOverride.fogAttenuationDistance,blendTime); 
                fog.Settings.baseHeight =
 Mathf.Lerp(fog.Settings.baseHeight, targetWeatherType.fogOverride.baseHeight,blendTime); 
                fog.Settings.maxHeight =
 Mathf.Lerp(fog.Settings.maxHeight, targetWeatherType.fogOverride.maxHeight,blendTime); 
                
                fog.Settings.ambientDimmer =
 Mathf.Lerp(fog.Settings.ambientDimmer, targetWeatherType.fogOverride.ambientDimmer,blendTime);
                fog.Settings.directLightMultiplier =
 Mathf.Lerp(fog.Settings.directLightMultiplier, targetWeatherType.fogOverride.directLightMultiplier,blendTime);
                fog.Settings.directLightShadowdimmer =
 Mathf.Lerp(fog.Settings.ambientDimmer, targetWeatherType.fogOverride.directLightShadowdimmer,blendTime);
#endif
            }
        }

        private void BlendEffectsOverride(float blendTime)
        {
            EnviroEffectsModule effects = EnviroManager.instance.Effects;

            if (effects != null)
            {
                effects.Settings.rain1Emission = Mathf.Lerp(effects.Settings.rain1Emission,
                    targetWeatherType.effectsOverride.rain1Emission, blendTime);
                effects.Settings.rain2Emission = Mathf.Lerp(effects.Settings.rain2Emission,
                    targetWeatherType.effectsOverride.rain2Emission, blendTime);

                effects.Settings.snow1Emission = Mathf.Lerp(effects.Settings.snow1Emission,
                    targetWeatherType.effectsOverride.snow1Emission, blendTime);
                effects.Settings.snow2Emission = Mathf.Lerp(effects.Settings.snow2Emission,
                    targetWeatherType.effectsOverride.snow2Emission, blendTime);

                effects.Settings.custom1Emission = Mathf.Lerp(effects.Settings.custom1Emission,
                    targetWeatherType.effectsOverride.custom1Emission, blendTime);
                effects.Settings.custom2Emission = Mathf.Lerp(effects.Settings.custom2Emission,
                    targetWeatherType.effectsOverride.custom2Emission, blendTime);
            }
        }

        private void BlendVolumetricCloudsOverride(float blendTime)
        {
            EnviroVolumetricCloudsModule clouds = EnviroManager.instance.VolumetricClouds;

            if (clouds != null)
            {
                clouds.settingsLayer1.coverage = Mathf.Lerp(clouds.settingsLayer1.coverage,
                    targetWeatherType.cloudsOverride.coverageLayer1, blendTime);
                clouds.settingsLayer1.dilateCoverage = Mathf.Lerp(clouds.settingsLayer1.dilateCoverage,
                    targetWeatherType.cloudsOverride.dilateCoverageLayer1, blendTime);
                clouds.settingsLayer1.dilateType = Mathf.Lerp(clouds.settingsLayer1.dilateType,
                    targetWeatherType.cloudsOverride.dilateTypeLayer1, blendTime);
                clouds.settingsLayer1.cloudsTypeModifier = Mathf.Lerp(clouds.settingsLayer1.cloudsTypeModifier,
                    targetWeatherType.cloudsOverride.typeModifierLayer1, blendTime);
                clouds.settingsLayer1.anvilBias = Mathf.Lerp(clouds.settingsLayer1.anvilBias,
                    targetWeatherType.cloudsOverride.anvilBiasLayer1, blendTime);

                clouds.settingsLayer1.scatteringIntensity = Mathf.Lerp(clouds.settingsLayer1.scatteringIntensity,
                    targetWeatherType.cloudsOverride.scatteringIntensityLayer1, blendTime);
                clouds.settingsLayer1.multiScatteringA = Mathf.Lerp(clouds.settingsLayer1.multiScatteringA,
                    targetWeatherType.cloudsOverride.multiScatteringALayer1, blendTime);
                clouds.settingsLayer1.multiScatteringB = Mathf.Lerp(clouds.settingsLayer1.multiScatteringB,
                    targetWeatherType.cloudsOverride.multiScatteringBLayer1, blendTime);
                clouds.settingsLayer1.multiScatteringC = Mathf.Lerp(clouds.settingsLayer1.multiScatteringC,
                    targetWeatherType.cloudsOverride.multiScatteringCLayer1, blendTime);
                clouds.settingsLayer1.powderIntensity = Mathf.Lerp(clouds.settingsLayer1.powderIntensity,
                    targetWeatherType.cloudsOverride.powderIntensityLayer1, blendTime);
                clouds.settingsLayer1.silverLiningSpread = Mathf.Lerp(clouds.settingsLayer1.silverLiningSpread,
                    targetWeatherType.cloudsOverride.silverLiningSpreadLayer1, blendTime);
                clouds.settingsLayer1.lightAbsorbtion = Mathf.Lerp(clouds.settingsLayer1.lightAbsorbtion,
                    targetWeatherType.cloudsOverride.ligthAbsorbtionLayer1, blendTime);

                clouds.settingsLayer1.density = Mathf.Lerp(clouds.settingsLayer1.density,
                    targetWeatherType.cloudsOverride.densityLayer1, blendTime);
                clouds.settingsLayer1.baseErosionIntensity = Mathf.Lerp(clouds.settingsLayer1.baseErosionIntensity,
                    targetWeatherType.cloudsOverride.baseErosionIntensityLayer1, blendTime);
                clouds.settingsLayer1.detailErosionIntensity = Mathf.Lerp(clouds.settingsLayer1.detailErosionIntensity,
                    targetWeatherType.cloudsOverride.detailErosionIntensityLayer1, blendTime);
                clouds.settingsLayer1.curlIntensity = Mathf.Lerp(clouds.settingsLayer1.curlIntensity,
                    targetWeatherType.cloudsOverride.curlIntensityLayer1, blendTime);

                if (clouds.settingsGlobal.dualLayer)
                {
                    clouds.settingsLayer2.coverage = Mathf.Lerp(clouds.settingsLayer2.coverage,
                        targetWeatherType.cloudsOverride.coverageLayer2, blendTime);
                    clouds.settingsLayer2.dilateCoverage = Mathf.Lerp(clouds.settingsLayer2.dilateCoverage,
                        targetWeatherType.cloudsOverride.dilateCoverageLayer2, blendTime);
                    clouds.settingsLayer2.dilateType = Mathf.Lerp(clouds.settingsLayer2.dilateType,
                        targetWeatherType.cloudsOverride.dilateTypeLayer2, blendTime);
                    clouds.settingsLayer2.cloudsTypeModifier = Mathf.Lerp(clouds.settingsLayer2.cloudsTypeModifier,
                        targetWeatherType.cloudsOverride.typeModifierLayer2, blendTime);
                    clouds.settingsLayer2.anvilBias = Mathf.Lerp(clouds.settingsLayer2.anvilBias,
                        targetWeatherType.cloudsOverride.anvilBiasLayer2, blendTime);

                    clouds.settingsLayer2.scatteringIntensity = Mathf.Lerp(clouds.settingsLayer2.scatteringIntensity,
                        targetWeatherType.cloudsOverride.scatteringIntensityLayer2, blendTime);
                    clouds.settingsLayer2.multiScatteringA = Mathf.Lerp(clouds.settingsLayer2.multiScatteringA,
                        targetWeatherType.cloudsOverride.multiScatteringALayer2, blendTime);
                    clouds.settingsLayer2.multiScatteringB = Mathf.Lerp(clouds.settingsLayer2.multiScatteringB,
                        targetWeatherType.cloudsOverride.multiScatteringBLayer2, blendTime);
                    clouds.settingsLayer2.multiScatteringC = Mathf.Lerp(clouds.settingsLayer2.multiScatteringC,
                        targetWeatherType.cloudsOverride.multiScatteringCLayer2, blendTime);
                    clouds.settingsLayer2.powderIntensity = Mathf.Lerp(clouds.settingsLayer2.powderIntensity,
                        targetWeatherType.cloudsOverride.powderIntensityLayer2, blendTime);
                    clouds.settingsLayer2.silverLiningSpread = Mathf.Lerp(clouds.settingsLayer2.silverLiningSpread,
                        targetWeatherType.cloudsOverride.silverLiningSpreadLayer2, blendTime);
                    clouds.settingsLayer2.lightAbsorbtion = Mathf.Lerp(clouds.settingsLayer2.lightAbsorbtion,
                        targetWeatherType.cloudsOverride.ligthAbsorbtionLayer2, blendTime);

                    clouds.settingsLayer2.density = Mathf.Lerp(clouds.settingsLayer2.density,
                        targetWeatherType.cloudsOverride.densityLayer2, blendTime);
                    clouds.settingsLayer2.baseErosionIntensity = Mathf.Lerp(clouds.settingsLayer2.baseErosionIntensity,
                        targetWeatherType.cloudsOverride.baseErosionIntensityLayer2, blendTime);
                    clouds.settingsLayer2.detailErosionIntensity = Mathf.Lerp(
                        clouds.settingsLayer2.detailErosionIntensity,
                        targetWeatherType.cloudsOverride.detailErosionIntensityLayer2, blendTime);
                    clouds.settingsLayer2.curlIntensity = Mathf.Lerp(clouds.settingsLayer2.curlIntensity,
                        targetWeatherType.cloudsOverride.curlIntensityLayer2, blendTime);
                }
            }
        }

        private void BlendFlatCloudsOverride(float blendTime)
        {
            EnviroFlatCloudsModule flatClouds = EnviroManager.instance.FlatClouds;

            if (flatClouds != null)
            {
                flatClouds.settings.cirrusCloudsAlpha = Mathf.Lerp(flatClouds.settings.cirrusCloudsAlpha,
                    targetWeatherType.flatCloudsOverride.cirrusCloudsAlpha, blendTime);
                flatClouds.settings.cirrusCloudsCoverage = Mathf.Lerp(flatClouds.settings.cirrusCloudsCoverage,
                    targetWeatherType.flatCloudsOverride.cirrusCloudsCoverage, blendTime);
                flatClouds.settings.cirrusCloudsColorPower = Mathf.Lerp(flatClouds.settings.cirrusCloudsColorPower,
                    targetWeatherType.flatCloudsOverride.cirrusCloudsColorPower, blendTime);
                flatClouds.settings.flatCloudsCoverage = Mathf.Lerp(flatClouds.settings.flatCloudsCoverage,
                    targetWeatherType.flatCloudsOverride.flatCloudsCoverage, blendTime);
                flatClouds.settings.flatCloudsDensity = Mathf.Lerp(flatClouds.settings.flatCloudsDensity,
                    targetWeatherType.flatCloudsOverride.flatCloudsDensity, blendTime);
                flatClouds.settings.flatCloudsLightIntensity = Mathf.Lerp(flatClouds.settings.flatCloudsLightIntensity,
                    targetWeatherType.flatCloudsOverride.flatCloudsLightIntensity, blendTime);
                flatClouds.settings.flatCloudsAmbientIntensity = Mathf.Lerp(
                    flatClouds.settings.flatCloudsAmbientIntensity,
                    targetWeatherType.flatCloudsOverride.flatCloudsAmbientIntensity, blendTime);
                flatClouds.settings.flatCloudsAbsorbtion = Mathf.Lerp(flatClouds.settings.flatCloudsAbsorbtion,
                    targetWeatherType.flatCloudsOverride.flatCloudsAbsorbtion, blendTime);
            }
        }

        private void BlendAuroraOverride(float blendTime)
        {
            EnviroAuroraModule aurora = EnviroManager.instance.Aurora;

            if (aurora != null)
            {
                aurora.Settings.auroraIntensityModifier = Mathf.Lerp(aurora.Settings.auroraIntensityModifier,
                    targetWeatherType.auroraOverride.auroraIntensity, blendTime);
            }
        }

        private void BlendEnvironmentOverride(float blendTime)
        {
            EnviroEnvironmentModule environment = EnviroManager.instance.Environment;

            if (environment != null)
            {
                environment.Settings.temperatureWeatherMod = Mathf.Lerp(environment.Settings.temperatureWeatherMod,
                    targetWeatherType.environmentOverride.temperatureWeatherMod, blendTime);
                environment.Settings.wetnessTarget = Mathf.Lerp(environment.Settings.wetnessTarget,
                    targetWeatherType.environmentOverride.wetnessTarget, blendTime);
                environment.Settings.snowTarget = Mathf.Lerp(environment.Settings.snowTarget,
                    targetWeatherType.environmentOverride.snowTarget, blendTime);

                environment.Settings.windDirectionX = Mathf.Lerp(environment.Settings.windDirectionX,
                    targetWeatherType.environmentOverride.windDirectionX, blendTime);
                environment.Settings.windDirectionY = Mathf.Lerp(environment.Settings.windDirectionY,
                    targetWeatherType.environmentOverride.windDirectionY, blendTime);
                environment.Settings.windSpeed = Mathf.Lerp(environment.Settings.windSpeed,
                    targetWeatherType.environmentOverride.windSpeed, blendTime);
                environment.Settings.windTurbulence = Mathf.Lerp(environment.Settings.windTurbulence,
                    targetWeatherType.environmentOverride.windTurbulence, blendTime);
            }
        }

        private void BlendAudioOverride(float blendTime)
        {
            EnviroAudioModule audio = EnviroManager.instance.Audio;

            if (audio != null)
            {
                for (int i = 0; i < audio.Settings.ambientClips.Count; i++)
                {
                    bool hasOverride = false;

                    for (int a = 0; a < targetWeatherType.audioOverride.ambientOverride.Count; a++)
                    {
                        if (targetWeatherType.audioOverride.ambientOverride[a].name ==
                            audio.Settings.ambientClips[i].name)
                        {
                            audio.Settings.ambientClips[i].volume = Mathf.Lerp(audio.Settings.ambientClips[i].volume,
                                targetWeatherType.audioOverride.ambientOverride[a].volume, blendTime);
                            hasOverride = true;
                        }
                    }

                    if (!hasOverride)
                        audio.Settings.ambientClips[i].volume =
                            Mathf.Lerp(audio.Settings.ambientClips[i].volume, 0f, blendTime);
                }

                for (int i = 0; i < audio.Settings.weatherClips.Count; i++)
                {
                    bool hasOverride = false;

                    for (int a = 0; a < targetWeatherType.audioOverride.weatherOverride.Count; a++)
                    {
                        if (targetWeatherType.audioOverride.weatherOverride[a].name ==
                            audio.Settings.weatherClips[i].name)
                        {
                            audio.Settings.weatherClips[i].volume = Mathf.Lerp(audio.Settings.weatherClips[i].volume,
                                targetWeatherType.audioOverride.weatherOverride[a].volume, blendTime);
                            hasOverride = true;
                        }
                    }

                    if (!hasOverride)
                        audio.Settings.weatherClips[i].volume =
                            Mathf.Lerp(audio.Settings.weatherClips[i].volume, 0f, blendTime);
                }
            }
        }

        private void BlendLightningOverride(float blendTime)
        {
            EnviroLightningModule lightning = EnviroManager.instance.Lightning;

            if (lightning != null)
            {
                lightning.Settings.lightningStorm = targetWeatherType.lightningOverride.lightningStorm;
                lightning.Settings.randomLightingDelay = Mathf.Lerp(lightning.Settings.randomLightingDelay,
                    targetWeatherType.lightningOverride.randomLightningDelay, blendTime);
            }
        }

        private void BlendSkyOverride(float blendTime)
        {
            EnviroSkyModule sky = EnviroManager.instance.Sky;
            if (sky != null)
            {
                List<Gradient> gradients = new List<Gradient>();
                sky.Settings.frontColorGradient0 = MultuplyGradientKeys(sky.Settings.frontColorGradient0, targetWeatherType.skyOverride.multiplier);
                sky.Settings.frontColorGradient1 = MultuplyGradientKeys(sky.Settings.frontColorGradient1, targetWeatherType.skyOverride.multiplier);
                sky.Settings.frontColorGradient2 = MultuplyGradientKeys(sky.Settings.frontColorGradient2, targetWeatherType.skyOverride.multiplier);
                sky.Settings.frontColorGradient3 = MultuplyGradientKeys(sky.Settings.frontColorGradient3, targetWeatherType.skyOverride.multiplier);
                sky.Settings.frontColorGradient4 = MultuplyGradientKeys(sky.Settings.frontColorGradient4, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient5 = MultuplyGradientKeys(sky.Settings.backColorGradient5, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient0 = MultuplyGradientKeys(sky.Settings.backColorGradient0, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient1 = MultuplyGradientKeys(sky.Settings.backColorGradient1, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient2 = MultuplyGradientKeys(sky.Settings.backColorGradient2, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient3 = MultuplyGradientKeys(sky.Settings.backColorGradient3, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient4 = MultuplyGradientKeys(sky.Settings.backColorGradient4, targetWeatherType.skyOverride.multiplier);
                sky.Settings.backColorGradient5 = MultuplyGradientKeys(sky.Settings.backColorGradient5, targetWeatherType.skyOverride.multiplier);
            }
        }



        //Changes the Weather to new type.
        public void ChangeWeather(EnviroWeatherType type)
        {
            if (targetWeatherType != type)
            {
                EnviroManager.instance.NotifyWeatherChanged(type);
            }

            if (currentZone != null)
                currentZone.currentWeatherType = type;

            targetWeatherType = type;
        }

        public void ChangeWeather(string typeName)
        {
            for (int i = 0; i < Settings.weatherTypes.Count; i++)
            {
                if (Settings.weatherTypes[i].name == typeName)
                {
                    if (targetWeatherType != Settings.weatherTypes[i])
                    {
                        EnviroManager.instance.NotifyWeatherChanged(Settings.weatherTypes[i]);
                    }

                    if (currentZone != null)
                        currentZone.currentWeatherType = Settings.weatherTypes[i];

                    targetWeatherType = Settings.weatherTypes[i];
                }
            }
        }

        public void ChangeWeatherInstant(EnviroWeatherType type)
        {
            if (targetWeatherType != type)
            {
                EnviroManager.instance.NotifyWeatherChanged(type);
            }

            if (currentZone != null)
                currentZone.currentWeatherType = type;

            targetWeatherType = type;
            instantTransition = true;
        }

        //Save and Load
        public void LoadModuleValues()
        {
            if (preset != null)
            {
                Settings = JsonUtility.FromJson<Enviro.EnviroWeather>(JsonUtility.ToJson(preset.Settings));
            }
            else
            {
                Debug.Log("Please assign a saved module to load from!");
            }
        }

        public void SaveModuleValues()
        {
#if UNITY_EDITOR
            EnviroWeatherModule t = ScriptableObject.CreateInstance<EnviroWeatherModule>();
            t.name = "Weather Module";
            t.Settings = JsonUtility.FromJson<Enviro.EnviroWeather>(JsonUtility.ToJson(Settings));

            string assetPathAndName =
                UnityEditor.AssetDatabase.GenerateUniqueAssetPath(EnviroHelper.assetPath + "/New " + t.name + ".asset");
            UnityEditor.AssetDatabase.CreateAsset(t, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public void SaveModuleValues(EnviroWeatherModule module)
        {
            module.Settings = JsonUtility.FromJson<Enviro.EnviroWeather>(JsonUtility.ToJson(Settings));
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(module);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public void CalculateWeatherZones()
        {
            foreach (var weatherZone in GameObject.FindObjectsByType<EnviroZone>(FindObjectsSortMode.None))
            {
                // Skip if the list index is null
                if (weatherZone == null)
                    continue;

                // If weather zone has no collider, skip it as it's useless
                Collider weatherZoneCollider = weatherZone.GetComponent<Collider>();
                if (!weatherZoneCollider)
                    continue;

                if (!weatherZoneCollider.enabled || !weatherZoneCollider.gameObject.activeInHierarchy)
                    continue;

                // Find closest distance to weather zone, 0 means it's inside it
                float m_localWeatherZoneClosestDistanceSqr = float.PositiveInfinity;

                Vector3 m_localWeatherZoneClosestPoint =
                    weatherZoneCollider.ClosestPoint(EnviroManager.instance.Camera.transform.position); // 5.6-only API
                float m_localWeatherZoneDistance =
                    ((m_localWeatherZoneClosestPoint - EnviroManager.instance.Camera.transform.position) / 2f)
                    .sqrMagnitude;

                if (m_localWeatherZoneDistance < m_localWeatherZoneClosestDistanceSqr)
                    m_localWeatherZoneClosestDistanceSqr = m_localWeatherZoneDistance;

                weatherZoneCollider = null;
                float m_localWeatherZoneBlendDistanceSqr = weatherZone.blendDistance * weatherZone.blendDistance;

                // Weather zone has no influence, ignore it
                // Note: Weather zone doesn't do anything when `closestDistanceSqr = blendDistSqr` but
                //       we can't use a >= comparison as blendDistSqr could be set to 0 in which
                //       case weather zone would have total influence
                if (m_localWeatherZoneClosestDistanceSqr > m_localWeatherZoneBlendDistanceSqr)
                {
                    currentZone = null;
                    continue;
                }
                currentZone = weatherZone;

                // Weather zone has influence
                float m_localWeatherZoneInterpolationFactor = 1f;

                if (m_localWeatherZoneBlendDistanceSqr > 0f)
                    m_localWeatherZoneInterpolationFactor =
                        1f - (m_localWeatherZoneClosestDistanceSqr / m_localWeatherZoneBlendDistanceSqr);

                // No need to clamp01 the interpolation factor as it'll always be in [0;1[ range
                BlendOverride(targetWeatherType, weatherZone.currentWeatherType, m_localWeatherZoneInterpolationFactor);
            }
        }

        public void BlendOverride(EnviroWeatherType from, EnviroWeatherType to, float i)
        {
            targetWeatherType = from;
            BlendVolumetricCloudsOverride(1);
            BlendFlatCloudsOverride(1);
            BlendLightingOverride(1);
            BlendEffectsOverride(1);
            BlendAuroraOverride(1);
            BlendFogOverride(1);
            BlendAudioOverride(1);
            BlendEnvironmentOverride(1);
            BlendLightningOverride(1f);
            //BlendSkyOverride(1);

            targetWeatherType = to;
            BlendVolumetricCloudsOverride(i);
            BlendFlatCloudsOverride(i);
            BlendLightingOverride(i);
            BlendEffectsOverride(i);
            BlendAuroraOverride(i);
            BlendFogOverride(i);
            BlendAudioOverride(i);
            BlendEnvironmentOverride(i);
            BlendLightningOverride(1f);
            //BlendSkyOverride(1);

            targetWeatherType = from;
        }

        public Gradient MultuplyGradientKeys(Gradient from, Color mul)
        {
            Gradient to = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[from.colorKeys.Length];
            GradientAlphaKey[] alphaKeys = from.alphaKeys;
            for (int i = 0; i < from.colorKeys.Length; i++)
            {
                float h, s, v, h1, s1, v1;
                Color.RGBToHSV(from.colorKeys[i].color, out h, out s, out v);
                Color.RGBToHSV(mul, out h1, out s1, out v1);
                Color color = Color.HSVToRGB(h1, s, v);
                colorKeys[i] = new GradientColorKey(color, from.colorKeys[i].time);
            }
            to.SetKeys(colorKeys, alphaKeys);
            return to;
        }
    }
}