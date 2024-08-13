using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace PlayerControllerTool
{
public partial class PlayerControllerToolWindow : OdinEditorWindow
{
    // The state can survive for the life time of the EditorWindow so it's best to store it here and just renew/dispose of it in OnEnable and OnDisable, rather than fetching repeatedly it in OnGUI.
    IConnectionState attachProfilerState;

    [MenuItem("Tools/PlayerControllerToolWindow")]
    private static void OpenWindow()
    {
        var window = GetWindow<PlayerControllerToolWindow>();
        window.Show();
        //window.titleContent = new GUIContent("PlayerControllerToolWindow");
    }


    protected override void  OnEnable()
    {
        base.OnEnable();
        InitCommandHandlers();
        attachProfilerState = PlayerConnectionGUIUtility.GetConnectionState(this, OnConnectedState);
        
        EditorConnection.instance.Initialize();
        EditorConnection.instance.Register(PlayerControllerToolCommon.kMsgSendToEditor, OnMessageEvent);
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        
        attachProfilerState.Dispose();
        
        EditorConnection.instance.Unregister(PlayerControllerToolCommon.kMsgSendToEditor, OnMessageEvent);
        EditorConnection.instance.DisconnectAll();
    }

    private void OnMessageEvent(MessageEventArgs args)
    {
        //Debug.Log("Message from player");
        
        var command = PlayerControllerToolCommon.PlayerControllerToolBaseCommand.DeSerialize(args.data);
        if (_handlers.TryGetValue(command.GetType(), out var methodInfo))
        {
            methodInfo.Invoke(this, new object[]{command});
        }
        command.ExecuteOnEditor();
        
        
    }
    
    private void OnConnectedState(string player)
    {
        Debug.Log(string.Format("Window connected to {0}", player));
    }

    private Dictionary<Type, MethodInfo> _handlers = new();
    private void InitCommandHandlers()
    {
        _handlers.Clear();
        
        var type = GetType();
        foreach (var method in type.GetMethods(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.DeclaredOnly))
        {
            var handlers = method.GetCustomAttributes(typeof(PlayerControllerToolCommon.ControllerCommandHandlerAttribute), false);
            if (handlers.Length != 0)
            {
                foreach (var handler in handlers)
                {
                    var commandHandler = handler as PlayerControllerToolCommon.ControllerCommandHandlerAttribute;
                    if (commandHandler != null)
                    {
                        _handlers[commandHandler.TargetCommandType] = method;
                        break;
                    }
                }
                
            }
        }
    }

    protected override void OnImGUI()
    {
        // Draw a toolbar across the top of the window and draw the drop-down in the toolbar drop-down style too
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        var rect = GUILayoutUtility.GetRect(100, EditorGUIUtility.singleLineHeight, EditorStyles.toolbarDropDown);
        PlayerConnectionGUI.ConnectionTargetSelectionDropdown(rect, attachProfilerState, EditorStyles.toolbarDropDown);
        EditorGUILayout.EndHorizontal();
        
        var playerCount = EditorConnection.instance.ConnectedPlayers.Count;
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Format("{0} players connected.", playerCount));
        int i = 0;
        foreach (var p in EditorConnection.instance.ConnectedPlayers)
        {
            builder.AppendLine(string.Format("[{0}] - {1} {2}", i++, p.name, p.playerId));
        }
        EditorGUILayout.HelpBox(builder.ToString(), MessageType.Info);

        base.OnImGUI();

    }

    partial void OnReceiveCommand(PlayerControllerToolCommon.PlayerControllerToolBaseCommand command);
    

}


}

