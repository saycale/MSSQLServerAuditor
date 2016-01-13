<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Динамические счётчики" id="InstanceDynamicParameters.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Динамические счётчики" id="InstanceDynamicParameters.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Динамические счётчики</h1>

				Статистика основана на данных из виртуальной таблицы
				<strong>dm_os_performance_counters</strong>, которая собирается с момента последнего
				рестарта экземпляра сервера баз данных.

				<p><a href="http://technet.microsoft.com/en-us/library/ms187743.aspx" target="_blank">sys.dm_os_performance_counters (Transact-SQL)</a></p>

				<p><a href="http://olontsev.ru/2012/10/deprecated-features-usage-detection/" target="_blank">Как определить, используются ли устаревшие функции</a></p>

				<p><a href="http://habrahabr.ru/post/70121/" target="_blank">Используем SQL Server Dynamic Management Views and Functions</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

