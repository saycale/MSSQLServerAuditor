<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Хранимые процедуры с вызовом недокументированных функций" id="DatabaseProceduresWithundocumented.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Хранимые процедуры с вызовом недокументированных функций" id="DatabaseProceduresWithundocumented.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Поиск процедур, вызывающих недокументированные процедуры в SQL Server</h1>

				<p>При использовании хранимыхнедокументированных процедур, Вы рискуете не обновитьбазу данных до новой версии. Или еще хуже…
				У Вас может быть нарушена функциональность, а Вы даже узнаете об этом. В случае с хранимыми недокументированными процедурами,
				Microsoft может не объявить заранее, если онит решат сделать их устаревшими, поэтому Вы не узнаете, что функциональность
				нарушена до тех пор, пока не станет слишком поздно.</p>

				<p>Ниже представлен запрограммированный список недокументированных хранимых процедур. В соответствии с их типом,
				для недокументированных процедур сложно найти документацию. Поэтому, скорее всего, в данном списке представлены не все процедуры. </p>

				<p><strong>Как это исправить:</strong> Перепишите функциональность, чтобы она не опиралась на недокументированные процедуры.</p>

				<p><strong>Уровень сложности:</strong> От среднего до высокого. Хранимые недокументированные процедуры часто используются ввиду легкости,
				Но их заменить не так просто. </p>

				<p><strong>Уровень опасности:</strong> От средней до высокой</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DataDesign/identify-procedures-that-call-sql-server" target="_blank">Identify procedures that call SQL Server undocumented procedures</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
