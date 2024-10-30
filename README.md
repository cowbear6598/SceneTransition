# 說明

- 場景切換的小工具，使用可視化編輯流程方式做使用，依賴：
    - [UniTask](https://github.com/Cysharp/UniTask)

# 安裝

```
Name: OpenUPM
URL: https://package.openupm.com
Scope(s): 
         com.cysharp.unitask   
```

- Open Window => Package Manager
- Add package from git URL

```
https://github.com/cowbear6598/SceneTransition.git?path=Assets/SceneTransition
```

# 使用

- 開啟 SceneTransition -> 流程編輯器即可開始編輯換場景流程
- TransitionIn 的物件需要繼承 SceneTransitionBehaviour，可以觀看專案中 Scripts 裡的 FadeSceneTransition.cs 作為範例
- 建好的 Asset 丟進程式碼並呼叫 Execute 即可跑流程

# 範例