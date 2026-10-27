# Nero Names 1.0

在亚历山德里亚猜兄弟小游戏中，按角色身份显示三个兄弟的中文名字。


[English](README.md) | 简体中文

## 环境与兼容性

- Windows Steam 版 FINAL FANTASY IX，已安装 [Memoria](https://github.com/Albeoris/Memoria/releases/tag/v2025.07.04) 的 2025-07-13 热修版本。
- 2025-07-13 是本次发布准备使用的编译与兼容基准；更早版本和 Canary 未验证。
- 本次完成源码编译和压缩包检查，尚未完成发布候选包的游戏内回归。详见 [发布检查清单](RELEASE-CHECKLIST.md)。
- 不包含游戏、Memoria、汉化或 Moguri 的 DLL、素材和存档。游戏引用需从自己的安装目录读取。


## 安装

1. 从 Releases 下载 `NeroNames-v1.0.zip` 并退出游戏。
2. 解压到游戏根目录（有 `FF9_Launcher.exe` 的目录），确认形成 `NeroNames/StreamingAssets/Scripts/Memoria.Scripts.NeroNames.dll`，不要套两层文件夹。
3. 在 Memoria 启动器的 Mods 页面启用本 mod，然后重启游戏。
4. 如果启动器未列出它，在 `Memoria.ini` 的 `[Mod]` 中，将 `"NeroNames"` 追加到现有 `FolderNames` 和 `Priorities` 列表。保留其他 mod 的名称与顺序，不要用示例覆盖整行。


## 卸载

退出游戏，在启动器中停用本 mod，或从 `FolderNames` / `Priorities` 移除其名称，再删除 `NeroNames` 文件夹。

## 显示规则与限制

仅在地图 1858、角色模型 5500 上显示：SID 4 → 泽内罗，SID 7 → 贝内罗，SID 8 → 格内罗。
标签跟随角色身份，不按左、中、右位置猜测。文本目前固定为中文；需要系统字体支持中文字形。
不修改小游戏逻辑、奖励和存档。修改过场景或角色 SID 的大型 mod 未验证。
字体依次尝试 Microsoft YaHei、SimHei、Arial；非 Windows 环境未验证。


## 排错

检查 `Memoria.log` 是否出现 `[NeroNames] Loaded v1.0`。
若文字为空框，检查系统中文字库；若没有标签，检查是否处于正确场景及角色是否可见。


## 组合使用与语言

- FloatingHPBar 与 BattleInfoOverlay 有协作初始化逻辑。单独使用 BattleInfoOverlay 时，浮窗可能要等第一次战斗指令结算后才出现。
- FloatingHPBar 和 ChocoboHotCold 均实现了 `IOverloadOnBattleInitScript`。在当前引擎只保留一个实现的机制下，存在启动钩子覆盖风险；四个 mod 同开尚需游戏内验证。遇到血条不出现时，先只启用 FloatingHPBar 排查。
- BattleInfoOverlay 的界面含固定中文；NeroNames 的名字固定为中文。其他文本取决于游戏及汉化环境。
- 不承诺与所有大型改版兼容。反馈时请附 Memoria 版本、启用顺序、复现步骤与相关日志，并先移除个人路径信息。


## 从源码构建

安装稳定版 .NET SDK 8 或更新版本。构建会读取你提供的游戏目录，只将 DLL 写入本仓库的 `NeroNames` 子目录，不安装到游戏。

Windows PowerShell（在仓库根目录）：

```powershell
./build.ps1 -GamePath 'D:/Games/FINAL FANTASY IX'
./package.ps1 -GamePath 'D:/Games/FINAL FANTASY IX'
```

macOS / Linux 的 Bash 构建入口（仅提供源码构建，运行兼容性未验证）：

```bash
bash build.sh '/path/to/FINAL FANTASY IX'
```

源码位于 `Source/NeroNames.cs`，编译产物位于 `NeroNames/StreamingAssets/Scripts/`，压缩包输出到 `dist/`。DLL 和 dist 默认不纳入 Git，成品单独上传 Releases。直接下载 GitHub 的 Source code ZIP 需要自行编译。


## 作者与许可

作者：**JustPeace**。本 mod 的源码与原创内容按 [MIT License](LICENSE) 发布，允许修改、使用和再分发，须保留版权与许可声明。
FINAL FANTASY IX、Memoria 及第三方 mod 的权利归各自权利人所有。