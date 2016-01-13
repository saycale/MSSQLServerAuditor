<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables with columns text/ntext" id="TablesWithTextColumns.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables with columns text/ntext" id="TablesWithTextColumns.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Don't use text datatype for SQL 2005 and up</h1>

				<p>With SQL Server versions prior to SQL2005, the only way to store large amounts of
				data was to use the text, ntext, or image data types. SQL2005 introduced new data types
				that replace these data type, while also allowing all of the useful string handling
				functions to work. Changing the data types to the new SQL2005+ equivalent should be
				relatively simple and quick to implement (depending on the size of your tables). So, why
				wait? Convert the data types now.</p>

				<p><strong>How to correct it:</strong> Change the data type to a MS SQL 2005+ version.
				Text should be converted to VARCHAR(MAX), NTEXT should be converted to NVARCHAR(MAX) and
				IMAGE should be converted to VARBINARY(MAX).</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Low</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/don-t-use-text-datatype-for-sql-2005-and" target="_blank">Don't use text datatype for MS SQL 2005 and above</a></p>

			</body>
			</html>
			</xsl:template>
		</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

