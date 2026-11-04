# CELL DEFENDER — STATUS

更新日期：2026-11-04
來源：GitHub main
總狀態：P0 建置基礎 / IN_PROGRESS

| 工作 | 狀態 | 證據/備註 |
|---|---|---|
| GitHub repository | DONE_VERIFIED | SunTung/CELL-DEFENDER，main，可寫入 |
| Unity基礎原型檔案 | DONE_UNVERIFIED | Assets/Scripts/CellGame.cs、Assets/Editor/*.cs |
| Unity Packages/ProjectVersion | DONE_UNVERIFIED | 已提交，尚未驗證Editor版本/套件 |
| GitHub Actions workflow | DONE_UNVERIFIED | .github/workflows/android.yml，尚無成功run證據 |
| Unity授權 | BLOCKED_EXTERNAL | 需確認授權與GitHub Secrets配置 |
| APK產出 | TODO | 尚無成功建置與artifact證據 |
| Android手機安裝 | TODO | 需APK成功後實測 |
| 七敵人/六攻擊/三道具完整功能 | TODO | 原型僅有部分基本行為，不可視為完成 |
| 四類24級技能 | TODO | 設計規格已固定，程式未實作 |
| 六關與無盡模式 | TODO | 規格已固定，程式未實作 |
| 正式16-bit美術 | TODO | 概念視覺不等於Unity可用素材 |

下一個工作：P0-01核對Unity版本及GameCI相容性，修正可建置工程，取得第一個Actions run。
報告主檔：docs/ENGINEERING_EXECUTION_PLAN.md

狀態定義：
- DONE_VERIFIED：有可核實驗收證據。
- DONE_UNVERIFIED：檔案已存在但尚未通過必要測試。
- IN_PROGRESS：正在實作。
- BLOCKED_EXTERNAL：需外部授權、裝置或權限；不阻止其他獨立工作。
- TODO：尚未開始。

## 2026-11-04 框架增量
- DONE_VERIFIED（僅提交存在）：新增8個C#架構檔與 docs/FRAMEWORK_ARCHITECTURE.md。
- DONE_UNVERIFIED：Core/GameSession、DamageSystem、ProgressionSystem、PickupSystem、AttackDefinition、EnemyDefinition、StageDefinition；未經Unity編譯或實機驗收。
- TODO：整合舊CellGame原型與GameSession、實作七種AI/六種攻擊的Runtime、四類技能抽選與創傷技能恢復、六關資料、HUD、美術、存檔。
- BLOCKED_EXTERNAL：Unity授權/Actions實際編譯、Android手機安裝驗收。
- 下一個工作：核實並修正Unity建置設定，啟動Actions取得第一份實際編譯日誌；同時可繼續實作不依賴授權的Runtime。

## 2026-11-04 P0/P1 實際執行（第二批）
- DONE_VERIFIED（GitHub證據）：Actions run #41840030642 已執行且失敗；日誌明確指出缺少Unity授權檔/序號，未進入C#編譯。
- DONE_VERIFIED（僅Git提交）：新增 ProjectileRuntime、PlayerDamageReceiver、EnemyRuntime、AttackPattern、.gitignore。
- DONE_UNVERIFIED：敵人運動/母體事件、追蹤彈、彈道方向與受傷接收器原始碼；尚未與場景Prefab完整連接，也未Unity編譯。
- BLOCKED_EXTERNAL：Unity授權Secrets配置；參考 https://github.com/SunTung/CELL-DEFENDER/actions/runs/41840030642
- TODO：完整六種攻擊生成器（目前僅彈道基礎）、七種敵人完整AI、生成上限、投射物池、正式UI與觸控。
- 下一步：修正Unity版本與專案建置設定；將GameSession、EnemyRuntime及ProjectileRuntime接入可玩場景；等待Unity授權配置後重跑Actions。

## 2026-11-04 執行續報
- 已查到Actions run 41840030642：失敗於Generate scene，後續Build APK及Artifact跳過；目前僅確認步驟級錯誤，未取得詳細日誌。
- 新增 PlayerRuntime.cs 與 WaveSpawner.cs；既有 ProjectileRuntime.cs、EnemyRuntime.cs 已存在，因此保留現有版本，不覆蓋。
- GameSession.cs 增加 CollectEnergy(int) 接口，供敵人死亡掉落能量使用。
- DONE_UNVERIFIED：以上原始碼已提交，未完成Unity編譯/手機測試。
- TODO：將ProjectileRuntime與EnemyRuntime的實際API整合，建立Prefab/Scene/波次資料與六種武器策略；修復Actions建置。

## 2026-11-04 後續建構
- 已取得 Actions job 126608859094 完整日誌：根因是 Unity License / Serial 缺失；並非已證明場景編譯錯誤。
- 已新增 RuntimeBootstrap；CreateGame 改為產生 Camera、GameSession、Player、敵人/子彈模板及RuntimeBootstrap。
- EnemyRuntime/ProjectileRuntime 已整合基本移動、射擊、碰撞、死亡獎勵；七種AI仍有未完成能力，六種攻擊尚非完整獨立策略。
- 修正玩家子彈模板實例啟用、BuildAndroid UnityEditor.Build namespace、Unity版本檔移除未核實revision、Actions增加授權預檢。
- DONE_UNVERIFIED：上述程式碼已提交，但尚未Unity編譯；場景暫用Editor動態Sprite，資產序列化與渲染仍需實際驗證或改用持久資產。
- BLOCKED_EXTERNAL：Unity授權Secrets未配置；需要使用者在GitHub Settings → Secrets and variables → Actions處理。不能提交授權密碼或序號至Git。
- 下一步：取得授權後執行Actions；依實際編譯日誌修復；接著完成場景持久Sprite、UI、技能選擇與道具實體。

## 2026-11-04 P1 推進：技能選擇、HUD、持久Sprite
- 已提交 GameSession：從未滿級的四類技能中隨機抽取最多3項，進入SkillChoice時暫停，選取後恢復；移除暫時性自動加點。
- 已提交 GameHud：OnGUI 9:16適配比例、HP/護罩/能量/關卡、三選一、暫停/恢復、GameOver/重開介面。
- 已提交 CreateGame：在Editor產生 Assets/Generated/pixel.png，經TextureImporter設定Sprite/Point後以持久Asset引用進入場景；場景新增HUD。
- DONE_UNVERIFIED：GitHub原始碼提交已成功，尚未Unity編譯或Android實機驗證。
- 已知待修：角色重新開始時場上敵人/子彈未清空；完整技能效果、敵人母體生怪、六種攻擊全行為、波次關卡資料、音效與存檔仍待做。
- BLOCKED_EXTERNAL：Actions Unity License尚未配置，先前run 41840030642的實際錯誤是Missing Unity License。
