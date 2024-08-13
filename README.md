# UnityPlayerControllerTool
A tool that allows you to toggle on and off various nodes in unity applications, especially on mobile phones. With this tool, we can quickly toggle nodes to locate the problem. For example, switch nodes on and off and observe the performance changes to quickly identify problems.  
*[REQUIREMENT]Odin  
Usage:  
1.Open Menu: Tools/PlayerControllerToolWindow  
2.Add a gameobject to entry scene with component [PlayerControllerToolRuntime]  
3.Build and run game with "development" option  
4.Connect mobile in [PlayerControllerToolWindow]  
5.In [PlayerControllerToolWindow],open [Hierarchy] tab,click [Collect Hierarchy Data],after a while,the hierarchy tree will be fetched and shown in the panel left.You can pickup one node and toggle it in the panel right.  
6.You can extend the tool, please refer to the implementation of [PlayerControllerToolWindow_1_GamePlay. Custom] and [Command]  
  
一个可以控制unity程序中各个节点开关的工具，特别是手机上的unity程序。有了这个工具，我们就可以快速的开关某个节点，以此定位问题。例如开关节点之后观察性能的变化以便快速定位有问题的模块。  
[需要]Odin  
使用方法：  
1.打开菜单：Tools/PlayerControllerToolWindow  
2.在场景上添加 gameobject,并添加 [PlayerControllerToolRuntime]组件  
3.以"development"选项编译运行游戏  
4.在[PlayerControllerToolWindow]中连接手机  
5.在[PlayerControllerToolWindow]中，打开[Hierarchy] 面板，单击 [Collect Hierarchy Data],等待一会后手机上的游戏的 hierarchy tree 就会显示在左边。你可以选中一个节点后在面板右边开关它  
6.你可以扩展工具，请参考 [PlayerControllerToolWindow_1_GamePlay. Custom] 和 [Command]的实现  

![image](https://github.com/sniffer123/UnityPlayerControllerTool/blob/main/screenshot.png)  

