# 代码

## IService

- IService是最基本的接口。**直接继承IService的每种接口（以下称为D类接口）**分别代表了一种功能/服务
- 可能会有若干个不同的类直接或间接地实现D类接口，它们代表了同一种功能的不同实现
- 通常，对于每个D类接口，同时至多有一个实现该接口的实例存在

## Service

- 各种适合写成单例的类，可以不写成单例，而是继承Service，Service类会自动将自身注册到ServiceLocator
- Service实例要获取其他Service实例，仅仅在变量上添加`[Other]`特性即可

- 非Service实例要获取Service实例，访问`ServiceLocator`

- 初始化应当写在`Initialize`方法中，而不是`Awake`或`Start`

## ServiceLocator

- 唯一的单例，用于获取各种Service
- 各种Service是通过Type来区分的。获取Service子类时要指定Type。这个Type不是要获取的类的Type，而是该类实现的D类接口的Type（对于每个D类接口，同时至多有一个实现该接口的实例存在，但运行前或运行时，该实例可能会改变。如果以D类接口的类型为参数，当实现D类接口的具体Service改变时，获取Service的语句就不用修改了）

# 工作流

