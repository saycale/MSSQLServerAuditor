<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures With Decimal Issues" id="DatabaseProceduresWithDecimalIssues.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures With Decimal Issues" id="DatabaseProceduresWithDecimalIssues.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Always include precision and scale with decimal and
				numeric</h1>

				<p>When you use the decimal (or numeric) data type, you should always identity the
				precision and scale for it. If you do not, the precision defaults to 18, and the scale
				defaults to 0. When scale is 0, you cannot store fractional numbers. If you do not want
				to store fractional numbers, then you should use a different data type, like bigint,
				int, smallint, or tinyint.</p>

				<p><strong>How to correct it:</strong> Use the query above to locate this problem with
				your code. Specify the precision and scale. This will often times require that you look
				up the proper precision and scale in a table definition.</p>

				<p><strong>Level of difficulty:</strong> Easy.</p>

				<p><strong>Level of severity:</strong> High.</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/always-include-precision-and-scale-with" target="_blank">Always include precision and scale with decimal and numeric</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

