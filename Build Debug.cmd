@REM -----------------------------------------------------------------------------------------------
@REM build "Debug" version for the following projects
@REM  1. AuditorLicenser
@REM  2. MSSQLServerAuditor
@REM  3. MSSQLServerAuditorServiceTestApp
@REM -----------------------------------------------------------------------------------------------

@REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Rebuild /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"

C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj                             /t:Rebuild /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
@REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe MSSQLServerAuditorServiceTestApp\MSSQLServerAuditorServiceTestApp.csproj /t:Rebuild /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
@REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe MSSQLServerAuditorService\MSSQLServerAuditorService.csproj               /t:Rebuild /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
