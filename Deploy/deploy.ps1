$sourceDir = "PowerShortcuts"
$startupDir = [Environment]::GetFolderPath([Environment+SpecialFolder]::Startup)
$destinationDir = Join-Path -Path $startupDir -ChildPath "PowerShortcuts"

# 1. Stop the PowerShortcuts.Host process if it is running
Get-Process | Where-Object {$_.ProcessName -eq "PowerShortcuts.Host"} | Stop-Process -Force

# 2. Remove the existing deploy directory if it exists
If (Test-Path $destinationDir) {
    Remove-Item -Path $destinationDir -Recurse -Force
}

# 3. Copy the PowerShortcuts directory to the startup directory
Copy-Item -Path $sourceDir -Destination $startupDir -Recurse -Force

# 4. Run the PowerShortcuts.Host in the startup directory
$executablePath = Join-Path -Path $destinationDir -ChildPath "PowerShortcuts.Host.exe"
Start-Process -FilePath $executablePath -NoNewWindow
