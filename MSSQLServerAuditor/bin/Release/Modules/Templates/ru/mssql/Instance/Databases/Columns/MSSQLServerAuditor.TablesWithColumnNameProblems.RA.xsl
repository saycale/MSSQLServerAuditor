<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с проблемами в именах колонок" id="TablesWithColumnNameProblems.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с проблемами в именах колонок" id="TablesWithColumnNameProblems.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Не используйте пробелы и другие недопустимые символы в именах колонок</h1>

				<p>Имена колонок и таблиц не должны содержать пробелы и другие недопустимые символы. Это
				считается плохой практикой, потому что вынуждает использовать вокруг имен квадратные
				скобки. Из-за квадратных скобок код сложнее прочесть и понять. Запрос, приведенный ниже,
				выделит колонки и таблицы, содержащие в загловке число. В большинстве случаев, если в
				имени колонки есть число, она представляет собой ненормированную базу данных. В этом
				правиле есть исключения, поэтому не все случаи нужно исправлять.</p>

				<p>Известно, что в некоторых компаниях разрешено (и даже рекомендовано) использовать
				символ черты снизу. Вы можете внести список допустимых символов в новый измененный коде
				ниже. Он позволяет использовать символ черты снизу и символ <strong>$</strong>. Вы
				можете попросить разработчиков программы изменить эту местную переменную и включить в
				нее символ, допустимый в Вашей организации.</p>

				<p><strong>Как это исправить:</strong> Если проблема вызвана количеством, возможно,
				следует изменить структуру базы данных, чтобы она включала больше таблиц. К примеру, у
				вас есть таблица StudentGrade (ОценкаУченика) с StudentId, Grade1, Grade2, Grade3,
				Grade4 (Ученик1, Оценка1, Оценка2, Оценка3, Оценка4). Следует изменить ее на
				StudentGrade (ОценкаУченика) с StudentId, Grade, Identifier (IDУченика, Оценка,
				идентификатор). У каждого учащегося в таблице будет несколько записей (один для каждой
				оценки). Вам нужно будет добавить колонку идентификации для указания, за что поставлена
				данная оценка (за контрольную 10-го ноября, доклад по книге и т.д.)</p>

				<p><strong>Уровень сложности:</strong> Средний</p>

				<p><strong>Уровень опасности:</strong> Незначительный</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/do-not-use-spaces-or-other-invalid-chara" target="_blank">Не используйте пробелы и другие недопустимые символы в именах колонок</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

