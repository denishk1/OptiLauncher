# OptiLauncher

![OptiLauncher](src/OptiLauncher/Assets/optilauncher-banner.png)

## The Ultimate Minecraft Launcher

OptiLauncher is a modern, high-performance Minecraft launcher built with .NET 9 and Avalonia UI. 
Featuring a beautiful Windows 11-inspired design with acrylic blur effects, comprehensive mod 
management, and support for every Minecraft version ever released.

## Features

### 🎨 Modern UI
- Windows 11 Fluent Design with Mica/Acrylic effects
- Dark and Light themes with automatic switching
- Smooth animations and high FPS rendering
- Fully responsive layout

### 🌍 Multi-Language Support
- English
- Русский (Russian)
- Українська (Ukrainian)

### 📦 Complete Version Support
- All official Minecraft versions from Pre-Classic to Latest Release
- Every snapshot, pre-release, and release candidate
- Automatic version fetching from Mojang API
- Advanced version filtering and search

### 🔧 Mod Loader Integration
- Vanilla
- Forge
- NeoForge
- Fabric
- Quilt
- LiteLoader

### ⚡ High-Performance Downloads
- Parallel downloads with configurable threads
- Resume support for interrupted downloads
- SHA1 verification
- Automatic retry with exponential backoff

### 🎮 Advanced Game Management
- Custom RAM allocation
- CPU priority control
- Resolution and fullscreen settings
- JVM argument customization
- Per-instance game directories

### 👤 Account System
- Microsoft authentication
- Ely.by support
- Offline mode
- Multiple account management
- Secure token storage

### 🌐 Server Browser
- Favorite servers list
- Server ping and status
- Online player count
- MOTD display
- Direct join functionality

### 📰 News Feed
- Markdown support
- Images and videos
- Remote JSON feed
- Cached offline viewing

## System Requirements

- **OS:** Windows 10 (1809+) / Windows 11
- **.NET:** .NET 9 Runtime
- **RAM:** 4GB minimum (8GB recommended)
- **Storage:** 1GB free space

## Installation

### Windows
1. Download `OptiLauncher-Setup.exe`
2. Run the installer
3. Launch OptiLauncher from the Start Menu

### Manual Installation
1. Download the latest release
2. Extract to your desired location
3. Run `OptiLauncher.exe`

## Building from Source

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 (17.8+) or Rider

### Build
```bash
git clone https://github.com/OptiLauncher/OptiLauncher.git
cd OptiLauncher
dotnet restore
dotnet build -c Release