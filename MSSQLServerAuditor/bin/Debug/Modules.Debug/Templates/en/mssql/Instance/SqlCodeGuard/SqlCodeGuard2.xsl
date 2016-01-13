<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Проблемы T-SQL кода" id="DatabaseObjectsSQLCodeGuard.HTML.en" columns="50;50" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Проблемы T-SQL кода" id="DatabaseObjectsSQLCodeGuard.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">
			<xsl:template match="/">
			<html>
				<head>
					<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shCore.js"></script>
					<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushTSql.js"></script>
					<script src="$JS_FOLDER$/SyntaxHighlighter/scripts/shBrushPlain.js"></script>

					<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shCore.css" rel="stylesheet" type="text/css" />
					<link href="$JS_FOLDER$/SyntaxHighlighter/styles/shThemeDefault.css" rel="stylesheet" type="text/css" />

					<script type="text/javascript">SyntaxHighlighter.defaults['toolbar'] = false;</script>
					<script type="text/javascript">SyntaxHighlighter.all()</script>
				</head>
				<body>
					<style>
						body { overflow: auto; padding:0; margin:0; }
					</style>
					<table border="0" width="100%" style="font-size : 12px; font-family : Courier New">
						<xsl:for-each select="MSSQLResults/GetDStoredProceduresCode-SQLCodeGuard[@RecordSet='2']">
							<tr>
								<td>
									<pre class="brush:tsql ruler: true; gutter: true; highlight: [{SCGErrorRows}]">
										<xsl:variable name="varSCGObject" select="SCGObject"/>
										<xsl:value-of select="../GetDStoredProceduresCode[ObjectName = $varSCGObject]/Procedure"/>
									</pre>
								</td>
							</tr>
						</xsl:for-each>
					</table>
				</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Список ошибок" id="DatabaseErrors.HTML.en" column="2" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:ms="urn:schemas-microsoft-com:xslt"
				xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<xsl:variable name="SQLErrorCode" select="MSSQLResults/MSSQLResult/@SqlErrorCode"/>

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script src="$JS_FOLDER$/json2.js"></script>
				<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
				<script src="$JS_FOLDER$/jquery.tablesorter.js"></script>
				<script src="$JS_FOLDER$/jquery.tablesorter.widgets.js"></script>

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
								},

								sortList: [[0,0],[1,0],[2,0]]
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
					<xsl:if test="@SqlErrorNumber != '0'">
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
					</xsl:if>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>

				<xsl:if test="$SQLErrorCode = ''">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Строка
						</th>
						<th>
							Колонка
						</th>
						<th>
							Код
						</th>
						<th>
							Сообщение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResults/GetDStoredProceduresCode-SQLCodeGuard[@RecordSet='1']">
					<tr>
						<td align="right">
							<xsl:choose>
								<xsl:when test="SCGRow != ''">
									<xsl:value-of select="SCGRow"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="SCGColumn != ''">
									<xsl:value-of select="SCGColumn"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="SCGErrorCode != ''">
									<xsl:value-of select="SCGErrorCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="SCGMessage != ''">
									<xsl:value-of select="SCGMessage"/>
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
