<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Список хранимых процедур с sp_" id="DatabaseProceduresWithSPName.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Список хранимых процедур с sp_" id="DatabaseProceduresWithSPName.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Не начинайте процедуры с SP_</h1>

				<p>При выполнении хранимой процедуры, SQL Server сначала проверяет, является ли она встроенной (системной).
				Он проверяет главную базу данных на наличие данной процедуры. Если процедура не найдена, SQL Server ищет в базе данных пользователя.
				В рабочей среде с большим количеством запущенных процессов, это может замедлить время обработки данных.</p>

				<p>Представьте, что будет, если Microsoft решит загрузить хранимую системную процедуру с именем
				созданной Вами процедуры. Ваша процедура неожиданно прекратит работу и вместо нее будет выполняться процедура Microsoft.
				Чтобы понять, о чем я, создайте в базе данных хранимую процедуру с именем sp_help. Когда Вы попробуете ее запустить,
				SQL запустит одноименную процедуру из главной базы данных.</p>

				<p>Чтобы исправить эту проблему, найдите все аналогичные процедуры и переименуйте их. Но могут возникнуть небольшие сложности.
				Некоторые хранимые процедуры вызываются другими хранимыми процедурами. В таких случаях, их тоже нужно переименовать.
				Следует также изменить интерфейсный код для вызова процедуры с новым именем.</p>

				<p><strong>Уровень сложности:</strong> Средне-высокий. Уровень усилий для исправления данной проблемы – от среднего до высокого,
				в зависимости от того, сколько процедур необходимо переименовать</p>

				<p>Один из возможных вариантов решения проблемы – переименовать процедуру, затем создать новую процедуру с прежним именем.
				Данная процедура удет вести файл журнала и вызывать оригинальную процедуру. Этот способ сделает Вашу программу снова работоспособной
				(хоть она и будет работать немного медленнее из-за журнальных операций). После этого Вы можете определить, какое именно приложение
				запустило процедуру и переименовать запрос.</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/MSSQLServer/don-t-start-your-procedures-with-sp_" target="_blank">Don't start your procedures with SP_</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
