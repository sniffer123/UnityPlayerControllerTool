using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerControllerTool
{
    [Serializable]
    public class Command_ChangeGameSpeed : PlayerControllerToolCommon.PlayerControllerToolBaseCommand
    {
        public float GameSpeed = 1.0f;
        public bool result = false;
        
        public override void ExecuteOnPlayer()
        {
            base.ExecuteOnPlayer();

            Time.timeScale = GameSpeed;

            result = true;

            SendToEditor();
        }

    }

}
