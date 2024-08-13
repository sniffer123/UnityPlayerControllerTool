using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace PlayerControllerTool
{
    public partial class PlayerControllerToolWindow : OdinEditorWindow
    {

        #region Custom Command


        [TabGroup("PlayerControllerToolWindow","GamePlay")]
        [TitleGroup("PlayerControllerToolWindow/GamePlay/Custom Command")]
        [HorizontalGroup("PlayerControllerToolWindow/GamePlay/Custom Command/Info")]
        [Button(ButtonSizes.Medium,Name = "Custom Command")]
        void SendCommand_CustomCommand()
        {
            Debug.Log($"On Click Custom Command");
        }
        
        #endregion
        
    }

    
}
