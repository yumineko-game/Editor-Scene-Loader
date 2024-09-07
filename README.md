# Editor Scene Loader
 ![2024-09-07_21h34_45](https://github.com/user-attachments/assets/7c33a5a6-d058-42ce-8047-9c3107e2c2a3)

Unity Editor用のSceneローダー。  
Project内のSceneを自動検出し、フォルダごとに分類して読み込みボタンを生成します。  
ホワイトリストとブラックリストを設定できます。  

## インストール(UPM)
```
https://github.com/yumineko-game/Editor-Scene-Loader.git?path=Assets/EditorSceneLoader
```

## 使い方
1. Unityエディタのメニューから `Window > Scene Loader` を選択します。
2. Scene Loaderウィンドウが表示されます。Directoryプルダウンから、読み込み先のフォルダを変更できます。
3. `設定` ボタンをクリックして、ホワイトリストとブラックリストのパターンを設定できます。
  
  
![2024-09-07_21h34_38](https://github.com/user-attachments/assets/63438305-331c-4d6f-b528-7283a4e2c4af)  
この場合は「Plugins」以外の「Assets」フォルダだけを対象にします。
