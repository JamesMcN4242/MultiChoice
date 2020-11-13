////////////////////////////////////////////////////////////
/////   PresetDataSystem.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Debug;

public static class PresetDataSystem
{
    private const string k_presetKey = "PresetDictionary";

    public static void SaveNewPreset(string key, List<string> entries)
    {
        Dictionary<string, List<string>> presets = LoadAllPresets();
        Assert(presets.ContainsKey(key) == false, "Trying to save a new preset, but overwrites a key instead");
        presets.Add(key, entries);
        SaveNewPresets(presets);
    }

    public static void OverwritePreset(string key, List<string> entries)
    {
        Dictionary<string, List<string>> presets = LoadAllPresets();
        presets[key] = entries;
        SaveNewPresets(presets);
    }

    public static Dictionary<string, List<string>> LoadAllPresets()
    {
        string json = PlayerPrefs.GetString(k_presetKey, null);

        if(string.IsNullOrEmpty(json))
        {
            return new Dictionary<string, List<string>>();
        }
        return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
    }

    public static List<string> LoadPreset(string key)
    {
        Dictionary<string, List<string>> presets = LoadAllPresets();
        return presets.ContainsKey(key) ? presets[key] : new List<string>();
    }

    public static void SaveNewPresets(Dictionary<string, List<string>> presets)
    {
        string json = JsonConvert.SerializeObject(presets);
        PlayerPrefs.SetString(k_presetKey, json);
        PlayerPrefs.Save();
    }
}
