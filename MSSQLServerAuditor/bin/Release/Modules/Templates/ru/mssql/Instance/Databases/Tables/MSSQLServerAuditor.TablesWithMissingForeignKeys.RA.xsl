<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Пропущенные ключи" id="TablesWithMissingForeignKeys.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Пропущенные ключи" id="TablesWithMissingForeignKeys.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Отсутствие внешних ключей</h1>

				<p>Ссылки являются сердцем базы данных. Можно создать красивую базу данных с идеально работающим
				интерфейсным кодом, который всегда, в 100% случаев, делает с данными именно то, что нужно. Но писать код сложно.
				Очень сложно. Часто ваши данные – самое ценное имущество. И нужно защитить его каждым битом имеющихся технологий.
				В сердце защиты Ваших данных лежит целостность на уровне ссылок. Что это значит? Это значит, что Вы никогда не должны
				терять свои данные!</p>

				<p>Приведенный ниже код найдет колонки с ID в имени, где колонка не является частью первичного ключа или ограничением внешнего ключа.
				В большинстве случаев он находит отсутствующий ограничитель и отображает потенциальную проблему. Вам нужно будет определить, существует ли
				данная проблема на самом деле и действовать соответственно.</p>

				<p><strong>Как это исправить:</strong> На первый взгляд кажется, что исправить проблему просто. Нужно лишь задать внешние ключи, верно?
				На самом деле это не так просто. В этот момент может выполняться код, удаляющий все необходимые данные из связанных с ним таблиц.
				Если у Вас есть код, удаляющий данные в родственных таблицах в неправильном порядке, возникнут ошибки ограничителей, связанные
				со ссылками. Аналогичнвя проблема может возникнуть с обновлениями и вставками. Если у Вас есть ограничения по ссылкам, важен порядок
				выполнения операций.</p>

				<p><strong>Уровень сложности:</strong> Высокий</p>

				<p><strong>Уровень опасности:</strong> Высокий</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DataDesign/missing-foreign-key-constraints" target="_blank">Отсутствие внешних ключей</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
