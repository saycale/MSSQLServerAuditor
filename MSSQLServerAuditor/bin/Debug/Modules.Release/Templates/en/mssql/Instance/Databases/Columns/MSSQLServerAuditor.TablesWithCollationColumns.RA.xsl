<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables with columns with different collation" id="TablesWithCollationColumns.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables with columns with different collation" id="TablesWithCollationColumns.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">SQL Server collation conflicts</h1>

				<p>Collations control how strings are sorted and compared. Sorting is not usually a
				problem because it does not cause collation conflicts. It may not sort the way you want
				it to, but it won't cause errors. The real problem here is when you compare data.
				Comparisons can occur several different ways. This can be a simple comparison in a where
				clause, or a comparison in a join condition. By having columns in your database that do
				not match the default collation of the database, you have a problem just waiting to
				happen.</p>

				<p>When you add a new column to an existing table or create a new table with string
				column(s), and you do NOT specify the collation, it will use the default collation of
				the database. If you then write queries that join with existing columns (that has a
				different collation) you will get collation conflict errors.</p>

				<p>Just to be clear here, I am NOT suggesting that every string column should have a
				collation that matches the default collation for the database. Instead, I am suggesting
				that when it is different, there should be a good reason for it. There are many
				successful databases out there where the developers never give any thought to the
				collation. In this circumstance, it's best for the collations for each string column
				match the default collation for the database.</p>

				<p><strong>How to correct it:</strong> To correct this problem, you can modify the
				collation for your existing string columns.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> High</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/sql-server-collation-conflicts" target="_blank">SQL Server Collation Conflicts</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

