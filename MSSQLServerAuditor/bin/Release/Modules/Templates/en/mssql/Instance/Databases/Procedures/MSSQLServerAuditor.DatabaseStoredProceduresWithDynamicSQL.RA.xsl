<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures With Dynamic SQL" id="DatabaseProceduresWithDynamicSQL.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures With Dynamic SQL" id="DatabaseProceduresWithDynamicSQL.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">The Curse and Blessings of Dynamic SQL</h1>

				<p><strong>How to correct it:</strong> use sp_executesql instead of exec.</p>

				<p><strong>Level of difficulty:</strong> Low</p>

				<p><strong>Level of severity:</strong> Low</p>

				<p><a href="http://www.sommarskog.se/dynamic_sql.html" target="_blank">The Curse and Blessings of Dynamic SQL</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

