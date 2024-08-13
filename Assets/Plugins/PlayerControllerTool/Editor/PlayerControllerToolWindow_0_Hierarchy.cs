using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace PlayerControllerTool
{
    public partial class PlayerControllerToolWindow : OdinEditorWindow
    {
        [TabGroup("PlayerControllerToolWindow","Hierarchy")]
        [HorizontalGroup("PlayerControllerToolWindow/Hierarchy/Row1",Title = "Performs",Order = 0)]
        [Button(ButtonSizes.Medium,Name = "Collect Hierarchy Data")]
        void SendCommand_CollectHierarchy()
        {
            var command = new Command_CollectHierarchy();
            command.SendToPlayer();
        }
        
        // 调试可打开该按钮
        // [TabGroup("PlayerControllerToolWindow","Hierarchy")]
        // [HorizontalGroup("PlayerControllerToolWindow/Hierarchy/Row1")]
        // [Button(ButtonSizes.Medium,Name = "Test Hierarchy Data")]
        // void GenTestHierarchyData()
        // {
        //     var data = PlayerControllerToolData.BuildTestData();
        //     _PlayerControllerToolDataWrap.SetData(data);
        //     
        // }

        [TabGroup("PlayerControllerToolWindow", "Hierarchy")]
        [HorizontalGroup("PlayerControllerToolWindow/Hierarchy/Row2", Title = "Runtime Hierarchy",Order = 1)]
        [ShowInInspector]
        private PlayerControllerToolDataWrap _PlayerControllerToolDataWrap = new PlayerControllerToolDataWrap();

        [PlayerControllerToolCommon.ControllerCommandHandler(typeof(Command_CollectHierarchy))]
        void OnReceiveCommand(Command_CollectHierarchy command)
        {
            Debug.Log("[PlayerControllerToolWindow]OnReceive Command_CollectHierarchy");
            _PlayerControllerToolDataWrap.SetData(command.data);
        }
        
        [PlayerControllerToolCommon.ControllerCommandHandler(typeof(Command_ToggleHierarchy))]
        void OnReceiveCommand(Command_ToggleHierarchy command)
        {
            Debug.Log("[PlayerControllerToolWindow]OnReceive Command_ToggleHierarchy");
            ShowNotification(new GUIContent($"Set [{command.rootName}/{command.goPathWithoutRoot}] to {command.active}.Result:{command.result}"));
        }
    }

    
}
