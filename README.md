# MSSQLServerAuditor

The purpose of the MSSQLServerAuditor utility is to make the complete audit of the running MS SQL server and provide the details analysis about the most important aspects of the MS SQL server. It is expected, that based on the results of the audit, the qualified database administrator may create the recommendations to resolve the issues and problems, found by the utility. The supported versions of MS SQL servers are: 2000, 2005, 2008, 2008 R2, 2012 and 2014. 

The utility running in the read only-mode and check many aspects the MS SQL Server. The utility is designed to run on production system and can be configure to affect on the database system as minimum as it possible. Internally, it is a .Net program, written on C# and running under MS Windows. Currently 32 and 64-bit Windows (desktop versions 7,8 and Server platforms MS Windows 2008, 2012) with .Net 4.5.x are supported.

The key features of the MSSQLServerAudit utility are the followings:

•	The application is stand alone (no other tools, such as MS Management Studio are required to use the program)
•	The audit databases is inspected in a “read-only mode”. No database objects are created on the inspected MS SQL instance.
•	The utility may be connected to many MS SQL instances and generate consolidation reports about many instances on the one page.
•	All collected information from the inspected MS SQL instances are stored internally in SQLite database.
•	Multitasking application: reports are generated in parallel by many working threads
•	To minimize affect on the production MS SQL instance, the number of threads for MS SQL databases queries is limited.

Requirements:

1. Windows OS
2. .Net 4.x Framework

How to run:

1. Download repository
2. Check the file "Build all64.cmd" to fix the location of MSBuild.exe. The default location is "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\"
3. Build project by "Build all64.cmd"
4. Start "MSSQLServerAuditor\bin\Debug\MSSQLServerAuditor.exe"
