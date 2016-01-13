<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures Without Set Nocount On" id="DatabaseProceduresWithoutSetNocountOn.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures Without Set Nocount On" id="DatabaseProceduresWithoutSetNocountOn.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Identify procedures without SET NOCOUNT ON</h1>

				<p>When you have a lot of updates and inserts or deletes in a stored procedure it is
				good to set nocount to on. SQL server can become really chatty when this setting is off;
				this will waste bandwith and CPU power. If a proc is called 30 times a second and you do
				3 DML operations then that will result in 90 n row(s) affected) messages.</p>

				<p>set nocount on should be the first statement in the proc after the parameters and the
				AS.</p>

				<p><strong>How to correct it:</strong> Rewrite your functionality and add 'SET NOCOUNT
				ON'.</p>

				<p><strong>Level of difficulty:</strong> Low</p>

				<p><strong>Level of severity:</strong> Low</p>

				<p><a href="http://www.mssqltips.com/sqlservertip/1226/set-nocount-on-improves-sql-server-stored-procedure-performance/" target="_blank">SET NOCOUNT ON Improves SQL Server Stored Procedure Performance</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

