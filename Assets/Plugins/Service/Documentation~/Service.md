## 概述

Service类用于代替单例

## Service

- 原本的单例类可以改为继承Service，Service类会自动将自身注册到ServiceLocator
- 如果要获取其他Service，仅使用`[Other]`特性即可（这个特性只能用在Service子类中，其他类通过ServiceLocator获取）

- 初始化写在`Initialize`方法中，而不是`Awake`或`Start`

## ServiceLocator

- 唯一的单例，用于获取各种Service
