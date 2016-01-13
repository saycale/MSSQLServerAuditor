<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Хранимые процедуры с SET ROWCOUNT" id="DatabaseProceduresWithSetRowCount.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Хранимые процедуры с SET ROWCOUNT" id="DatabaseProceduresWithSetRowCount.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Следующая версия SQL Server не будет поддерживать функцию SET ROWCOUNT</h1>

				<p>В новых версиях SQL Server изменения происходят очень быстро. Некоторые функции, считавшиеся стандартными решениями
				типичных сценариев TSQL, в следующих версиях будут объявлены устаревшими. Одной из таких функций является параметр SET ROWCOUNT.
				Данный параметр также будет объявлен устаревшим в следующих версиях SQL Server. Единственным способом ограничить результаты
				будет использование ключевого слова TOP.</p>

				<p>Некоторые другие функции SET тоже не будут поддерживаться, например</p>

				SET ANSI_NULLS<br />
				SET ANSI_PADDING<br />
				SET CONCAT_NULL_YIELDS_NULL<br />

				<p>Я советую Вам ознакомиться с полным списком устаревших функций базы данных и не слишком на них опираться, если Вы хотите,
				чтобы Ваша база данных была совместима с новыми версиями SQL Server. </p>

				<p><strong>Уровень сложности:</strong> Низкий</p>

				<p><strong>Уровень опасности:</strong> Низкий</p>

				<p><a href="http://blog.namwarrizvi.com/?p=81" target="_blank">SET ROWCOUNT will not be supported in future version of SQL Server</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
