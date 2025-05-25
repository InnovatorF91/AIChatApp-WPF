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
### 3. OpenAI API キーの設定
**OpenaiImageService.cs の apiKey パラメータに、OpenAI の API キーを直接設定してください。
```bash
private readonly string apiKey = "あなたの OpenAI API キー";
```
**※ 今後 appsettings.json や環境変数への移行も検討中です。

### 4. アプリを実行
**Visual Studio で CreateAndEditImageApp.sln を開き、F5 キーで実行。

## 🚀  テスト実行方法
**CreateAndEditImageAppTests プロジェクトには、以下の単体テストが含まれています：
```bash
dotnet test
```
**・InputCommand の動作確認
**・OpenAI サービス呼び出し検証
**・UI 操作（Scroll・Capture・Shutdown 等）のモックテスト

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

## 🤝 貢献について
**Pull Request や Issue は大歓迎です！
**1. このリポジトリをフォーク
**2. ブランチを作成：git checkout -b feature/新機能名
**3. コミット：git commit -m '機能追加'
**4. プッシュ：git push origin feature/新機能名
**5. プルリクエストを作成

## 🗂 今後の予定
**このプロジェクトには、今後以下のような新機能の追加を予定しています：
**チャット履歴の保存
**複数画像生成モードの追加
**入力履歴の自動補完
**API エラー時のリトライ機構
**ぜひ ⭐️ Star を付けて、アップデートをお見逃しなく！

## 📄 ライセンス
**本リポジトリは MIT ライセンス の下で公開されています。
**商用・非商用を問わず、自由にご利用いただけます。
