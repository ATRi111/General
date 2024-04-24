**这里是从之前参与的所有Unity项目中总结出的代码，不断迭代，这里未必是最新的**

# 文件夹目录

## Addon

### Astar

- A*寻路算法（用于2D正方形网格），包括一些拓展和优化，如权重系数、跳点算法等


### Character

- 与组件有关的一些方法


### MeshExtend

- 方便修改Mesh的工具类，可以用于在C#代码中控制Mesh


### EditorExtend

- 编辑器扩展的通用代码


### MyTimer

- 泛用的、功能多样的定时器类


### Service

- 游戏的基本框架。包括单例，音频，对象池，存档读档，场景加载等
- 此模块依赖EditorExtend模块

### UI

- 常用基本UI组件的拓展
- 此模块依赖EditorExtend模块，MyTimer模块

### StateMathine

- 基本的状态机，可以在此基础上派生出有特定功能的状态机


### Tools

- 各种工具方法


### NodeExtend

- 为了更方便地使用XNode插件而进行的拓展


### xNode-master

- XNode插件（这是一个第三方插件）
