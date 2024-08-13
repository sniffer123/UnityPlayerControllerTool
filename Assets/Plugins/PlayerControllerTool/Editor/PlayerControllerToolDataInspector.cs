using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace PlayerControllerTool
{
#region Odin Menu Tree

    public class PlayerControllerToolDataInspector : OdinValueDrawer<PlayerControllerToolDataWrap>
    {
        private OdinMenuTree _odinMenuTree;
        private PlayerControllerToolData _data;
        private PlayerControllerToolData.PlayerControllerToolData_Node _curNode;
        private PropertyTree _nodePropertyTree;

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (_odinMenuTree == null)
            {
                _odinMenuTree = new OdinMenuTree();
                _odinMenuTree.Config.DrawSearchToolbar = true;
                _odinMenuTree.Selection.SelectionChanged += new Action<SelectionChangedType>(this.OnSelectionChanged);
            }
            
            PlayerControllerToolDataWrap value = this.ValueEntry.SmartValue;
            if (_data != value.Data)
            {
                Debug.Log($"On rebuild menu tree");
                _data = value.Data;
                _odinMenuTree.MenuItems.Clear();
                foreach (var root in _data.roots)
                {
                    AddNode(_odinMenuTree.MenuItems,root,"");
                }

                
                // int testCount = 2000;
                // var testNode = new PlayerControllerToolData.PlayerControllerToolData_Node();
                // testNode.childs = new PlayerControllerToolData.PlayerControllerToolData_Node[testCount];
                // for (int i = 0; i < 30; i++)
                // {
                //     var child = new PlayerControllerToolData.PlayerControllerToolData_Node();
                //     testNode.childs[i] = child;
                // }
                // for (int i = 0; i < testCount; i++)
                // {
                //     _odinMenuTree.Add($"Home{i}",                           testNode,                           EditorIcons.House);
                //     for (int j = 0; j < 10; j++)
                //     {
                //         _odinMenuTree.Add($"Home{i}/Child{j}",                           testNode,                           EditorIcons.House);
                //     }
                // }
                // _odinMenuTree = new OdinMenuTree(supportsMultiSelect: true)
                // {
                //     { "Home",                           this,                           EditorIcons.House       },
                //     { "Odin Settings",                  null,                           EditorIcons.House     },
                //     { "Odin Settings/Color Palettes",   ColorPaletteManager.Instance,   EditorIcons.EyeDropper  },
                //     { "Camera current",                 Camera.current                                          },
                // };
            }
            Draw();
            
            this.ValueEntry.SmartValue = value;
            
        }

        void AddNode(List<OdinMenuItem> odinMenuItems, PlayerControllerToolData.PlayerControllerToolData_Node node,string prefix)
        {
            var itemPath = $"{prefix}/{node.name}";
            _odinMenuTree.Add(itemPath,node);
            if (node.childs != null)
            {
                foreach (var child in node.childs)
                {
                    AddNode(odinMenuItems, child, itemPath);
                }
            }

        }
        
        private void OnSelectionChanged(SelectionChangedType type)
        {
            _curNode = _odinMenuTree.Selection.SelectedValue as PlayerControllerToolData.PlayerControllerToolData_Node;
            if (_nodePropertyTree != null)
            {
                _nodePropertyTree.Dispose();
                _nodePropertyTree = null;
            }

            if (_curNode != null)
            {
                _nodePropertyTree = PropertyTree.Create(_curNode);
            }
            
            var item = _odinMenuTree.MenuItems.Find(x => x.Value == _curNode);
            if (item != null)
            {
                item.Toggled = !item.Toggled;
            }
        }

        private float menuWidth = 280f;
        public virtual float MenuWidth
        {
            get => this.menuWidth;
            set => this.menuWidth = value;
        }
        private void Draw()
        {
            var outerRect = EditorGUILayout.BeginHorizontal(GUILayoutOptions.MinHeight(600).ExpandHeight(true));
            EditorGUILayout.BeginVertical((GUILayoutOption[]) GUILayoutOptions.Width(this.MenuWidth).ExpandHeight(true));
            Rect currentLayoutRect = GUIHelper.GetCurrentLayoutRect();
            
            EditorGUI.DrawRect(currentLayoutRect, SirenixGUIStyles.MenuBackgroundColor);
            Rect rect = currentLayoutRect;
            rect.xMin = currentLayoutRect.xMax - 4f;
            rect.xMax += 4f;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
            this.MenuWidth += SirenixEditorGUI.SlideRect(rect).x;
            _odinMenuTree.DrawMenuTree();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical((GUILayoutOption[]) GUILayoutOptions.ExpandHeight(true));
            EditorGUI.DrawRect(GUIHelper.GetCurrentLayoutRect(), SirenixGUIStyles.DarkEditorBackground);
            // draw others
            DrawSelectionNode();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUI.DrawRect(rect.AlignCenter(1f), SirenixGUIStyles.BorderColor);
            if (_odinMenuTree != null)
                _odinMenuTree.HandleKeyboardMenuNavigation(); 
            
        }

        private void DrawSelectionNode()
        {
            if (_curNode != null)
            {
                _nodePropertyTree.Draw(false);

            }
        }

    }

    #endregion
    


}


