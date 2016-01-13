<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Wide table check" id="WideTables.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Wide table check" id="WideTables.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Wide table check</h1>

				<p>The wide table check will check if the sum of all the max values of the columns in
				your table exceeds 8,060 bytes.</p>

				<p>In SQL Server 2008, the restriction that a table can contain a maximum of 8,060 bytes
				per row is relaxed for tables that contain varchar, nvarchar, varbinary, sql_variant, or
				CLR user-defined type columns. The length of each one of these columns must still fall
				within the limit of 8,000 bytes; however, their combined widths can exceed the
				8,060-byte limit. Here is what Books On Line says you have to consider:</p>

				<p>Surpassing the 8,060-byte row-size limit might affect performance because SQL Server
				still maintains a limit of 8 KB per page. When a combination of varchar, nvarchar,
				varbinary, sql_variant, or CLR user-defined type columns exceeds this limit, the SQL
				Server Database Engine moves the record column with the largest width to another page in
				the ROW_OVERFLOW_DATA allocation unit, while maintaining a 24-byte pointer on the
				original page.</p>

				<p>Moving large records to another page occurs dynamically as records are lengthened
				based on update operations. Update operations that shorten records may cause records to
				be moved back to the original page in the IN_ROW_DATA allocation unit. Also, querying
				and performing other select operations, such as sorts or joins on large records that
				contain row-overflow data slows processing time, because these records are processed
				synchronously instead of asynchronously.</p>

				<p><strong>How to correct it:</strong> So be aware of the implications of having wide
				tables, if you can, move these columns tho their own tables, this will also help when
				your developers specify SELECT * when they really only want 2 columns out of the
				table</p>

				<p><strong>Level of difficulty:</strong> Moderate</p>

				<p><strong>Level of severity:</strong> Mild</p>

				<p><a href="http://wiki.lessthandot.com/index.php/SQLCop_wide_table_check" target="_blank">Wide table check</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

