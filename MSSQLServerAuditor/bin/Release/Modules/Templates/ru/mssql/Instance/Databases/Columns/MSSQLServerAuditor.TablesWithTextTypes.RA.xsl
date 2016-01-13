<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с колонками типа text/ntext" id="TablesWithTextColumns.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с колонками типа text/ntext" id="TablesWithTextColumns.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Не используйте текстовый тип данных для SQL 2005 и выше</h1>

				<p>В версиях SQL Server ниже SQL2005 единственным способом хранения большого объема данных были следующие типы данных: text, ntext и image.
				В SQL2005 представлены новые типы данных взамен выше перечисленных, а также поддерживаются все полезные функции работы со строками.
				Изменить тип данных в эквивалентный новым версиям SQL2005+ просто и быстро (в зависимости от размера таблицы). Итак, к чему ждать?
				Измените тип данных прямо сейчас.</p>

				<p><strong>Как исправить:</strong> Измените тип данных на версию SQL2005+. Тип данных text будет изменен на varchar(max), ntext на nvarchar(max),
				image на varbinary(max).</p>

				<p><strong>Уровень сложности:</strong> Простой</p>

				<p><strong>Уровень опасности:</strong> Низкий</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/don-t-use-text-datatype-for-sql-2005-and" target="_blank">Не используйте текстовый тип данных для SQL 2005 и выше</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

