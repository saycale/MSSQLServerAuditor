<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Безымянные ограничения таблиц" id="UnnamedConstraints.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Безымянные ограничения таблиц" id="UnnamedConstraints.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Как присвоить имя ограничению по умолчанию и удалить безымянные ограничения в SQL Server</h1>

				<p>Лучше всего давать имена ограничениям, потому что это избавит Вас от множества проблем в дальнейшем. </p>

				<p><strong>Как это исправить:</strong> Дайте имена всем ограничениям.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Легкий</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/how-to-name-default-constraints-and-how-" target="_blank">Неименованные ограничители у таблиц</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
