using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace PlayerControllerTool
{
    public class PlayerControllerToolCommon 
    {
        public static readonly Guid kMsgSendToPlayer = new Guid("d4e156a5-17eb-4877-8f58-07479c218920");
        public static readonly Guid kMsgSendToEditor = new Guid("effd824b-4588-436e-89fb-0e8491cff8ef");

        public static bool kUseLog = true;
        public static Action<string> Log => kUseLog?Debug.Log: (str) => { };
        public static Action<string> LogError => kUseLog?Debug.LogError: (str) => { };
        
        [System.AttributeUsage(System.AttributeTargets.Method)
        ]
        public class ControllerCommandHandlerAttribute : System.Attribute
        {
            public Type TargetCommandType;

            public ControllerCommandHandlerAttribute(Type targetCommand)
            {
                TargetCommandType = targetCommand;
            }
        }
        
        [Serializable]
        public abstract class PlayerControllerToolBaseCommand
        {
            public byte[] Serialize()
            {
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    
                    var typeName = Encoding.ASCII.GetBytes(GetType().FullName);
                    bw.Write(typeName.Length);
                    bw.Write(typeName);
                    
                    var json = JsonUtility.ToJson(this);
                    var jsonBytes = Encoding.ASCII.GetBytes(json);
                    bw.Write(jsonBytes.Length);
                    bw.Write(jsonBytes);

                    return ms.ToArray();
                }
            }

            public static PlayerControllerToolBaseCommand DeSerialize(byte[] data)
            {
                using (var ms = new MemoryStream(data))
                using (var br = new BinaryReader(ms))
                {
                    int typeNameLength = br.ReadInt32();
                    var typeNameBytes = br.ReadBytes(typeNameLength);
                    var typeName = Encoding.ASCII.GetString(typeNameBytes);
                    var type = Type.GetType(typeName);
                    if (type == null)
                    {
                        Log($"[PlayerControllerTool]Can not find type with name:{typeName}");
                        return null;
                    }

                    var jsonBytesLength = br.ReadInt32();
                    var jsonBytes = br.ReadBytes(jsonBytesLength);
                    var json = Encoding.ASCII.GetString(jsonBytes);
                    var command = JsonUtility.FromJson(json,type);
                    return command as PlayerControllerToolBaseCommand;
                }
            }

            
            public virtual void ExecuteOnPlayer()
            {
                Log($"[PlayerControllerTool]Execute command on Player:{GetType().Name}");
            }

            public virtual void ExecuteOnEditor()
            {
                Log($"[PlayerControllerTool]Execute command on Editor:{GetType().Name}");
            }

            public virtual void SendToPlayer()
            {
                #if UNITY_EDITOR
                var data = Serialize();
                var connection = UnityEditor.Networking.PlayerConnection.EditorConnection.instance;
                if (connection.ConnectedPlayers.Count > 0)
                {
                    connection.Send(PlayerControllerToolCommon.kMsgSendToPlayer, data);
                }
                else
                {
                    Debug.Log($"No connection found.Try execute in Editor.");
                    ExecuteOnPlayer();
                }
                
                #endif
            }

            public virtual void SendToEditor()
            {
                #if DEVELOPMENT_BUILD
                
                #endif
                var data = Serialize();
                UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.Send(PlayerControllerToolCommon.kMsgSendToEditor, data);
            }
        }
        
    }

}
