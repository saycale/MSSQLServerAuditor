<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Instance Dynamic Parameters" id="InstanceDynamicParameters.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Instance Dynamic Parameters" id="InstanceDynamicParameters.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Instance Dynamic Parameters</h1>

				Data based on the virtual table
				<strong>dm_os_performance_counters</strong>, which collect statistics since
				the last instance restart.

				<p><a href="http://technet.microsoft.com/en-us/library/ms187743.aspx" target="_blank">sys.dm_os_performance_counters (Transact-SQL)</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

