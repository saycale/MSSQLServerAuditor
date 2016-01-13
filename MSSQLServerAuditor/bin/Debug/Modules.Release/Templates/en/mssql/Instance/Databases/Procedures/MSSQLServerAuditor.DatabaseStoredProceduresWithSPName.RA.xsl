<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures With SP Name" id="DatabaseProceduresWithSPName.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures With SP Name" id="DatabaseProceduresWithSPName.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Don't start your procedures with SP_</h1>

				<p>When SQL Server executes a stored procedure, it first checks to see if it is a
				built-in stored procedure (system supplied). It checks the master database for the
				existence of this procedure. If the procedure is not found, it will search the user
				database. It doesn't sound like much, but in a high transaction environment, the slight
				performance hit will add up.</p>

				<p>Also, consider what would happen if Microsoft decides to ship a system stored
				procedure with the same name as the procedure you wrote. Suddenly, your procedure will
				stop working and the one supplied by Microsoft will be executed instead. To see what I
				mean, try creating a stored procedure in your database named sp_help. When you execute
				this stored procedure, SQL will actually execute the one in the master database
				instead.</p>

				<p><strong>How to correct it:</strong> To correct this problem, you will need to
				identify all procedures named this way, and then change the name of the procedure. There
				are far greater implications though. Some stored procedures are called by other stored
				procedures. In cases like this, you will need to change those stored procedures too.
				Additionally, you will also need to change your front end code to call the procedure
				with the new name.</p>

				<p><strong>Level of difficulty:</strong> Medium to high. The level of effort required to
				correct this problem can range from medium to high, depending on how many procedures you
				have than require a name change.</p>

				<p>One possible strategy you could use to help resolve this problem would be to rename
				the procedure, and then create a procedure with the original name. This procedure could
				write to a log file, and then call the original procedure. This strategy allows your
				application to continue working (albeit a little slower because of the logging). You can
				then determine which application ran the procedure and change the name of the call.</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/MSSQLServer/don-t-start-your-procedures-with-sp_" target="_blank">Don't start your procedures with SP_</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

