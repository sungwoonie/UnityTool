using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public static class ScriptableGeneratorUtility
{
    [MenuItem("Tools/Scriptable Generator/Generate All ScriptableObjects")]
    public static void GenerateAll()
    {
        foreach(ScriptableStatType statType in Enum.GetValues(typeof(ScriptableStatType)))
        {
            if(NotScriptable(statType))
                continue;

            string folderPath = $"Assets/Resources/ScriptableStats/{statType}";
            string dataTablePath = $"Assets/DataTables/Stat/CSV/{statType}Table.csv";

            if(!File.Exists(dataTablePath))
            {
                Debug.LogWarning($"CSV 파일 없음 → 스킵: {dataTablePath}");
                continue;
            }

            string csvText = File.ReadAllText(dataTablePath);
            TextAsset csvAsset = new TextAsset(csvText);

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }

            var parsedData = CSVReader.Read(csvAsset);

            if(IsOneStat(statType))
            {
                string assetPath = $"{folderPath}/{statType}.asset";
                if(AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath) == null)
                    CreateInstance(statType, assetPath, statType.ToString());
                continue;
            }

            foreach(var entry in parsedData)
            {
                string assetPath = $"{folderPath}/{entry.Key}.asset";
                if(AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath) == null)
                    CreateInstance(statType, assetPath, entry.Key);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        EditorUtility.DisplayDialog("Scriptable Generator", "모든 ScriptableObject 생성 완료!", "확인");
    }

    private static void CreateInstance(ScriptableStatType statType, string path, string key)
    {
        Type soType = null;

        if(specialTypeOverrides.TryGetValue(statType, out soType))
        {
            Debug.Log($"[Generator] 예외 매핑: {statType} → {soType.Name}");
        }
        else
        {
            string className = $"{statType}Scriptable";
            foreach(var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(var t in asm.GetTypes())
                {
                    if(t.Name == className && t.IsSubclassOf(typeof(ScriptableObject)))
                    {
                        soType = t;
                        break;
                    }
                }
                if(soType != null) break;
            }
        }

        if(soType == null)
        {
            Debug.LogError($"[Generator] 타입 탐색 실패: {statType}");
            return;
        }

        var instance = ScriptableObject.CreateInstance(soType);
        instance.name = key;

        var method = soType.GetMethod("SetStatName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        method?.Invoke(instance, new object[] { key });

        AssetDatabase.CreateAsset(instance, path);
        Debug.Log($"[Generator] 생성됨: {path}");
    }

    private static bool NotScriptable(ScriptableStatType statType)
    {
        return true; //
        //return statType == ScriptableStatType.DiceStat || statType == ScriptableStatType.NecklaceEffectStat || statType == ScriptableStatType.RingElementalEffectStat;
    }

    private static bool IsOneStat(ScriptableStatType statType)
    {
        return true; //
        //return statType == ScriptableStatType.AdvancementStat || statType == ScriptableStatType.AwakeningTierStat || statType == ScriptableStatType.GrimoireStat || statType == ScriptableStatType.VIPStat;
    }

    private static readonly Dictionary<ScriptableStatType, Type> specialTypeOverrides = new()
    {
        //{ ScriptableStatType.AbilityStat, typeof(TrainingStatScriptable) },
        //{ ScriptableStatType.AwakeningStat, typeof(TrainingStatScriptable) },
        //{ ScriptableStatType.TrainingStat, typeof(TrainingStatScriptable) },
    };
}

public enum ScriptableStatType
{
    None
}