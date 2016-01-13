<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Широкие таблицы" id="WideTables.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Широкие таблицы" id="WideTables.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Что такое широкие таблицы?</h1>

				<p>Проверка широких таблиц покажет, превышает ли сумма максимальных значений в столбцах 8,060 байт.</p>

				<p>В SQL Server 2008 ограничение в 8,060 байт на табличный ряд "ослаблено" для таблиц с заданными
				пользователем колонками, имеющие данные следующих типов: varchar, nvarchar, varbinary, sql_variant или CLR.
				Длина каждой колонки не должна превышать 8000 байт, однако их общая ширина может быть больше 8060 байт.
				Вот на что советует обратить внимание Books On Line:</p>

				<p>Превышение ограничения ряда в 8060 байт может сказаться на производительности, поскольку в SQL Server
				действует ограничение 8 КБ на страницу. Если сочетание заданных пользователем колонок с типами VARCHAR,
				NVARCHAR, VARBINARY, sql_variant или CLR превышает это значение, ядро системы управления базой данных SQL Server
				перемещает самую широкую колонку на другую страницу в единичный блок ROW_OVERFLOW_DATA, оставляя на оригинальной
				странице 24байтовый указатель.</p>

				<p>Перемещение больших записей на другую страницу происходит динамически, так как длина записей увеличивается из-за операций обновления.
				Если операции обновления уменьшают длину записи, они могут вызвать возвращение записей на оригинальную страницу в единичный блок IN_ROW_DATA.
				Запросы и выполнение других операций типа SELECT, сортировка или объединение больших записей с данными, превышающими
				максимальный размер ряда, также замедляет время обработки, поскольку данные записи обрабатываются одновременно, а не последовательно.</p>

				<p><strong>Как это исправить:</strong> Не следует забывать о "подводных камнях" использования широких таблиц и, если это возможно,
				следует перенести такие колонки в их собственные таблицы. Разработчики могут воспользоваться SELECT *, если им нужно убрать 2 колонки из таблицы.</p>

				<p><strong>Уровень сложности:</strong> Средний</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://wiki.lessthandot.com/index.php/SQLCop_wide_table_check" target="_blank">Широкие таблицы</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

