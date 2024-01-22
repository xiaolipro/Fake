@echo off
chcp 65001 > nul

cd %~dp0
cd src

set nuget_source=https://api.nuget.org/v3/index.json
set package_version=8.0.0-preview.240122
set api_key=%1

if "%api_key%"=="" (
    echo 请传入 NuGet 源的 API Key.
    goto :eof
) else (
    echo "%api_key%"
)

for /D %%d in (*) do (
    cd %%d
    dotnet pack --output ../../packages --version-suffix %package_version%
    cd ..
)

set /p confirmPush=是否推送 %package_version% 到 %nuget_source% ? (yes/no): 
if /i "%confirmPush%"=="yes" (
    cd ../packages
    for %%f in (*%package_version%.nupkg) do (
        echo dotnet nuget push %%f --source %nuget_source% -k %api_key%
        dotnet nuget push %%f --source %nuget_source% -k %api_key%
    )
) else (
    echo 取消推送.
)