<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Статистика индексов" id="IndexesStatisticStatus.HTML.en" columns="100" rows="100" splitter="yes">
	<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Статистика индексов" id="IndexesStatisticStatus.HTML.en">
		<xsl:stylesheet version="1.0"
			  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
			  xmlns:ms="urn:schemas-microsoft-com:xslt"
			  xmlns:dt="urn:schemas-microsoft-com:datatypes">

		<xsl:template match="/">

		<xsl:variable name="SQLErrorCode" select="MSSQLResults/MSSQLResult/@SqlErrorCode"/>

		<html>
		<head>
			<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

			<script type="text/javascript" src="$JS_FOLDER$/json-js/json2.js"/>
			<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
			<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"/>
			<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"/>

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

			<xsl:if test="$SQLErrorCode != ''">
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
				<xsl:for-each select="MSSQLResults/MSSQLResult">
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
								<xsl:variable name="MyError" select="1" />
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

			<xsl:if test="$SQLErrorCode = ''">
			<table id="myTable">
			<thead>
				<tr>
					<th>
						Экземпляр
					</th>
					<th>
						Тип
					</th>
					<th>
						Схема
					</th>
					<th>
						Таблица
					</th>
					<th>
						Индекс
					</th>
					<th>
						Обновление статистики
					</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="MSSQLResults/GetIndexesStatisticStatus">
				<tr>
					<td>
						<xsl:choose>
							<xsl:when test="Instance != ''">
								<xsl:value-of select="Instance"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="ObjectType != ''">
								<xsl:value-of select="ObjectType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="SchemaName != ''">
								<xsl:value-of select="SchemaName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="TableName != ''">
								<xsl:value-of select="TableName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="IndexName != ''">
								<xsl:value-of select="IndexName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td align="right">
						<xsl:choose>
							<xsl:when test="IndexStatsLastUpdated != ''">
								<xsl:value-of select="ms:format-date(IndexStatsLastUpdated, 'dd/MM/yyyy')"/>
								<xsl:text>&#160;</xsl:text>
								<xsl:value-of select="ms:format-time(IndexStatsLastUpdated, 'HH:mm:ss')"/>
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
</root>

