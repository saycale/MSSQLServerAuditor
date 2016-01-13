@REM -----------------------------------------------------------------------------------------------
@REM build "Debug" version for the following projects
@REM  1. MSSQLServerAuditor
@REM -----------------------------------------------------------------------------------------------
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Rebuild /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"

@REM -----------------------------------------------------------------------------------------------
@REM build "Release" version for the following projects
@REM  1. MSSQLServerAuditor
@REM  2. AuditorLicenser
@REM  3. RSAEncryptor
@REM -----------------------------------------------------------------------------------------------
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Rebuild /p:Configuration=Release;Platform=AnyCPU;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Rebuild /p:Configuration=Release;Platform=AnyCPU;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Rebuild /p:Configuration=Release;Platform=AnyCPU;DefineConstants="RELEASE"

@REM -----------------------------------------------------------------------------------------------
@REM build "Trial" version for the following projects
@REM  1. MSSQLServerAuditor
@REM -----------------------------------------------------------------------------------------------
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Rebuild /p:Configuration=Trial;Platform=AnyCPU;DefineConstants="TRIAL"

pause