<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Пропущенные индексы" id="MissingIndexes.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Пропущенные индексы" id="MissingIndexes.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Пропущенные индексы</h1>

				<p>Данный отчёт укажет рекомендации по добавлению индексов. Кроме параметров
				индекса также представлен DDL скрипт для создания индексов. Отчёт
				основывается на динамических представлениях.</p>

				<p>Отчёт не более чем рекомендации, поэтому будьте внимательны и
				руководствуйтесь здравым смыслом при создании индексов. Кроме ускорения
				выполнения операций по выборке данных наличие большого количества индексов
				на таблице может привести к проблемам с производительностью при вставки или
				обновлении данных. Кроме того, каждый индекс требует ресурсов сервера по
				поддержанию индекса в актуальном состоянии, поэтому на реальной системе
				важен баланс по количеству индексов.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Незначительный</p>

				<p><a href="http://technet.microsoft.com/en-us/library/ms345524.aspx" target="_blank">About the Missing Indexes Feature</a></p>

				<p><a href="http://blog.sqlauthority.com/2011/01/03/sql-server-2008-missing-index-script-download/" target="_blank">SQL SERVER – 2008 – Missing Index Script – Download</a></p>

				<p><a href="http://basitaalishan.com/2013/03/13/find-missing-indexes-using-sql-servers-index-related-dmvs/" target="_blank">Find missing indexes using SQL Servers index related DMVs</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

