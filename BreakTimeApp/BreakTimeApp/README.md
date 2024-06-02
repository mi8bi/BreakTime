## データベースの作成
以下のコマンドを実行しdbフォルダを作成する。   
その後、実行ディレクトリに移動させる。

``` powershell
Add-Migration InitialCreate
Update-Database
Move-Item .\BreakTimeApp\db {実行ディレクトリ}
```

## 参考サイト
 - [Microsoftのデータベースの作成](https://learn.microsoft.com/ja-jp/ef/core/get-started/overview/first-app?tabs=visual-studio#create-the-database)