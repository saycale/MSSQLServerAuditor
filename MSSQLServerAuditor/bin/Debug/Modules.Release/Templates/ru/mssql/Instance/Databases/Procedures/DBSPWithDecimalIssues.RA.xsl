<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Хранимые процедуры без указания переменных с плавающей запятой" id="DatabaseProceduresWithDecimalIssues.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Хранимые процедуры без указания переменных с плавающей запятой" id="DatabaseProceduresWithDecimalIssues.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Всегда указывайте точность и шкалу для данных типов NUMERIC и DECIMAL.</h1>

				<p>Если Вы используете данные типов NUMERIC и DECIMAL, всегда указывайте точность и шкалу. В противном случае, точность по умолчанию
				будет установлена на 18, а шкала на 0. Когда шкала равна 0, невозможно хранить дробные числа. Если Вы не хотите хранить дробные числа,
				следует использовать другой тип данных, например bigint, int, smallint или tinyint.</p>

				<p><strong>Как это исправить:</strong>Используйте запрос выше, чтобы найти проблему в коде. Укажите точность и шкалу.
				Часто требуется подбирать правильную точность и шкалу по таблице.</p>

				<p><strong>Уровень сложности:</strong> Простой</p>

				<p><strong>Уровень опасности:</strong> Высокий</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/always-include-precision-and-scale-with" target="_blank">Always include precision and scale with decimal and numeric</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

