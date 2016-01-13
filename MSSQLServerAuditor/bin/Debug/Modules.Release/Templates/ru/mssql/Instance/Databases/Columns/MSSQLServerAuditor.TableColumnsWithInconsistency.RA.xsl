<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с проблемами в типах колонок" id="TableColumnsWithInconsistency.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с проблемами в типах колонок" id="TableColumnsWithInconsistency.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Проверка типов колонок таблиц</h1>

				<p>В течение разработки схемы базы данных из-за различных факторов может
				случится, что одинаковые по сути и смыслу колонки (имеющие одинаковые
				названия) получат различные типы данных. Данный отчёт и призван сообщить об
				этом. Данный отчёт не более чем рекомендация к размышлению о структуре базы
				данных и решение об изменении типов любых колонок должно приниматься
				архитектором базы данных в каждом конкретном случае.</p>

				<p><strong>Как это исправить:</strong> Подумайте об изменении типов колонок
				таблиц, возможно в базе даннах есть скрытая ошибка или неточность.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Небольшой</p>

				<p><a href="http://sqlapproach.blogspot.com.au/2013/02/columns-inconsintency-check.html" target="_blank">Columns Inconsintency Check</a></p>
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

