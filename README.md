# 說明 (目前還是 Alpha)

- 場景切換的小工具，使用可視化編輯流程方式做使用，依賴：
    - [UniTask](https://github.com/Cysharp/UniTask)
    - Addressable (場景資源)
- 目前為測試版，還有很多東西還沒加，例如讀取完成事件等

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
- SceneWorkflowEvent 用來接收事件
    - OnTransitionInComplete
    - OnTransitionOutComplete
    - OnSceneLoaded(string)
    - OnSceneUnloaded(string)

# 範例

![](https://github.com/cowbear6598/SceneTransition/blob/main/Documents/Example.gif)
