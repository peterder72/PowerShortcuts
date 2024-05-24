# PowerShortcuts

[![continuous](https://github.com/peterder72/PowerShortcuts/actions/workflows/continuous.yml/badge.svg)](https://github.com/peterder72/PowerShortcuts/actions/workflows/continuous.yml)

A simple Windows program to add shortcuts that are criminally missing from Windows.
Shortcuts heavily inspired by [dwm](https://dwm.suckless.org/).

Currently only tested on my machine (Windows 11 23H2),
may not (and probably will not) work on other versions due to the undocumented nature of the Virtual Desktop API.

## Current Features

- **No admin privileges** required!
- **Tray icon** for easy access
- Shortcuts for **lightning fast Virtual Desktop** operations.
  - Alt+\<digit> to switch to desktop \<digit> 
  - Alt+Shift+\<digit> to move window in focus to desktop \<digit>
  - Ctrl+Shift+A to toggle pinning the window across all virtual desktops

## Technical features

- Written in **.NET 8.0**
- Uses pre-compiled [VirtualDesktopAccessor](https://github.com/Ciantic/VirtualDesktopAccessor) binary for Virtual Desktop operations
- Uses [nuke](https://github.com/nuke-build/nuke) for build automation
- Undocumented Virtual Desktop switching COM API inspired by [pyvda](https://github.com/mrob95/pyvda)
- Has a **simple installer**
  - Adds a shortcut to the Start Menu
  - Adds itself to **autostart**

# Installation

1. Download the latest .msi installer release from the [releases page](https://github.com/peterder72/PowerShortcuts/releases)
2. Install the software by running the installer
3. The software will **NOT** start automatically after installation (for now), so you can start it from the Start Menu

## Why not AutoHotKey?

Too non-verbose and too much of a hassle to write custom logic with DLL interop.
I will need more complex functionality to be invoked by the shortcuts in the future, so AHK scripting will not do

## Why not pyvda?

Python in painfully slow (~1s to switch desktop), especially with AHK.
Also, distribution and environment setup is a nightmare in Python.

There's great support for COM and dll interop in .NET, and just by rewriting some of the pyvda code in C# 
I was able to achieve a significant speedup in all operations.

## Future plans

- Plugin support for custom shortcut loading
- Support for other Windows versions
