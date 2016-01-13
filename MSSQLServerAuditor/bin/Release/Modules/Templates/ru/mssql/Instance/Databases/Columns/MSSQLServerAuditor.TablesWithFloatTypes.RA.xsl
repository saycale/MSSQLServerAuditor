<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Таблицы с колонками типа float" id="TablesWithFloatTypes.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Таблицы с колонками типа float" id="TablesWithFloatTypes.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Не используйте тип данных float</h1>

				<p>Возможно, это звучит жестко и не всегда верно. Тем не менее, в большинстве случаев
				следует избегать тип данных float. К сожалению, типы данных float и real являются
				приблизительными и могут привести к значительным ошибкам округления.</p>

				<h1 class="firstHeading">Запрос на тип данных float может дать несовместимый
				результат</h1>

				<p>Данные с плавающей запятой приблизительны. Поэтому не все значения в пределах данного
				типа данных могут быть точно отображены. При добавлении маленького и очень большого
				числа, маленькое число может быть потеряно в конечном результате.</p>

				<p>Будучи конечными пользователями, мы хотим получить непротиворечивый и правильный
				результат. А тип данных float не дает правильный результат, так как его значение не
				является точным. Результат запроса данных float дает несовместимый результат. На примере
				выше, мы меняем порядок операторов + и – и получаем другой результат. Предположим, что
				мы хотим суммировать значения типа float, например sum(floatcolumn). SQL Server может
				выполнить параллельное сканирование всей таблицы другим потоком и сложить полученные
				результаты. В этом случае значения имеют произвольный порядок и конечный результат тоже
				будет произвольным. В случае одного из наших клиентов, он выполнял один и тот же запрос
				несколько раз и каждый раз получал новый результат. </p>

				<p>Поэтому мы советуем избегать тип данных float, особенно, если нужно сложить значения
				типа float. Перед сложением измените тип данных на numeric. Например, X имеет тип данных
				float.</p>

				<p>Вы можете использовать функцию sum(cast(x - десятичная дробь(30,4))) для получения
				непротиворечивого результата.</p>

				<p><strong>Как это исправить:</strong> Проверьте используемые данные и оцените
				необходимые точность и диапазон. Измените тип данных (или код) для получения десятичной
				дроби с нужной точностью и диапазоном. </p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/do-not-use-the-float-data-type" target="_blank">Не используйте тип данных float</a></p>

				<p><a href="http://blogs.msdn.com/b/qingsongyao/archive/2009/11/14/float-datatype-is-evil.aspx" target="_blank">Float datatype is evil</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

