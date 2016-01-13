<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables With a Prefix" id="TablesWithPrefix.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables With a Prefix" id="TablesWithPrefix.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Don't prefix your table names with tbl</h1>

				<p>This is a naming convention issue. Tables should not be prefaced with tbl because it
				does nothing to add to the clarity of the code (self-documenting). It actually has the
				opposite effect because it takes longer to read it and causes you to perform an
				interpretation of the three letter abbreviation.</p>

				<p><strong>How to correct it:</strong> Rename the table to remove the prefix. This is
				not as simple as it seems because this table could be referenced from a number of
				places, including views, stored procedures, user defined functions, index creation
				scripts, in-line SQL (embedded within front end applications), etc...</p>

				<p><strong>Level of difficulty:</strong> moderate to high</p>

				<p><strong>Level of severity:</strong> Mild</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/MSSQLServer/don-t-prefix-your-table-names-with-tbl" target="_blank">Don't prefix your table names with tbl</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

