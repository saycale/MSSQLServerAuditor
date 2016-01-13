<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Заполнение некластерных индексов" id="IndexesFillFactorSummary.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Заполнение некластерных индексов" id="IndexesFillFactorSummary.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Укажите фактор заполнения для индекса</h1>

				<p>Опция "фактор заполнения" создана для точной настройки хранилища индекса
				с данными и его производительности. Когда индекс создан или перестроен,
				значение коэффициента заполнения определяет процент пространства на каждой
				странице нижнего уровня, которое будет заполнено данными, оставляя на каждой
				странице напоминание о свободном пространстве для расширения в будущем.
				Например, значение фактора заполнения 80 означает, что 20 процентов каждой
				страницы конечного уровня останется пустой, обеспечивая пространство для
				расширения индекса по мере добавления данных в базовую таблицу. Пустое
				пространство резервируется между индексами строк, а не в конце индекса.
				Коэффициент заполнения – это значение в процентах от 1 до 100. Значение
				сервера пол умолчанию - 0 (ноль), что означает, что страницы конечного
				уровня полностью заполнены.</p>

				<p><a href="http://technet.microsoft.com/en-us/library/ms177459.aspx" target="_blank">Specify Fill Factor for an Index</a></p>

				<p><a href="http://blog.sqlauthority.com/2009/12/16/sql-server-fillfactor-index-and-in-depth-look-at-effect-on-performance/" target="_blank">SQL SERVER – коэфициент заполнения, индекс и взгляд изнутри на производительность</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

