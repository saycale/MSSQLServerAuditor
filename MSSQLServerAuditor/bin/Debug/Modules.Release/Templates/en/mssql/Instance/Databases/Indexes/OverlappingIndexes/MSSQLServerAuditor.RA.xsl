<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Overlapping Indexes" id="OverlappingIndexes.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Overlapping Indexes" id="OverlappingIndexes.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Overlapping Indexes</h1>

				<p>SQL Server allows the creation of multiple non-clustered indexes, with a
				maximum of 999 in the SQL 2008 release (compared to 249 in 2005 release);
				the only limitation is that the "Index Name" must be unique for the schema.
				This could mean that some indexes might actually be duplicates of each other
				in all but their name, also known as exact duplicate indexes. If this
				happens, it can waste precious SQL Server resources and generate unnecessary
				overhead, causing poor database performance.</p>

				<p><strong>How to correct it:</strong> Remove unused indexes.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://www.confio.com/logicalread/duplicate-indexes-and-sql-server-performance/" target="_blank">Duplicate Indexes and SQL Server Performance</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

