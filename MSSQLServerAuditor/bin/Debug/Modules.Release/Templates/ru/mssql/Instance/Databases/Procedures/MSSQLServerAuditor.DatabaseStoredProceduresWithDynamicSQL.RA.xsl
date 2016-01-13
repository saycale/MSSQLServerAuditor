<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Список хранимых процедур с динамическим SQL" id="DatabaseProceduresWithDynamicSQL.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Список хранимых процедур с динамическим SQL" id="DatabaseProceduresWithDynamicSQL.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Зло и Благо динамического SQL</h1>

				<p><strong>Как исправить:</strong> use sp_executesql instead of exec.</p>

				<p><strong>Уровень сложности:</strong> Низкий</p>

				<p><strong>Уровень опасности:</strong> Низкий</p>

				<p><a href="http://www.sommarskog.se/dynamic_sql.html" target="_blank">Зло и Благо динамического SQL</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

