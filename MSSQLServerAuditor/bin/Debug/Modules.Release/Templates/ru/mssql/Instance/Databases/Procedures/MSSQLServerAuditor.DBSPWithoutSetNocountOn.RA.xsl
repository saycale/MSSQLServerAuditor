<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Хранимые процедуры без SET NOCOUNT ON" id="DatabaseProceduresWithoutSetNocountOn.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Хранимые процедуры без SET NOCOUNT ON" id="DatabaseProceduresWithoutSetNocountOn.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Поиск процедур без SET NOCOUNT ON</h1>

				<p>Если Вы выполняете много обновлений, вставок или удалений в хранимой процедуре,
				следует установить NOCOUNT ON. SQL server может стать очень "болтливым", если этот режим
				выключен. Это будет лишь засорять канал и расходовать системные ресурсы. Если процедура
				вызывается 30 раз в секунду, и Вы выполняете 3 DML операции, пострадает 90 рядов
				сообщений.</p>

				<p>Команда "set nocount on" должна быть первой в процедуре после параметров и AS.</p>

				<p><strong>Как исправить:</strong> Добавьте 'SET NOCOUNT ON' в начале хранимой процедуры.</p>

				<p><strong>Уровень сложности:</strong> Низкий</p>

				<p><strong>Уровень опасности:</strong> Низкий</p>

				<p><a href="http://www.mssqltips.com/sqlservertip/1226/set-nocount-on-improves-sql-server-stored-procedure-performance/" target="_blank">Параметр SET NOCOUNT ON улучшает производительность хранимых процедр SQL Server</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

