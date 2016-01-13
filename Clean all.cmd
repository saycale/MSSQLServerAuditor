@REM -----------------------------------------------------------------------------------------------
@REM build "Debug" version for the following projects
@REM  1. MSSQLServerAuditor
@REM -----------------------------------------------------------------------------------------------
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Debug;Platform=x86;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Debug;Platform=x64;DefineConstants="DEBUG"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Debug;Platform=x86;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Debug;Platform=x64;DefineConstants="DEBUG"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Debug;Platform=x86;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Debug;Platform=x64;DefineConstants="DEBUG"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe TestApp\TestApp.csproj                       /t:Clean /p:Configuration=Debug;Platform=AnyCPU;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe TestApp\TestApp.csproj                       /t:Clean /p:Configuration=Debug;Platform=x86;DefineConstants="DEBUG"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe TestApp\TestApp.csproj                       /t:Clean /p:Configuration=Debug;Platform=x64;DefineConstants="DEBUG"

@REM -----------------------------------------------------------------------------------------------
@REM build "Release" version for the following projects
@REM  1. MSSQLServerAuditor
@REM  2. AuditorLicenser
@REM  3. RSAEncryptor
@REM -----------------------------------------------------------------------------------------------
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Release;Platform=AnyCPU;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Release;Platform=x86;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Release;Platform=x64;DefineConstants="RELEASE"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Release;Platform=AnyCPU;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Release;Platform=x86;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Release;Platform=x64;DefineConstants="RELEASE"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Release;Platform=AnyCPU;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Release;Platform=x86;DefineConstants="RELEASE"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Release;Platform=x64;DefineConstants="RELEASE"

@REM -----------------------------------------------------------------------------------------------
@REM build "Trial" version for the following projects
@REM  1. MSSQLServerAuditor
@REM -----------------------------------------------------------------------------------------------
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Trial;Platform=AnyCPU;DefineConstants="TRIAL"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Trial;Platform=x86;DefineConstants="TRIAL"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe MSSQLServerAuditor\MSSQLServerAuditor.csproj /t:Clean /p:Configuration=Trial;Platform=x64;DefineConstants="TRIAL"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Trial;Platform=AnyCPU;DefineConstants="TRIAL"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Trial;Platform=x86;DefineConstants="TRIAL"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe AuditorLicenser\AuditorLicenser.csproj       /t:Clean /p:Configuration=Trial;Platform=x64;DefineConstants="TRIAL"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Trial;Platform=AnyCPU;DefineConstants="TRIAL"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Trial;Platform=x86;DefineConstants="TRIAL"
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RSAEncryptor\RSAEncryptor.csproj             /t:Clean /p:Configuration=Trial;Platform=x64;DefineConstants="TRIAL"

pause
