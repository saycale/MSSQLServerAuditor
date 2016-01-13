<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables with columns type inconsistency" id="TableColumnsWithInconsistency.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables with columns type inconsistency" id="TableColumnsWithInconsistency.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Columns Inconsintency Check</h1>

				<p>Frequently data types, length and other field attributes change during
				development, so the report that highlights possible mistakes (same column
				name but different definition) was created.</p>

				<p><strong>How to correct it:</strong> To correct this problem, you can
				modify the types for some of your columns.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://sqlapproach.blogspot.com.au/2013/02/columns-inconsintency-check.html" target="_blank">Columns Inconsintency Check</a></p>
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

