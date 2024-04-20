# Blockchain Observatory Worker Service

## Description
This background service monitors cryptocurrency prices and sends Telegram alerts to subscribed users.

## Installation

### Prerequisites
- .NET runtime
- Internet connectivity

### Environment Setup
Set the Telegram bot token:
```powershell
setx BlockchainObservatory__PriceAlertWorker__Telegram__BotToken "YOUR_BOT_TOKEN" /M
```

### Service Installation
Deploy the service using the following command:
```powershell
sc.exe create BlockchainObservatory.WorkerService binpath= "..\publish\WorkerService.exe"
```
