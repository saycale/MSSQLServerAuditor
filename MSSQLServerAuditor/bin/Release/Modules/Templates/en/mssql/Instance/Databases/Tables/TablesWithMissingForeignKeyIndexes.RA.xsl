<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables With Missing Foreign KeyIndexes" id="TablesWithMissingForeignKeyIndexes.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables With Missing Foreign KeyIndexes" id="TablesWithMissingForeignKeyIndexes.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Missing Foreign Key Indexes</h1>

				<p>One way to improve performance in SQL Server is simply to ensure that there﻿ are
				indexes on all foreign key. This is NOT done automatically, which is kind of surprising
				given how much of an impact such a change would have on the performance of a significant
				install base of SQL Server.</p>

				<p><strong>How to correct it:</strong> Review the tables and create the indexes</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://hazaa.com.au/blog/sql-server-generating-sql-for-missing-foreign-key-indexes/" target="_blank">Missing Foreign Key Indexes</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

