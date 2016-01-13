<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Базы данных экземпляра" id="InstanceDatabasesReports.HTML.ru" columns="100" rows="100" splitter="yes">
	<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Базы данных экземпляра" id="InstanceDatabasesReports.HTML.ru">
		<xsl:stylesheet version="1.0"
			  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
			  xmlns:ms="urn:schemas-microsoft-com:xslt"
			  xmlns:dt="urn:schemas-microsoft-com:datatypes">
		<xsl:template match="/">
		<html>
		<head>
		</head>
		<body>
			<style>
				body { overflow: auto; padding:0; margin:0; }
			</style>

			В данном отчёте отображается информация на уровне баз данных

		</body>
		</html>
		</xsl:template>
		</xsl:stylesheet>
	</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
