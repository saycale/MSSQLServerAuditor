<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables Without a Primary Key" id="TablesWithoutAPrimaryKey.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables Without a Primary Key" id="TablesWithoutAPrimaryKey.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Best Practice: Every table should have a primary key</h1>

				<p>By definition, primary keys must contain unique data. They are implemented in the
				database through the use of a unique index. If there is not already a clustered index on
				the table, then the primary key's index will be clustered. It's not always true, but
				most of the time, you want your primary keys to be clustered because it is usually the
				key criteria in your requests to the data. This includes join conditions and where
				clause criteria. Clustered indexes give you exceptional performance because it allows
				SQL Server to create optimal execution plans.</p>

				<p><strong>How to correct it:</strong>Identify tables without a primary key using the
				query above. Examine each table and identify what makes each row in the table unique.
				Modify the table to include a primary key.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/best-practice-every-table-should-have-a" target="_blank">Best Practice: Every table should have a primary key</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

