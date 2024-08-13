using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerControllerTool
{
    [Serializable]
    public class Command_CollectHierarchy : PlayerControllerToolCommon.PlayerControllerToolBaseCommand
    {
        public PlayerControllerToolData data;
        
        
        public override void ExecuteOnPlayer()
        {
            base.ExecuteOnPlayer();

            data = new PlayerControllerToolData();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scenes = SceneManager.GetSceneAt(i);
                var roots = scenes.GetRootGameObjects();
                data.roots = new PlayerControllerToolData.PlayerControllerToolData_Node[roots.Length];
                for (int j = 0; j < roots.Length; j++)
                {
                    var goRoot = roots[j];
                    data.roots[j] = AddGameObjectInfo(goRoot);
                }
            }
            
            SendToEditor();
        }

        private PlayerControllerToolData.PlayerControllerToolData_Node AddGameObjectInfo(
            GameObject gameObject)
        {
            var node = new PlayerControllerToolData.PlayerControllerToolData_Node();
            node.name = gameObject.name;
            node.active = gameObject.activeSelf;

            node.childs = new PlayerControllerToolData.PlayerControllerToolData_Node[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var goChild = gameObject.transform.GetChild(i).gameObject;
                node.childs[i] = AddGameObjectInfo(goChild);
            }
            
            return node;
        }

    }

}
