<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с tbl префиксами" id="TablesWithPrefix.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с tbl префиксами" id="TablesWithPrefix.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Не начинайте имя таблицы с «tbl»</h1>

				<p>Эта проблема связана с именами. Нельзя использовать приставку «tbl» в таблицах, потому что это не добавляет ясности коду
				(самодокументирует). В действительности такая приставка дает обратный эффект, так как на чтение кода требуется больше
				времени и пользователю приходится расшифровывать аббревиатуру.</p>

				<p><strong>Как это исправить:</strong> Переименуйте таблицу, убрав приставку. Это не так просто, как кажется, потому что
				таблица может быть связана с другими элементами, такими как виды, хранимые процедуры, заданные пользователем функции, скрипты
				для создания индекса, встроенный SQL (в интерфейсных приложениях) и т.д.</p>

				<p><strong>Уровень сложности:</strong> От среднего до высокого</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/MSSQLServer/don-t-prefix-your-table-names-with-tbl" target="_blank">Не начинайте имя таблицы с «tbl»</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

