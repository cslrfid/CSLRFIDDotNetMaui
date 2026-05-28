# CSLRFIDMobile — .NET MAUI Demo App for CS710S / CS108 RFID Readers

![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android%20%7C%20Windows-blue)
![Language](https://img.shields.io/badge/Language-C%23%20.NET%2010-512BD4)
![MAUI](https://img.shields.io/badge/MAUI-10.0.70-purple)
![CSLibrary](https://img.shields.io/badge/CSLibrary2026-2.0.0--beta.2-orange)
![License](https://img.shields.io/badge/License-MIT-green)

A cross-platform .NET MAUI reference application that drives CSL's **CS710S** and **CS108** handheld UHF RFID readers over Bluetooth Low Energy. The app demonstrates how to consume the `CSLibrary2026` managed library from a MAUI codebase — discovering readers, opening a BLE session, running batch inventory and Geiger-style tag location, and persisting per-device reader configuration.

![App Screen1](/Images/AppScreen1.png)

![App Screen2](/Images/AppScreen2.png)

---

## Architecture

The MAUI app is a **single shared codebase** across iOS, Android, and Windows. Because the BLE plumbing is fully managed (Plugin.BLE wraps CoreBluetooth, Android BLE APIs, and WinRT), there is no per-platform bridge code for reader I/O — only the standard MAUI platform shells under `Platforms/`.

### Project Structure

```
CSLRFIDMobile/
├── App.xaml / AppShell.xaml      # MAUI app shell, Shell routes
├── MauiProgram.cs                # DI container, MAUI builder
├── GlobalUsings.cs
├── View/                         # XAML pages (TinyView)
│   ├── PageMainMenu.xaml
│   ├── PageDeviceList.xaml       # BLE scan + connect
│   ├── PageInventory.xaml        # Batch RFID inventory
│   ├── PageGeigerSearch.xaml     # Tag location / Geiger
│   ├── PageGeigerSettings.xaml
│   ├── PageSetting.xaml          # Settings landing
│   ├── PageTabbedSetting.xaml
│   ├── PageSettingAntenna.xaml          # Per-port power / dwell
│   ├── PageSettingOperation.xaml        # Region, profile, mode
│   ├── PageSettingPowerSequencing.xaml  # Antenna sequence
│   ├── PageSettingAdministration.xaml   # Firmware / OEM
│   └── PageAbout.xaml
├── ViewModel/                    # TinyMvvm view models
├── Model/
│   ├── ReaderConfig.cs           # CONFIG — persisted per-device
│   └── ClassBattery.cs
├── Services/
│   ├── CSLReaderService.cs       # Central reader singleton
│   ├── AppStateService.cs        # appconfig.cfg persistence
│   └── Popups/                   # Syncfusion popup adapters
├── Helper/                       # SoundPlayer, CustomViewCell, …
├── Controls/                     # GradientProgressBar, …
├── Behaviour/                    # MVVM behaviors
├── Converter/                    # Value converters
├── Platforms/                    # Android / iOS / Windows shells
└── Resources/                    # Fonts, images, raw audio
```

### Reader Library Integration

The RFID stack ships as a single NuGet package — `CSLibrary` namespace is provided by **CSLibrary2026 v2.0.0-beta.2**. The app obtains a reader instance through the `CSLReaderService` singleton:

```csharp
public HighLevelInterface? reader = new HighLevelInterface();
// …
reader.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);
```

`HighLevelInterface` exposes six subsystems:

| Property                | Purpose                                            |
|-------------------------|----------------------------------------------------|
| `reader.rfid`           | EPC Gen 2 inventory, read / write / lock / kill    |
| `reader.barcode`        | Onboard 1D / 2D barcode imager (CS108)             |
| `reader.battery`        | Voltage polling, charge state                      |
| `reader.notification`   | Button events, voltage events, error events       |
| `reader.bluetoothIC`    | Bluetooth IC firmware version / control           |
| `reader.siliconlabIC`   | Silicon Labs MCU firmware / reset                  |

`Plugin.BLE` flows in transitively from `CSLibrary2026` — do not add a direct package reference for it.

---

## Dependencies

### Runtime

- [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet)
- [.NET MAUI](https://learn.microsoft.com/dotnet/maui/)
- [CSLibrary2026](https://www.nuget.org/packages/CSLibrary2026/) — RFID reader API
- [Plugin.BLE](https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le) — cross-platform BLE (transitive)
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)
- [TinyMvvm.Maui](https://github.com/dhindrik/TinyMvvm)
- [Syncfusion.Maui.Toolkit](https://www.syncfusion.com/maui-controls)
- [Plugin.Maui.Audio](https://github.com/jfversluis/Plugin.Maui.Audio)
- [SkiaSharp / SkiaSharp.Views.Maui](https://github.com/mono/SkiaSharp)
- [epj.CircularGauge.Maui](https://github.com/ewerspej/maui-circular-gauge)
- [Newtonsoft.Json](https://www.newtonsoft.com/json) — config serialization

### Tooling

- Visual Studio 2022 17.12+ or Visual Studio Code with the .NET MAUI extension
- The `maui-ios` and `maui-android` workloads (`dotnet workload install maui`)
- For Windows builds: Windows 10 Build 26100.0 SDK

---

## Setup

### Prerequisites

- A .NET 10 SDK (the repo's `global.json` pins `10.0.100` with `rollForward: latestMinor`)
- A CS108 or CS710S RFID reader for runtime testing
- iOS 15.0+ device or simulator / Android API 21+ device or emulator / Windows 10 Build 26100.0

### Installation

```bash
# Clone
git clone https://github.com/cslrfid/CSLRFIDDotNetMaui.git
cd CSLRFIDDotNetMaui

# Install MAUI workloads (one-time)
dotnet workload install maui

# Restore packages
dotnet restore CSLRFID.sln

# Build for a specific platform
dotnet build CSLRFIDMobile/CSLRFIDMobile.csproj -f net10.0-ios
dotnet build CSLRFIDMobile/CSLRFIDMobile.csproj -f net10.0-android36.0
dotnet build CSLRFIDMobile/CSLRFIDMobile.csproj -f net10.0-windows10.0.26100.0   # Windows only
```

Open `CSLRFID.sln` in Visual Studio and deploy to a device or emulator, or use `dotnet build … -t:Run` / `dotnet build … -t:Install` from the CLI.

---

## Features

1. **Reader Discovery & BLE Connection** — `PageDeviceList` scans for nearby CS108 / CS710S readers via Plugin.BLE, shows RSSI, and opens a session. Connection state changes are surfaced through `CSLReaderService`.
2. **Batch RFID Inventory** — `PageInventory` runs `Operation.TAG_RANGING` and aggregates EPCs in real time, with running counters and read-rate display.
3. **Geiger Search (Tag Location)** — `PageGeigerSearch` issues a targeted `Operation.TAG_SEARCH` and visualises proximity through `epj.CircularGauge.Maui` plus audible feedback via `Plugin.Maui.Audio`.
4. **Per-Antenna Configuration** — `PageSettingAntenna` configures enable / power / dwell for each of the 16 ports; persisted as part of the per-device JSON config.
5. **Region, Profile & Frequency** — `PageSettingOperation` selects RFID country/region, fixed channel vs. frequency hopping, and link profile (with the CS710S 244→241 adjustment applied automatically).
6. **Power Sequencing** — `PageSettingPowerSequencing` orders the active antenna cycle.
7. **Administration** — `PageSettingAdministration` exposes firmware queries (Silicon Labs MCU, Bluetooth IC) and OEM-level controls.
8. **Battery Monitoring** — `reader.battery` polling cadence is configurable via `RFID_BatteryPollingTime`; voltage events surface through `CSLReaderService.VoltageEvent`.
9. **Per-Device Configuration Persistence** — the `CONFIG` model is serialised to JSON in `FileSystem.Current.AppDataDirectory`, keyed by device GUID; app-level state (linked reader) lives in `appconfig.cfg`.
10. **Support for LED Tags** — tag-write helpers exposed via `reader.rfid` cover LED-tag activation patterns.

---

## Usage

### Connecting to a Reader

1. Launch the app and tap **Device List** from the main menu.
2. Tap **Scan** — nearby readers appear with their advertised name and RSSI.
3. Tap a reader entry; the app opens a BLE session and waits for `INITIALIZATION_COMPLETE` (up to 20 s).
4. On success the saved per-device configuration is loaded automatically; you are returned to the main menu with the reader's serial number shown.

### Running an Inventory

1. From the main menu tap **Inventory**.
2. Confirm region, profile, and antenna power on the **Setting** pages if needed.
3. Press the reader's hardware trigger or tap **Start** — `Operation.TAG_RANGING` begins.
4. EPCs stream into the list; counters update in real time.
5. Press the trigger or tap **Stop** to halt; the list remains for review and can be cleared.

### Locating a Tag (Geiger Search)

1. From the main menu tap **Geiger Search**.
2. Enter or paste the target EPC, then tap **Start**.
3. Move the reader around the search area — the circular gauge climbs as you near the tag and audio feedback accelerates.
4. Tap **Stop** when done.

### Adjusting Reader Configuration

1. From the main menu tap **Setting**.
2. Use the tabbed pages to edit antenna power / dwell, operation mode, power sequencing, or administration parameters.
3. Saving any page persists the change to the device's JSON config and pushes the value to the reader on the next operation.

---

## License

Released under the [MIT License](LICENSE). Copyright © Convergence Systems Limited.
