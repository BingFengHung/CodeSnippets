# CodeSnippets
使用樹狀結構列出定義好的 Code Snippets
在樹狀結構中，可將滑鼠停在標題上，會顯示出內容
雖然 Visual Studio 裡面有內建 Snippet Template 定義的功能，但是有視覺化的操作介面會比較容易一些

## 開啟延伸工具
檢視 -> 其他視窗 -> CodeSnippetsToolWindow

<img src="./misc/how_to_open_codesnippets.png" width="550">

## 如何使用
### 新增 Topic
輸入主題之後，給定項目名稱與內容，會自動新增到指定的 Topic 中

<img src="./misc/add_new_snippets.png" width="350">

### 刪除項目
點選項目旁邊的刪除按鈕即可進行刪除

![delete_item](./misc/delete_item.png)

## 移除延伸工具
移除專案之後記得刪掉在 %APPDATA%\CodeSnippets 資料夾

## Todo
- [ ] 新增編輯功能
- [ ] 每次新增項目完之後不要將 TreeView 合起來
- [ ] 將 Snippet 的預覽放大一點
- [ ] Snippet 分兩塊，上面是該 Snippet 的說明
