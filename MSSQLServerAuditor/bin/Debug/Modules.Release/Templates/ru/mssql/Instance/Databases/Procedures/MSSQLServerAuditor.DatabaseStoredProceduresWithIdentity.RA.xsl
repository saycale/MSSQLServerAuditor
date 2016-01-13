<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Хранимые процедуры с @@Identity" id="DatabaseProceduresWithIdentity.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Хранимые процедуры с @@Identity" id="DatabaseProceduresWithIdentity.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">6 способов получить значение текущей идентификации</h1>

				<p>Всегда используйте SCOPE_IDENTITY() за исключением случаев, когда Вам НЕОБХОДИМО узнать последнюю идентификацию независимо от шкалы
				(например, вам нужно узнать идентификацию из вставки таблицы в тригерре).</p>

				<p><strong>Как это исправить:</strong> Используйте SCOPE_IDENTITY().</p>

				<p><strong>Уровень сложности:</strong> Низкий</p>

				<p><strong>Уровень опасности:</strong> Низкий</p>

				<p><a href="http://wiki.lessthandot.com/index.php/6_Different_Ways_To_Get_The_Current_Identity_Value" target="_blank">6 Different Ways To Get The Current Identity Value</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

