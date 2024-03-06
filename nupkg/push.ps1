$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

Set-Location $scriptPath
Set-Location ".\packages"

dotnet nuget push *.* -s xiaolipro.cn --skip-duplicate