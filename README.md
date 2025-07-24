
# Generic MongoDB Data Access Layer for C#

這是一個基於 C# 和 MongoDB Driver 的泛型資料存取層，  
專門設計用來簡化 MongoDB 的操作，讓你的 Service 層可以專注在業務邏輯，  
不用直接跟 MongoDB 的 Builders 和 Filter 操作搏鬥。

---

## 功能特色

- **泛型支援**：可用於任意 Entity 與 Document 類型  
- **Expression 查詢**：支持用 Expression 樹寫過濾條件，直覺又安全  
- **封裝 Builders**：自動產生 Filter 與 Update 定義，Service 層乾淨又簡潔  
- **自訂索引建立**：支援靜態索引建立，提升查詢效能  
- **易於擴充**：可依需求客製過濾、更新邏輯  
- **MongoDB.Entities 整合**：利用 MongoDB.Entities 實現物件與 MongoDB Document 的雙向映射，並加強資料模型管理與驗證  
- **AutoMapper 支援**：輕鬆實現 Entity 與 Document 的映射，減少重複程式碼

---

## 安裝方式

```bash
# 專案中安裝必要套件
dotnet add package MongoDB.Driver
dotnet add package MongoDB.Entities
dotnet add package AutoMapper
```

---

## 使用範例：用這框架寫一個 POS Service

假設你有一個 POS 系統，裡面包含訂單（Order）和訂單項目（OrderItem）兩個主要資料結構，這些資料都要存取在 MongoDB 裡。

利用這個泛型資料存取層，你可以很方便地：

- **用 AutoMapper 設定 Entity 與 MongoDB Document 之間的映射規則**，這樣資料轉換不需要自己手動寫，減少重複與錯誤。
- **建立專門的資料存取物件 (Repository)**，專責處理訂單資料的查詢、更新等操作，讓 Service 層只管業務邏輯，不用理會資料庫細節。
- **用 Lambda Expression 寫出查詢條件**，例如透過訂單編號快速找出指定訂單，直覺且型別安全。
- **用 Action delegate 簡潔地定義更新內容**，像是修改訂單狀態，語意清晰又易維護。

整體來說，這個架構讓你在寫 POS 服務時，可以把焦點放在「訂單流程」、「付款狀態更新」等業務流程上，  
不必每次操作資料庫都得面對 MongoDB 複雜的 Filter 或 Update 定義，讓開發變得更快、更穩健。

---

## 專案結構

- `MongoEntityDataAccess<TEntity, TDocument>`: 核心資料存取泛型類別  
- `Utils`: 常用工具函式  
- `DTO / Entity`: 業務層與資料層映射物件  
- `Service`: 呼叫資料存取層並實作商業邏輯  
- `MongoDB.Entities`: 提供強型別資料模型與簡化 CRUD 操作的 ODM 框架  
- `AutoMapper`: 負責 Entity 與 Document 之間的映射轉換

---

## 未來規劃

- 支援更靈活的 Update Action 轉換  
- 加入 Transaction 支援  
- 整合更多 NoSQL 功能  
- 增加更完善的錯誤與例外管理  

---

## 聯絡方式

如果你喜歡這個專案，或有任何問題歡迎聯絡我：  
Email: asail0712@gmail.com

---

> 有了這個泛型資料存取層，MongoDB 操作從此不再手忙腳亂，  
> 讓我們一起把專案玩得更乾淨、更優雅吧！🚀
