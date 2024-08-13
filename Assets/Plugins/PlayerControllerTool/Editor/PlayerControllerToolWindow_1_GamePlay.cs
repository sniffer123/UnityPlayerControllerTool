using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace PlayerControllerTool
{
    public partial class PlayerControllerToolWindow : OdinEditorWindow
    {

        #region Game Speed

        [TabGroup("PlayerControllerToolWindow","GamePlay")]
        [TitleGroup("PlayerControllerToolWindow/GamePlay/Change Game Play",indent:true)]
        [HorizontalGroup("PlayerControllerToolWindow/GamePlay/Change Game Play/Info",width:0.5f)]
        [Range(0.1f, 3)]
        public float GameSpeed = 1.0f;
        
        [TabGroup("PlayerControllerToolWindow","GamePlay")]
        [TitleGroup("PlayerControllerToolWindow/GamePlay/Change Game Play")]
        [HorizontalGroup("PlayerControllerToolWindow/GamePlay/Change Game Play/Info")]
        [Button(ButtonSizes.Medium,Name = "Change")]
        void SendCommand_ChangeGameSpeed()
        {
            var command = new Command_ChangeGameSpeed();
            command.GameSpeed = GameSpeed;
            command.SendToPlayer();
        }
        
        [TabGroup("PlayerControllerToolWindow","GamePlay")]
        [TitleGroup("PlayerControllerToolWindow/GamePlay/Change Game Play")]
        [HorizontalGroup("PlayerControllerToolWindow/GamePlay/Change Game Play/Info")]
        [Button(ButtonSizes.Medium,Name = "Reset")]
        void SendCommand_ResetGameSpeed()
        {
            var command = new Command_ChangeGameSpeed();
            command.GameSpeed = 1.0f;
            command.SendToPlayer();
        }

        #endregion
        
    }

    
}
