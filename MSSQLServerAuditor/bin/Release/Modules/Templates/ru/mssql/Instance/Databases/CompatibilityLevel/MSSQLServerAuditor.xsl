<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Полный список доступных баз данных экземпляра (информация)" id="DatabaseInfo.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Полный список доступных баз данных экземпляра (информация)" id="DatabaseInfo.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults">

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script src="$JS_FOLDER$/json-js/json2.js"></script>
				<script src="$JS_FOLDER$/jquery-1.11.1.js"></script>
				<script src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"></script>
				<script src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"></script>

				<script type="text/javascript">
					$(document).ready(function()
						{
							$("#myErrorTable").tablesorter({
								theme : 'MSSQLServerAuditorError',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});

							$("#myTable").tablesorter({
								theme : 'MSSQLServerAuditor',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});
						}
					);
				</script>
			</head>
			<body>
				<style>
					body { overflow: auto; padding:0; margin:0; }
				</style>
				<xsl:if test="MSSQLResult[@SqlErrorNumber!='0' or @SqlErrorCode!='']/child::node()">
				<table id="myErrorTable">
				<thead>
					<tr>
						<th>
							Зкземпляр
						</th>
						<th>
							Запрос
						</th>
						<th>
							Категория
						</th>
						<th>
							Наборов
						</th>
						<th>
							#
						</th>
						<th>
							Код
						</th>
						<th>
							Номер
						</th>
						<th>
							Сообщение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0' or @SqlErrorCode!='']">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="@instance != ''">
									<xsl:value-of select="@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="@name != ''">
									<xsl:value-of select="@name"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="@hierarchy != ''">
									<xsl:value-of select="@hierarchy"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@RecordSets != ''">
									<xsl:value-of select="@RecordSets"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@RowCount != ''">
									<xsl:value-of select="@RowCount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@SqlErrorCode != ''">
									<xsl:value-of select="@SqlErrorCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@SqlErrorNumber != ''">
									<xsl:value-of select="@SqlErrorNumber"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="SqlErrorMessage != ''">
									<xsl:value-of select="SqlErrorMessage"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>
				<xsl:if test="MSSQLResult[@name='DatabaseInfo' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							База данных
						</th>
						<th>
							Имя базы источника
						</th>
						<th>
							Версия
						</th>
						<th>
							Владелец
						</th>
						<th>
							Дата создания
						</th>
						<th>
							Параметр сортировки
						</th>
						<th>
							Статус
						</th>
						<th>
							Тип восстановления
						</th>
						<th>
							Тип доступа
						</th>
						<th>
							Обновление лога
						</th>
						<th>
							Автозакрытие
						</th>
						<th>
							Автоматическое сжатие
						</th>
						<th>
							Создание статистики
						</th>
						<th>
							Обновление статистики
						</th>
						<th>
							Репликатор
						</th>
						<th>
							Подписчик
						</th>
						<th>
							Используется шифрование
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='DatabaseInfo' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="../../@instance != ''">
									<xsl:value-of select="../../@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseName != ''">
									<xsl:value-of select="DatabaseName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseSourceName != ''">
									<xsl:value-of select="DatabaseSourceName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:if test="DatabaseCompatibilityLevel != MasterCompatibilityLevel">
								<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="DatabaseCompatibilityLevel != ''">
									<xsl:value-of select="DatabaseCompatibilityLevel"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:if test="DatabaseOwner != 'sa'">
								<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="DatabaseOwner != ''">
									<xsl:value-of select="DatabaseOwner"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseCreationDate != ''">
									<xsl:value-of select="ms:format-date(DatabaseCreationDate, 'dd.MM.yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(DatabaseCreationDate, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:if test="DatabaseCollation != MasterCollation">
								<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="DatabaseCollation != ''">
									<xsl:value-of select="DatabaseCollation"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseStatus != ''">
									<xsl:value-of select="DatabaseStatus"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseRecovery != ''">
									<xsl:value-of select="DatabaseRecovery"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseUpdateability != ''">
									<xsl:value-of select="DatabaseUpdateability"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsInStandBy != ''">
									<xsl:value-of select="DatabaseIsInStandBy"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:if test="DatabaseIsAutoClose != '0'">
								<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="DatabaseIsAutoClose != ''">
									<xsl:value-of select="DatabaseIsAutoClose"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsAutoShrink != ''">
									<xsl:value-of select="DatabaseIsAutoShrink"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsAutoCreateStatistics != ''">
									<xsl:value-of select="DatabaseIsAutoCreateStatistics"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsAutoUpdateStatistics != ''">
									<xsl:value-of select="DatabaseIsAutoUpdateStatistics"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsPublished != ''">
									<xsl:value-of select="DatabaseIsPublished"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsSubscribed != ''">
									<xsl:value-of select="DatabaseIsSubscribed"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseIsEncrypted != ''">
									<xsl:value-of select="DatabaseIsEncrypted"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Описание" id="DatabaseInfo.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Описание" id="DatabaseInfo.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Настройки параметров баз данных</h1>

				<h2 class="secondHeading">Настройки SQL Server и автозакрытия</h2>

				<p>Если Вы недавно создали базу данных, а параметр автозакрытия установлен как True,
				измените его на False. Это наилучший вариант, он ничему не повредит и избавит Вам от
				проблем вроде описанной ниже. Давайте проверим, получится ли у меня перезапустить то
				субботнее утро.</p>

				<p><strong>Как это исправить:</strong>Установите параметр автозакрытия false.</p>

				<p><strong>Уровень сложности:</strong>Легкий</p>

				<p><strong>Уровень опасности:</strong>Легкий</p>

				<p><a href="http://www.sql-server-performance.com/tips/optimizing_indexes_general_p1.aspx" target="_blank">Настройки SQL Server и автозакрытия</a></p>

				<h2 class="secondHeading">Конфликты collation с временными таблицами и табличными переменными</h2>

				<p>Если collation Вашей базы данных не совпадает с collation временной базы данных,
				может возникнуть конфликт. Временные таблицы и табличные переменные создаются во
				временной базе данных. Если Вы не задаете collation строковых колонок в переменных Вашей
				таблицы и временных таблицах, они унаследуют collation временной базы данных по
				умолчанию. При сравнении и/или добавлении временной таблицы или табличной переменной,
				может возникнуть конфликт разных collation.</p>

				<p>Обычно лучше всего, чтобы все элементы имели одинаковую collation, включая временную
				бау данных, модели (используется для создания новой базы данных), Вашу базу данных и все
				строковые колонки (varchar, nvarchar, char, nchar, text, ntext).</p>

				<p>Существует несколько способов решения этой проблемы. Самое лучшее решение –
				изменить collation базы данных по умолчанию (это повлияет на строковые колонки), затем
				изменить collation существующих колонок. Другой способ – изменить код, создающий
				временную таблицу или табличные переменные,чтобы он задавал collation строковых колонок.
				Вы можете прописать collation самостоятельно или использовать collation базы данных по
				умолчанию.</p>

				<p><strong>Уровень сложности:</strong> Средний</p>

				<p><strong>Уровень опасности:</strong> Высокий. Это скрытая ошибка, которую сложно
				найти. Она может произойти в любой момент.</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/collation-conflicts-with-temp-tables-and" target="_blank">Конфликты collation</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Описание2" id="DatabaseInfo.Description2.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Описание2" id="DatabaseInfo.Description2.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Old Compatibility Level</h1>

				<p>Each SQL Server database has a setting called compatibility level that
				kinda-sorta influences how T-SQL commands are interpreted. There's a lot of
				misconceptions around what this does - some people think setting a database
				at an old compatibility level will let an out-of-date application work just
				fine with a brand spankin' new SQL Server. That's not usually the case,
				though.</p>

				<p>This report checks the compatibility_level field for each database in
				sys.databases and compares it to the compatibility_level of the master
				database.</p>

				<p><strong>How to correct it:</strong> Check out the Books Online
				explanation of how to change compatibility levels of a database.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://www.brentozar.com/blitz/old-compatibility-level/" target="_blank">Old Compatibility Level</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

