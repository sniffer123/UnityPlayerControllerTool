using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerControllerTool
{
    [Serializable]
    public class Command_ToggleHierarchy : PlayerControllerToolCommon.PlayerControllerToolBaseCommand
    {
        public string rootName = string.Empty;
        public string goPathWithoutRoot = string.Empty;
        public bool active = false;
        public bool result = false;
        
        public override void ExecuteOnPlayer()
        {
            base.ExecuteOnPlayer();

            Transform found = null;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                foreach (var root in scene.GetRootGameObjects())
                {
                    if (root.name == rootName)
                    {
                        found = root.transform.Find(goPathWithoutRoot);
                    }
                }
            }

            if (found != null)
            {
                found.gameObject.SetActive(active);
                Debug.Log($"[PlayerControllerToolWindow]Set [{rootName}/{goPathWithoutRoot}] to {active}");
                result = true;
            }

            SendToEditor();
        }

    }

}
