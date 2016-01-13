<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Пустые таблицы" id="EmptyTables.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Пустые таблицы" id="EmptyTables.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Список пустых таблиц на каждой из баз данных</h1>

				<p><strong>Как исправить проблему:</strong> Внимательно просмотрите список пустых
				таблиц. Может быть таблица осталась после рефакторинга и больше не используется, может
				была мысль её использовать, но до реализации так и не дошло. Может быть масса причин,
				почему таблицы пустые. Может быть они и не нужны совсем? </p>

				<p><strong>Уровень сложности:</strong> Простой</p>

				<p><strong>Уровень опасности:</strong> Простой</p>

				<p><a href="http://wiki.lessthandot.com/index.php/List_all_empty_tables_in_your_SQL_Server_database" target="_blank">List all empty tables</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
