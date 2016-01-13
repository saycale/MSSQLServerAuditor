<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures With Char Issues" id="DatabaseProceduresWithCharIssues.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures With Char Issues" id="DatabaseProceduresWithCharIssues.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Always include size when using varchar, nvarchar, char and
				nchar</h1>

				<p>There are several string data types in SQL Server. There are varchar, nvarchar, char,
				and nchar. Most front end languages do not require you to identify the length of string
				variables, and SQL Server is no exception. When you do not specify the length of your
				string objects in SQL Server, it applies its own defaults. For most things, the default
				length is one character. This applies to columns, parameters, and locally declared
				variables. The notable exception is with the cast and convert functions which default to
				30 characters.</p>

				<p><strong>How to correct it:</strong> To correct this problem, identify where it exists
				(using the SQL shown above). Then, for each occurrence, identify the size. If the
				problem occurred when declaring a column in a table and you WANT the size to be 1
				character, then specify (1). Other places, you will need to determine the size that it
				should be. Sometimes this involves looking up the size in the table definition.</p>

				<p><strong>Level of difficulty:</strong> Easy.</p>

				<p><strong>Level of severity:</strong> High, because this problem can corrupt your
				data.</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/MSSQLServer/always-include-size-when-using-varchar-n" target="_blank">Always include size when using varchar, nvarchar, char and nchar</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

