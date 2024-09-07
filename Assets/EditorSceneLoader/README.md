# Editor Scene Loader
Unity Editor用のSceneローダー。  
Project内のSceneを自動検出し、フォルダごとに分類して読み込みボタンを生成します。  
ホワイトリストとブラックリストを設定できます。

## インストール
1. Unityエディタのメニューから `Window > Package Manager` を選択します。
2. `+` ボタンをクリックし、`Add package from git URL...` を選択します。
3. `https://github.com/yumineko-game/Editor-Scene-Loader.git?path=Assets/EditorSceneLoader`  
を入力し、`Add` ボタンをクリックします。


## 使い方
1. Unityエディタのメニューから `Window > Scene Loader` を選択します。
2. Scene Loaderウィンドウが表示されます。Directoryプルダウンから、読み込み先のフォルダを変更できます。
3. `設定` ボタンをクリックして、ホワイトリストとブラックリストのパターンを設定できます。