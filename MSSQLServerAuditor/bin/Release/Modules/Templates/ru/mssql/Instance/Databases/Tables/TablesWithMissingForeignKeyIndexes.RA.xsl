<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с пропущенными индексами для ключей" id="TablesWithMissingForeignKeyIndexes.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с пропущенными индексами для ключей" id="TablesWithMissingForeignKeyIndexes.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Отсутствие индексов у внешних ключей</h1>

				<p>Один из способов повысить производительность в SQL Server – убедиться, что все внешние ключи имеют индексы.
				Это НЕ происходит автоматически, что отчасти удивительно, учитывая то, насколько это повысит производительность базы
				установленного оборудования SQL Server.</p>

				<p><strong>Как это исправить:</strong>Проверьте все таблицы и создайте индексы</p>

				<p><a href="http://hazaa.com.au/blog/sql-server-generating-sql-for-missing-foreign-key-indexes/" target="_blank">Missing Foreign Key Indexes</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
