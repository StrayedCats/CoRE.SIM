# ドキュメンテーション
この文書は [Material for MkDocs](https://squidfunk.github.io/mkdocs-material/) を使用しています。

## ローカルホスティング

1. `MkDocs` 用の Material をインストールします。
   ```
   python3 -m pip install mkdocs-material
   ```

2. ローカルホスティングを開始します。
   ```
   $ cd CoRE.SIM
   $ python3 -m mkdocs serve
   
    INFO     -  Building documentation...
    INFO     -  Cleaning site directory
    INFO     -  Documentation built in 0.16 seconds
    INFO     -  [03:13:22] Watching paths for changes: 'docs', 'mkdocs.yml'
    INFO     -  [03:13:22] Serving on http://127.0.0.1:8000/
   ```

3. ウェブブラウザで `http://127.0.0.1:8000/` にアクセスします。
   <!-- (TODO 画像を変更) -->
   ![](image_0.png)

詳細は [Material for MkDocs - 入門](https://squidfunk.github.io/mkdocs-material/getting-started/) を参照してください。

## MkDocs ファイル
新しいドキュメンテーションファイルには、次の `/docs` ディレクトリと `mkdocs.yml` を使用します。

```
CoRE.SIM
├─ docs/                // 各ドキュメントのマークダウンと画像ファイル
└─ mkdocs.yml           // MkDocs の設定
```

各ドキュメントごとにディレクトリを作成します。例えば、この "Documentation" ページのディレクトリ構造は次のようになるかもしれません。

```
CoRE.SIM
└─ docs/                            // すべてのドキュメントのルート
    └─ DeveloperGuide               // カテゴリ
        └─ Documentation            // 各ドキュメントのルート
            ├─ index.md             // マークダウンファイル
            └─ image_0.png          // マークダウンファイルで使用される画像
```