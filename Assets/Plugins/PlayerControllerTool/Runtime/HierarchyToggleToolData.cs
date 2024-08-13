using System;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerControllerTool
{
    
    
    [Serializable]
    public class PlayerControllerToolData 
    {
        [Serializable]
        public class PlayerControllerToolData_Node
        {
            [BoxGroup("GameObjectInfo")]
            [LabelWidth(100f),ReadOnly]
            public string name;
            
            [BoxGroup("GameObjectInfo")]
            [ LabelWidth(100f),ReadOnly]
            public bool active;

            [HideInInspector]
            public PlayerControllerToolData_Node[] childs;
            
            private PlayerControllerToolData_Node _parent;
            public PlayerControllerToolData_Node parent => _parent;

            public (string,string) GetGoPath()
            {
                StringBuilder sb = new StringBuilder();
                var parentStack = new Stack<PlayerControllerToolData_Node>();
                var node = this;
                while (node.parent != null )
                {
                    parentStack.Push(node.parent);
                    node = node.parent;
                }

                while (parentStack.Count > 1)
                {
                    node = parentStack.Pop();
                    sb.Append(node.name).Append("/");
                }
                sb.Append(name);

                var rootName = string.Empty;
                if (parentStack.Count > 0)
                {
                    var root = parentStack.Pop();
                    rootName = root.name;
                }
                else
                {
                    //Debug.LogError($"[PlayerControllerToolWindow]Invalid node without root.");
                    rootName = name;
                }
                return (rootName,sb.ToString());
            }

            public void Setup(PlayerControllerToolData_Node parent)
            {
                _parent = parent;

                if (childs != null)
                {
                    foreach (var child in childs)
                    {
                        child.Setup(this);
                    }
                }
            }

            
            [HideIf("active")]
            [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
            private void SetEnable()
            {
                active = true;
                ToggleOnPlayer(active);
            }

            [ShowIf("active")]
            [Button(ButtonSizes.Large), GUIColor(1, 0.2f, 0)]
            private void SetDisable()
            {
                active = false;
                ToggleOnPlayer(active);
            }

            private void ToggleOnPlayer(bool in_active)
            {
                var command = new Command_ToggleHierarchy();
                (command.rootName,command.goPathWithoutRoot) = GetGoPath();
                command.active = in_active;
                command.SendToPlayer();
            }
        }

        public PlayerControllerToolData_Node[] roots;

        public PlayerControllerToolData_Node FindNode(string nodePath)
        {
            return null;
        }

        public static PlayerControllerToolData BuildTestData()
        {
            var data = new PlayerControllerToolData();
            data.roots = GenNodes(3, "Root");
            data.roots[0].childs = GenNodes(5, "Child");
            data.roots[1].childs = GenNodes(4, "Child");
            data.roots[2].childs = GenNodes(6, "Child");

            PlayerControllerToolData_Node[] GenNodes(int size, string namePrefix)
            {
                var nodes = new PlayerControllerToolData_Node[size];
                for (int i = 0; i < size; i++)
                {
                    nodes[i] = new PlayerControllerToolData_Node(){ name = $"{namePrefix} {i}",active = true};
                }

                return nodes;
            }
            
            return data;
        }

        public void Setup()
        {
            if (roots != null)
            {
                foreach (var root in roots)
                {
                    root.Setup(null);
                }
            }
        }

    }

    public struct PlayerControllerToolDataWrap
    {
        public PlayerControllerToolData Data;

        public int DataVersion;

        public void SetData(PlayerControllerToolData in_Data)
        {
            Data = in_Data;
            Data.Setup();
            DataVersion++;
        }
    }

}
