<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы без первичного ключа" id="TablesWithoutAPrimaryKey.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы без первичного ключа" id="TablesWithoutAPrimaryKey.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Лучше всего, чтобы у каждой таблицы был первичный ключ</h1>

				<p>Согласно своему определению, первичные ключи должны содержать уникальные данные. Они выполняются в базе данных через уникальный индекс.
				Если в таблице нет кластерного индекса, будет кластеризован индекс первичного ключа. Это не всегда так, но в большинстве случаев мы хотим,
				чтобы первичные ключи были клатеризованы, потому что они, как правило, являются ключевым критерием в запросах к данным (включая условия
				объединения и критерий "where"). Кластерные индексы значительно повышают быстродействие системы, так как позволяют SQL Server создавать
				оптимальные планы выполнения.</p>

				<p><strong>Как это исправить:</strong> Найдите таблицы без первичного ключа, используя запрос выше. Проанализируйте каждую таблицу и
				определите, что именно делает каждый ряд уникальным. Измените таблицу, добавив в нее первичный ключ.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/best-practice-every-table-should-have-a" target="_blank">Таблицы без первичного ключа</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
