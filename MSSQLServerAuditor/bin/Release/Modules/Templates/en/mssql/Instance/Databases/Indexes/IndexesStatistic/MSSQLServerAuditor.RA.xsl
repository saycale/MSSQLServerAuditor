<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Index statistics" id="IndexesStatisticStatus.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Index statistics" id="IndexesStatisticStatus.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">SQL Server: How Important Are Index Statistics</h1>

				<p>Statistics play a vital role in performance. Accurate statistics about the data held
				in tables are used to provide the best execution strategy for SQL queries. but if the
				statistics don't accurately reflect the current contents of the table you'll get a
				poorly-performing query. How do you find out if statistics are correct, and what can you
				do if the automatic update of statistics isn't right for the way a table is used?</p>

				<p><a href="http://www.databasejournal.com/features/mssql/sql-server-how-important-are-index-statistics.html" target="_blank">SQL Server: How Important Are Index Statistics</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

