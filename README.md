# AIChatApp-WPF

🎨 **OpenAI 公式 API を活用した、WPF + Prism 製のデスクトップチャットアプリです。**  
画像生成機能を搭載し、MVVM + 単体テスト構成により高い保守性・拡張性を実現しています。

---

## 🧠 主な機能

- 🖼️ **OpenAI による画像生成**：ユーザーが入力したテキストに基づき画像を生成。
- 📐 **MVVM + Prism アーキテクチャ**：疎結合・テスト可能なコード構成。
- 🧪 **単体テスト対応**：xUnit + Moq による ViewModel テスト完備。
- 🧩 **UI 操作を抽象化**：`ScrollViewer` などの UI 操作をインターフェースとして抽出。
- 💬 **XAML によるイベント制御**：`CommandParameter` を通じて要素をバインド。

---

## 🛠️ 使用技術

| 項目             | 内容                              |
|------------------|-----------------------------------|
| プログラミング言語 | C# (.NET 8)                      |
| UI フレームワーク   | WPF + Prism                      |
| テスト           | xUnit + Moq                      |
| DI コンテナ      | Prism の標準 DI コンテナ            |
| 外部 API        | OpenAI 公式画像生成 API             |

---

## 🚀 開始方法

### 1. このリポジトリをクローン

```bash
git clone https://github.com/InnovatorF91/AIChatApp-WPF.git
cd AIChatApp-WPF
```

### 2. 依存関係の復元
```bash
dotnet restore
```

### 3. アプリを実行
Visual Studio で CreateAndEditImageApp.sln を開き、F5 キーで実行。<br>

## 🚀  テスト実行方法
CreateAndEditImageAppTests プロジェクトには、以下の単体テストが含まれています：<br>
```bash
dotnet test
```
InputCommand の動作確認<br>
OpenAI サービス呼び出し検証<br>
UI 操作（Scroll・Capture・Shutdown 等）のモックテスト<br>

## 📁 プロジェクト構成
```bash
AIChatApp-WPF/
├── CreateAndEditImageApp/         # 本体アプリケーション
│   ├── Views/                     # ビュー
│   ├── ViewModels/                # ビューモデル
│   ├── Services/                  # サービス層
│   └── App.xaml                   # アプリのエントリーポイント
├── CreateAndEditImageAppTests/    # テストプロジェクト
│   └── MainViewModelTests.cs      # 単体テスト例
└── README.md                      # このファイル
```

### 🧡 貢献について

Pull Request や Issue は大歓迎です！<br>
i. このリポジトリをフォーク<br>
ii. ブランチを作成：`git checkout -b feature/新機能名`<br>
iii. コミット：`git commit -m '機能追加'`<br>
iv. プッシュ：`git push origin feature/新機能名`<br>
v. プルリクエストを作成<br>

### 📅 今後の予定

このプロジェクトには、今後以下のような新機能の追加を予定しています：<br>
・チャット履歴の保存<br>
・複数画像生成モードの追加<br>
・入力履歴の自動補完<br>
・API エラー時のリトライ機構<br>
ぜひ ⭐️ Star を付けて、アップデートをお見逃しなく！<br>

### 📄 ライセンス

本リポジトリは MIT ライセンスの下で公開されています。<br>
商用・非商用を問わず、自由にご利用いただけます。<br>


---

## 🌐 サーバー構成（ASP.NET Core Web API）

このアプリは、OpenAI API 呼び出しを安全かつ拡張可能に行うため、**WPF クライアントと分離された Web API サーバー**をローカルに構築しています。

- 🔗 クライアントは `Repositories` 層から API リクエストを送信
- 🧠 サーバーは `Controllers` → `Logics` → `Services` の三層構造で処理
- 🔐 OpenAI API キーはサーバー側にのみ保持し、クライアントには公開されません

### ✅ 主な構成

| 層          | 説明                                      |
|-------------|-------------------------------------------|
| `Controllers` | クライアントからのリクエストを受け取り、レスポンスを返す |
| `Logics`      | 業務ロジック処理、バリデーションなど             |
| `Services`    | OpenAI API との直接通信を担当                   |

---

### 🚀 サーバーの起動方法

#### 1. Web API プロジェクトの実行（Visual Studio で）
`CreateAndEditImageServer` プロジェクトを右クリック → 「スタートアッププロジェクトに設定」→ 実行（F5）

#### 2. 実行後のエンドポイント確認（例）
```
https://localhost:7110/api/OpenaiImage/GetImage
```

#### 3. クライアントからの呼び出し例（Repositories 経由）
```csharp
await _httpClient.PostAsJsonAsync("/api/OpenaiImage/GetImage", new { prompt });
```
#### 4. OpenAI API キーの設定
OpenaiImageService.cs の apiKey パラメータに、OpenAI の API キーを直接設定してください。<br>
```bash
private readonly string apiKey = "あなたの OpenAI API キー";
```
※ 今後 appsettings.json や環境変数への移行も検討中です。<br>
---

## 🧪 サーバー側の単体テスト

### ✅ テストフレームワーク

- `xUnit` + `Moq` によりコントローラーおよびサービス層をモックベースで検証
- `OpenaiImageController` と `OpenaiImageService` の例外処理や応答の正常性をテスト済み

### 🔍 テスト対象の例

| テスト名 | 検証内容 |
|----------|----------|
| `GenerateImageAsync_normal_returnOK` | 正常系の画像生成リクエスト処理 |
| `GenerateImageAsync_ThrowsInvalidOperationException_WhenBillingLimitReached` | OpenAI の課金上限エラーを適切にハンドリングするか |
| `Controller における 400/500 応答の返却検証` | クライアントがエラーを受け取れることを検証 |

---

### 🧪 テストの実行（全体）

```bash
dotnet test
```

`CreateAndEditImageServerTests` プロジェクト内のすべての Controller/Service 単体テストが実行されます。

---

### 🔐 セキュリティ対応

- GitHub Secret Scanning により API キーが push された際、自動検知で push 拒否されるよう対策済み
- `.gitignore` で `appsettings.*.json` や `.user` ファイルを除外

---

### 💡 ワンポイント補足

- `wwwroot` フォルダは静的ファイルを使わない場合、`app.UseStaticFiles()` を削除して安全に除外可能
- API キーは今後環境変数または Secret Manager による安全な管理へ移行予定
