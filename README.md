- **此框架适用于规模不大的非联网游戏**
- **此框架无需任何修改即可使用（报错可能是因为没有安装某些插件），不过仍可能在开发过程中不断迭代**
- **注意~Documents文件夹，其中包含文档**

# 文件夹目录

## Addon

### Astar

- 用于2D正方形网格的A*寻路算法，可适应移动力、单向通道、可穿过不可停留、困难地形等具体需求
- 此模块依赖EditorExtend模块


### Character

- 目前仅包含角色属性相关逻辑（可用于实现装备、Buff等）
- 此模块依赖EditorExtend模块


### EditorExtend

- 用于简化自定义编辑器的工作流，还包含一些编辑器工具
- 此模块依赖MyTimer模块


### MyTimer

- 功能类似DoTween，需要人为派生来实现各种延时过程
- 此模块依赖EditorExtend模块


### Service

- 游戏的基本框架。包括服务定位器，事件系统，音频，对象池，存档读档，场景加载，资源加载等
- 此模块依赖EditorExtend模块

### UI

- 常用基本UI组件的拓展
- 此模块依赖EditorExtend模块，MyTimer模块

### StateMathine

- 基本的状态机，可以在此基础上派生出有特定功能的状态机


### Tools

- 各种工具方法
- 此模块依赖EditorExtend模块
