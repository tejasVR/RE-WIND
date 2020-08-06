using System.IO;
using UnityEditor;
using UnityEngine;

namespace XREngine.Core.Scripts.Editor
{
    public class ManageXREngineChanges : EditorWindow
    {
        [SerializeField] private string xrEnginePath = "";

        [MenuItem("XREngine/Manage XREngine Changes")]
        private static void ManageChanges()
        {
            GetWindow<ManageXREngineChanges>();
        }

        protected void OnEnable()
        {
            // Here we retrieve the data if it exists or we save the default field initialisers we set above
            var data = EditorPrefs.GetString("ManageXREngineChanges", JsonUtility.ToJson(this, false));
            
            // Then we apply them to this window
            JsonUtility.FromJsonOverwrite(data, this);
        }

        protected void OnDisable()
        {
            // We get the Json data
            var data = JsonUtility.ToJson(this, false);
            
            // And we save it
            EditorPrefs.SetString("ManageXREngineChanges", data);
        }

        private void OnGUI()
        {
            xrEnginePath = EditorGUILayout.TextField("XREngine Repo Path: ", xrEnginePath);

            GUILayout.Label("Make sure to link the path to the 'XREngine' folder within your XREngine repo");

            if (GUILayout.Button("Browse for XREngine Repo"))
            {
                xrEnginePath = EditorUtility.OpenFolderPanel("Select XREngine Repo Location", "", "");

                Debug.Log("Hey there, you've set your XREngine repo folder to be: " + xrEnginePath);
            }

            if (GUILayout.Button("Push XREngine Changes"))
            {
                if (Directory.Exists(xrEnginePath))
                {
                    FileUtil.ReplaceDirectory("Assets/XREngine", xrEnginePath);

                    Debug.Log("Hey there, the files in your current XREngine folder have been sent over to your XREngine repo!");
                }
            }

            if (GUILayout.Button("Pull XREngine Changes"))
            {
                if (Directory.Exists(xrEnginePath))
                {
                    FileUtil.ReplaceDirectory(xrEnginePath, "Assets/XREngine");

                    AssetDatabase.Refresh();

                    Debug.Log("Hey there, the files in your XREngine Repo have been pulled into your current XREngine folder!");
                }
            }
        }
    }
}


