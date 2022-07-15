using UnityEngine;
using UnityEditor;
using System;

namespace Project.Singleton.ScriptableObjects
{
    /// <summary>
    /// Allows the child class to be a singleton scriptable object. In this state, there can be only one entity in which data is stored and at the same time there is access to the entity through the Instance field.
    /// </summary>
    /// <typeparam name="T">Nested class</typeparam>
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] results = Resources.FindObjectsOfTypeAll<T>();
                    if (results.Length == 0)
                    {
                        _instance = CreateScriptableObject();
                    }
                    else if (results.Length > 1)
                    {
                        throw new Exception($"\"{_instance.GetType()}\" must be in one instance.");
                    }
                    else
                    {
                        _instance = results[0];
                    }
                    _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }
                return _instance;
            }
        }

        private static T CreateScriptableObject()
        {
            T asset = CreateInstance<T>();
            //!!!Нужно заменить статический путь на путь к этому скрипту.
            string path = $"Assets/ScriptableObjects/SingletonScriptableObject/{asset.GetType().Name}/{asset.GetType().Name}.asset";
            var oldAsset = AssetDatabase.LoadAssetAtPath(path, asset.GetType());
            if (AssetDatabase.LoadAssetAtPath(path, asset.GetType()) != null)
            {
                return (T)oldAsset;
            }
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Debug.LogWarning($"{asset.GetType()} was not created and therefore was created automatically.");
            Selection.activeObject = asset;
            return asset;
        }
    }
}
