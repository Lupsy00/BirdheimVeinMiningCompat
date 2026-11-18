using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;

namespace BirdheimVeinMiningCompat
{
    [BepInPlugin("lupsy.valheim.birdheimveinminingcompat", "Birdheim VeinMining Compat", "1.0.0")]
    [BepInDependency("hansenbansen.birdheim", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            try
            {
                Type guardType = AccessTools.TypeByName("Birdheim.LoadingGuard");
                if (guardType == null)
                {
                    guardType = AccessTools.TypeByName("LoadingGuard");
                }

                if (guardType == null)
                {
                    Logger.LogWarning("Could not find Birdheim's LoadingGuard class.");
                    return;
                }

                var knownField = AccessTools.Field(guardType, "Known");
                var knownCollection = knownField != null ? knownField.GetValue(null) as ICollection<string> : null;

                if (knownCollection != null)
                {
                    if (!knownCollection.Contains("local.valheim.sharedmining"))
                    {
                        knownCollection.Add("local.valheim.sharedmining");
                        Logger.LogInfo("Successfully added Vein Mining (local.valheim.sharedmining) to Birdheim's LoadingGuard whitelist.");
                    }
                }
                else
                {
                    Logger.LogWarning("Could not read the 'Known' field in Birdheim's LoadingGuard.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to patch Birdheim's LoadingGuard whitelist: " + ex.ToString());
            }
        }
    }
}