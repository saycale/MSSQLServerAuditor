<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Хранимые процедуры без указания размера текстовых переменных" id="DatabaseProceduresWithCharIssues.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Хранимые процедуры без указания размера текстовых переменных" id="DatabaseProceduresWithCharIssues.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Всегда указывайте размер при помощи varchar, nvarchar, char или nchar</h1>

				<p>В SQL Server несколько строковых типов данных. Это varchar, nvarchar, char и nchar. Многие интерфейсные
				языки не требуют указания длины строковых переменных и SQL Server не исключение. Если Вы не укажете длину
				строковых объектов в SQL Server, он применит значения по умолчанию. Наиболее часто длина по умолчанию
				составляет 1 символ. Они применяется для столбцов, параметров и локальных переменных. Исключение - функции CAST и CONVERT,
				длина которых по умолчанию – 30 символов. </p>

				<p><strong>Как это исправить:</strong> Чтобы исправить эту проблему, найдите ее местоположение (используя код SQL выше).
				Затем укажите размер для каждого случая.  Если проблема появилась во время объявления столбца в таблице и Вы ХОТИТЕ, чтобы
				его размер был равен 1 символу, укажите (1). В других местах нужно будет определить необходимый размер. Иногда для этого
				требуется найти размер в таблице.</p>

				<p><strong>Уровень сложности:</strong> Простой</p>

				<p><strong>Уровень опасности:</strong> Высокий, так как данная проблема может привести к повреждению данных</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/MSSQLServer/always-include-size-when-using-varchar-n" target="_blank">Always include size when using varchar, nvarchar, char and nchar</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

