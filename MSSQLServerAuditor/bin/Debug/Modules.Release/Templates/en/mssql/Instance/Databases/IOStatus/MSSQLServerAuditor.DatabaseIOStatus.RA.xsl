<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Disks IO Status" id="DatabaseIOStatus.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Disks IO Status" id="DatabaseIOStatus.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Databases disks IO Status</h1>

				<a href="http://www.mssqltips.com/sqlservertip/2329/how-to-identify-io-bottlenecks-in-ms-sql-server/" target="_blank">How to Identify IO Bottlenecks in MS SQL Server</a>
				<p></p>
				<a href="http://www.databasejournal.com/features/mssql/finding-the-source-of-your-sql-server-io.html" target="_blank">Finding the Source of Your SQL Server Database I/O</a>
				<p></p>
				<a href="http://technet.microsoft.com/en-us/magazine/jj643251.aspx" target="_blank">SQL Server: Minimize Disk I/O</a>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

