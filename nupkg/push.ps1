$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

Set-Location $scriptPath
Set-Location ".\packages"

Get-ChildItem -Filter *.nupkg | ForEach-Object {
    Write-Host "dotnet nuget push $( $_.FullName )"
    & dotnet nuget push $( $_.FullName ) --source https://baget.xiaolipro.cn/v3/index.json
}