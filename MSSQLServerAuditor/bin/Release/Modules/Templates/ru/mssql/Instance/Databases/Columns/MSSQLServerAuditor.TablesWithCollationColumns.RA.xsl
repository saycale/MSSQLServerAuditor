<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с колонками у которых collation другой" id="TablesWithCollationColumns.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с колонками у которых collation другой" id="TablesWithCollationColumns.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Конфликты collation в SQL Server</h1>

				<p>Collation контролирует сортировку и сравнение строк. Сортировка, как правило,не
				является проблемой, так как не вызывает конфликтов между разными collation. Она может
				быть выполнена не совсем так, как Вы планировали, но не вызовет ошибок. Проблема
				возникнет при сравнении данных. Существует несколько видов сравнений. Например, простое
				сравнение, где выражение или сравнение имеет условие объединения. Если в базе данных
				есть колонки, имеющие collation отличную от collation базы данных по умолчанию, проблема
				не заставит себя долго ждать.</p>

				<p>Если Вы НЕ задаете collation при добавлении новой колонки к существующей таблице со
				строковыми колонками, будет использована collation базы данных по умолчанию. Если после
				этого Вы создадите запрос для присоединения к существующим колонкам (с другой
				collation), может произойти ошибка конфликта collation.</p>

				<p>Хочу пояснить, я НЕ предлагаю, чтобы collation каждой строковой колонки совпадала с
				collation по умолчанию базы данных. Я советую, чтобы в случае, если она отличается от
				collation по умолчанию, у этого была веская причина. Есть немало баз данных, где
				разработчики не задумывались о collation. В данной ситуации лучше, чтобы collation
				каждой строковая колонки совпадала с collation по умолчанию базы данных.</p>

				<p><strong>Как это исправить:</strong> Чтобы исправить эту проблему, измените collation
				существующих строковых колонок.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Высокий</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/sql-server-collation-conflicts" target="_blank">Конфликты различных типов Collation</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

