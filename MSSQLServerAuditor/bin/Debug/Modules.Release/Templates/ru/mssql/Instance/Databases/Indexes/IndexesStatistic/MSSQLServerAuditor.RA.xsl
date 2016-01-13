<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Статистика для индексов" id="IndexesStatisticStatus.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Статистика для индексов" id="IndexesStatisticStatus.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">SQL Server: Важность статистики индекса</h1>

				<p>Статистика играет очень важную роль в производительности. Точная
				статистика данных, хранящихся в таблицах, используется для вычисления
				наилучшей стратегии выполнения запросов SQL. Если статистика неправильно
				отражает текущее содержимое таблицы, запрос будет плохо выполнен. Как
				узнать, верна ли статистика, и что сделать, если автоматическое обновление
				статистики не подходит для способа использования таблицы?</p>

				<p>SQL сервер использует статистику для выбора соответствующего плана
				запроса, так что, если у Вас проблемы с производительностью во время
				запросов, стоит ее исправить. Устаревшая статистика может заставить SQL
				сервер выбрать неправильный план. Выполните следующий запрос для нахождения
				устаревшей статистики. Он использует функцию STATS_DATE()</p>

				<p>Если Вы обнаружили, что статистика устарела, воспользуйтесь фукцией
				"UPDATE STATISTICS()" для ее обновления. Для этой цели Вы даже можете
				написать курсор.</p>

				<p><a href="http://www.databasejournal.com/features/mssql/sql-server-how-important-are-index-statistics.html" target="_blank">SQL Server: How Important Are Index Statistics</a></p>

				<p><a href="http://sequelserver.blogspot.com.au/search/label/find%20outdated%20Statistics" target="_blank">How to find outdated statistics in sql.</a></p>

				<p><a href="http://www.mssqltips.com/sqlservertip/2734/what-are-the-sql-server-wasys-statistics/" target="_blank">Что такое статистика SQL Server _WA_Sys... ?</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

