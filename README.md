# Nero Names 1.0

English | [简体中文](README.zh-CN.md)

Shows fixed Chinese labels above Zenero, Benero and Genero during the Alexandria shuffle minigame. Labels follow actor identity rather than left, middle or right screen positions.

## Display rules

The overlay is restricted to field map 1858 and model 5500:

| Actor SID | Label | Identity |
| --- | --- | --- |
| 4 | 泽内罗 | Zenero |
| 7 | 贝内罗 | Benero |
| 8 | 格内罗 | Genero |

There is no INI configuration in this version. Disable it through the launcher or remove its entry from `Memoria.ini`.

## Limitations and troubleshooting

- Names are currently hardcoded in Chinese. A font with Chinese glyphs is required.
- Fonts are tried in this order: Microsoft YaHei, SimHei, Arial. Non-Windows runtime compatibility has not been verified.
- The mod does not change minigame logic, rewards or saves. Mods that replace actors or change SIDs require separate testing.
- Check `Memoria.log` for `[NeroNames] Loaded v1.0`. If text renders as boxes, check Chinese font availability. If labels are absent, confirm the map, actor visibility and mod activation.

## Requirements and validation status

- Windows Steam edition of FINAL FANTASY IX with [Memoria v2025.07.04, including the 2025-07-13 hotfix](https://github.com/Albeoris/Memoria/releases/tag/v2025.07.04).
- Memoria 2025-07-13 is the current compilation and compatibility baseline. Earlier versions and Canary have not been verified.
- Source compilation and package structure checks have passed. These release candidates have **not yet completed in-game regression testing**. See [the release checklist](RELEASE-CHECKLIST.md), currently written in Chinese.
- Game files, Memoria binaries, translations, Moguri assets and save files are not included. Build references must come from your own game installation.

## Installation

1. Download `NeroNames-v1.0.zip` from Releases and close the game.
2. Extract it into the game directory containing `FF9_Launcher.exe`. The resulting path must be `NeroNames/StreamingAssets/Scripts/Memoria.Scripts.NeroNames.dll` without an extra nested mod folder.
3. Enable the mod in the Memoria launcher's Mods tab and restart the game.
4. If it is not listed, append `"NeroNames"` to the existing `FolderNames` and `Priorities` lists in the `[Mod]` section of `Memoria.ini`. Preserve the other entries and their order.

GitHub's automatic **Source code** archives do not include compiled DLLs. Use a release attachment or build the source yourself.

## Uninstall

Close the game, disable this mod in the launcher or remove its name from `FolderNames` / `Priorities`, then delete the `NeroNames` folder.

## Combining mods

FloatingHPBar and BattleInfoOverlay have cooperative initialisation logic. BattleInfoOverlay alone may appear only after the first battle action resolves.

FloatingHPBar and ChocoboHotCold both implement `IOverloadOnBattleInitScript`. The current engine retains a single implementation, so one mod can override the other's hook. Running all four mods together and their loading order still require in-game verification. If health bars are missing, test FloatingHPBar on its own first.

BattleInfoOverlay has Chinese interface labels, and NeroNames uses fixed Chinese names. Other text depends on the game and installed translation. Compatibility with large gameplay overhauls is not guaranteed.

## Building and packaging

Install a stable .NET SDK, version 8 or newer. Run these commands from the repository root:

```powershell
./build.ps1 -GamePath 'D:/Games/FINAL FANTASY IX'
./package.ps1 -GamePath 'D:/Games/FINAL FANTASY IX'
```

The build reads references from `x64/FF9_Data/Managed` in the supplied game directory and writes the DLL into this repository's `NeroNames/StreamingAssets/Scripts` directory. It does not install the mod or modify the game directory.

An additional Bash build entry point is provided; it has not been run on macOS or Linux:

```bash
bash build.sh '/path/to/FINAL FANTASY IX'
```

Source: `Source/NeroNames.cs`. Packages and SHA-256 checksums: `dist/`. Generated DLLs and `dist/` are ignored by Git; upload the ZIP separately as a Release attachment. Packaging refuses to overwrite an existing ZIP; rename or remove that ZIP before rebuilding a package.

## Reporting problems

Include the mod version, Memoria version, operating system, resolution, game language, active mod order, reproduction steps and relevant `Memoria.log` excerpts. Remove personal paths before sharing logs. Screenshots are helpful for layout problems.

## Author and license

Author: **JustPeace**. Original code and content in this mod are released under the [MIT License](LICENSE). Retain the copyright and license notice when redistributing.

FINAL FANTASY IX, Memoria and third-party mods remain the property of their respective rights holders. This license does not grant rights to their code or assets.