<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Нагрузка дисковой системы" id="DatabaseIOStatus.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Нагрузка дисковой системы" id="DatabaseIOStatus.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Показатели нагрузки дисковой системы</h1>

				Статистика (число чтений и записей на каждый из файлов, составляющих базы данных)
				основана на данных из виртуальной таблицы <strong>dm_io_virtual_file_stats</strong>.
				Статистика собирается с момента последнего рестарта экземпляра сервера баз данных.

				<p><a href="http://technet.microsoft.com/ru-ru/library/ms190326.aspx" target="_blank">sys.dm_io_virtual_file_stats (Transact-SQL)</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

