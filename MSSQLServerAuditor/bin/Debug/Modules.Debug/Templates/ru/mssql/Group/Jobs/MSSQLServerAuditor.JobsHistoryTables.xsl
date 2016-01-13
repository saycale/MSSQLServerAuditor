<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="История выполнения плановых заданий" id="JobsExecutionHistory.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="JobsExecutionHistory.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
				<xsl:stylesheet version="1.0"
						xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
						xmlns:ms="urn:schemas-microsoft-com:xslt"
						xmlns:dt="urn:schemas-microsoft-com:datatypes">

				<xsl:template match="/MSSQLResults">

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
					<xsl:if test="MSSQLResult[@SqlErrorNumber!='0']/child::node()">
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
						<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0']">
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

				<xsl:if test="$SQLErrorCode = ''">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Имя задания
						</th>
						<th>
							Включен
						</th>
						<th>
							Дата запуска
						</th>
						<th>
							Время выполнения
						</th>
						<th>
							Статус
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResults/GetJobsExecutionHistory">
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
								<xsl:when test="JobName != ''">
									<xsl:value-of select="JobName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobEnabled != ''">
									<xsl:value-of select="JobEnabled"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobRunDateTime != ''">
									<xsl:value-of select="ms:format-date(JobRunDateTime, 'dd.MM.yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(JobRunDateTime, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="JobDurationText != ''">
									<xsl:value-of select="JobDurationText"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="JobRunStatus != ''">
									<xsl:value-of select="JobRunStatus"/>
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
