using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace PlayerControllerTool
{
    public class PlayerControllerToolRuntime : MonoBehaviour
    {
        void OnEnable()
        {
            PlayerConnection.instance.RegisterConnection(OnConnect);
            PlayerConnection.instance.RegisterDisconnection(OnDisconnect);
            PlayerConnection.instance.Register(PlayerControllerToolCommon.kMsgSendToPlayer, OnMessageEvent);
        }

        void OnDisable()
        {
            PlayerConnection.instance.UnregisterConnection(OnConnect);
            PlayerConnection.instance.UnregisterDisconnection(OnDisconnect);
            PlayerConnection.instance.Unregister(PlayerControllerToolCommon.kMsgSendToPlayer, OnMessageEvent);
        }
    
        private void OnConnect(int playerId)
        {
            Debug.Log("OnConnect: " + playerId);
        }
    
        private void OnDisconnect(int playerId)
        {
            Debug.Log("OnDisconnect: " + playerId);
        }

        private void OnMessageEvent(MessageEventArgs args)
        {
            //Debug.Log("Message from Editor");

            var command = PlayerControllerToolCommon.PlayerControllerToolBaseCommand.DeSerialize(args.data);
            command.ExecuteOnPlayer();
        }
    }

}
