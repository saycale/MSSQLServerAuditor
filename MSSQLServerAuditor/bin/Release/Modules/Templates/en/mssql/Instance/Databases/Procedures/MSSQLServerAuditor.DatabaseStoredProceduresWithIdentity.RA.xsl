<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures With Identity" id="DatabaseProceduresWithIdentity.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures With Identity" id="DatabaseProceduresWithIdentity.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">6 Different Ways To Get The Current Identity Value</h1>

				<p>Always use SCOPE_IDENTITY() unless you DO need the last identity value regradless of
				scope (for example you need to know the identity from the table insert inside the
				trigger).</p>

				<p><strong>How to correct it:</strong> use SCOPE_IDENTITY().</p>

				<p><strong>Level of difficulty:</strong> Low</p>

				<p><strong>Level of severity:</strong> Low</p>

				<p><a href="http://wiki.lessthandot.com/index.php/6_Different_Ways_To_Get_The_Current_Identity_Value" target="_blank">6 Different Ways To Get The Current Identity Value</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

