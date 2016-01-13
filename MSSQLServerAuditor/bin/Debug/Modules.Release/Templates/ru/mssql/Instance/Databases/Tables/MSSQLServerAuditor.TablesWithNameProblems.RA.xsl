<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с проблемными именами" id="TablesWithNameProblems.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с проблемными именами" id="TablesWithNameProblems.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Не используйте пробелы или другие недопустимые символы в именах таблиц</h1>

				<p>Имена колонок (и таблиц) не должны содержать пробелы и другие недопустимые символы. Это считается плохой практикой,
				так как часто вынуждает использовать вокруг имен квадратные скобки. Из-за квадратный скобок код труднее прочесть и понять.
				Приведенный ниже запрос выделит колонки и таблицы с числами в имени. Чаще всего, если в имени колонки есть цифра, она
				представляет собой неупорядоченную базу данных. В этом правиле есть исключения, поэтому не все элементы с данной проблемой
				нужно исправлять. </p>

				<p><strong>Как это исправить:</strong> Если проблема в количестве, возможно, требуется изменить структуры базы данных так,
				чтобы она вмещала больше таблиц. Например, если у Вас есть таблица StudentGrade (Оценка учащегося) с StudentId,
				Grade1, Grade2, Grade3, Grade4 (Idученика, Оценка1, Оценка2, Оценка3). Переименуйте ее в StudentGrade (Оценка учащегося)
				с StudentId, Grade, Identifier (Id ученика, Оценка, Идентификатор). У каждого ученика в таблице будет несколько рядов
				(один для каждой оценки). Нужно будет добавить колонку с идентификатором, которая показывала бы, за что поставлена данная оценка
				(контрольную 10го ноября, доклад по книге и т.д.) Если возникнет путаница со знаками, следует переименовать колонку одним словом
				или фразой без пробелов, цифр и символов. Затем проверьте все связанные с ней элементы. Это могут быть процедуры, функции,
				виды, индексы, интерфейсный код и т.д.</p>

				<p><strong>Уровень сложности:</strong> Средний</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/do-not-use-spaces-or-other-invalid-chara" target="_blank">Таблицы с проблемными именами</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
