<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Перекрывающиеся индексы" id="OverlappingIndexes.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Перекрывающиеся индексы" id="OverlappingIndexes.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Поиск перекрывающиеся (лишние) индексы</h1>

				<p>В некоторых приложениях из-за несогласованности работы программистов
				возникают ситуации, когда создаются индексы в которых нет необходимости.
				Т.е. запросы могут использовать уже существующие индексы. Из-за этого
				возникают дополнительные издержки на их обслуживание. Таким образом, если
				поля индекса перекрываются более широким индексом в том же порядке
				следования полей начиная с первого поля, то этот индекс считается лишним,
				так как запросы могут использовать более широкий индекс. Данный сценарий как
				раз ищет эти перекрывающиеся (лишние) индексы.</p>

				<p><strong>Как это исправить:</strong> Удалите неиспользуемые индексы.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Незначительный</p>

				<p><a href="http://www.sql.ru/blogs/andraptor/1218" target="_blank">Поиск перекрывающихся (лишних) индексов в SQL Server 2005+</a></p>

				<p><a href="http://www.confio.com/logicalread/duplicate-indexes-and-sql-server-performance/" target="_blank">Duplicate Indexes and SQL Server Performance</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

