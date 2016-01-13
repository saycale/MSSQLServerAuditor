<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Данные о конфигурации MS SQL сервера" id="AuditInstanceConfiguration.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Данные о конфигурации MS SQL сервера" id="AuditInstanceConfiguration.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
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
				<xsl:if test="MSSQLResult[@name='GetInstanceConfiguration' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Название
						</th>
						<th>
							Параметр
						</th>
						<th>
							Описание параметра
						</th>
						<th>
							Текущее значение
						</th>
						<th>
							Рекомендуемое значение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetInstanceConfiguration' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
					<tr>
						<xsl:if test="ConfigurationValueRecommended != '' and ConfigurationValueInUse != ConfigurationValueRecommended">
							<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
						</xsl:if>
						<xsl:if test="ConfigurationValueNotRecommended != '' and ConfigurationValueInUse = ConfigurationValueNotRecommended">
							<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
						</xsl:if>
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
								<xsl:when test="ConfigurationOptionName != ''">
									<xsl:value-of select="ConfigurationOptionName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="ConfigurationOptionDescription != ''">
									<xsl:value-of select="ConfigurationOptionDescription"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="ConfigurationValueInUse != ''">
									<xsl:value-of select="ConfigurationValueInUse"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="ConfigurationValueRecommended != ''">
									<xsl:value-of select="ConfigurationValueRecommended"/>
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

	<mssqlauditorpreprocessors name="Описание" id="AuditInstanceConfiguration.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Описание" id="AuditInstanceConfiguration.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Информационные проверки</h1>

				<p>Проверки на данной странице выполняются лишь в информационных целях.
				Проверки покажут, включена ли данная опция.</p>

				<p><strong>"CLR включен":</strong> Некоторые приложения не позволят
				запустить CLR, поскольку он может нести угрозу безопасности. По умолчанию
				CLR отключен. У Вас должна быть веская причина чтобы разрешить выполение
				внешних утилит CLR.</p>

				<p><strong>"Резервное сжатие данных по умолчанию":</strong> Эта проверка
				покажет, включено ли резервное сжатие данных по умолчанию. Если оно
				включено, резервные данные будут сохранятся, если Вы зададите команду
				регулярного резервного копирования базы данных. Для заданного резервного
				копирования, Вы можете указать команды WITH NO_COMPRESSION (без сжатия) и
				WITH COMPRESSION (с применением компрессии) в поле BACKUP (резервное копирование). Так Вы
				можете создавать резервные копии без сжатия данных. Впервые резервное сжатие
				данных было представлено в SQL Server 2008 Enterprise. Начиная с SQL Server
				2008 R2, резервное сжатие данных поддерживается SQL Server 2008 R2 Standard
				и всеми более поздними версиями. Каждая версия SQL Server 2008 и последующие
				может восстановить сжатые резервные данные.</p>

				<p><strong>"Database Mail XPs":</strong> Эта провека покажет, используется
				ли отправка по электронной почте через использование нового протокола или
				используется устаревший метод. Всесто использования функции
				<strong>xp_sendmail</strong> рекомендуется использование
				<strong>sp_send_dbmail</strong>. Подробности в <a
				href="http://msdn.microsoft.com/en-us/library/ms175887.aspx"
				target="_blank">БОЛ</a>.</p>

				<p><strong>Как это исправить:</strong> Проверьте параметры настройки и, если
				некоторых из них имеют отличные от рекомендуемых значения, и Вы не совсем
				понимаете, почему, установите рекомендованные значения.</p>

				<p><strong>Уровень сложности:</strong> Легкий</p>

				<p><strong>Уровень опасности:</strong> Средний</p>

				<p><a href="http://wiki.lessthandot.com/index.php/SQLCop_informational_checks" target="_blank">Server Informational Checks</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

