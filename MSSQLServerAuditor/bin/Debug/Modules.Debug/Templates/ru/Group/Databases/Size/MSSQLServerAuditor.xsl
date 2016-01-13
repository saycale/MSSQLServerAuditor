<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Database distribution by size (Mb)" id="DatabasesSize.HTML.ru" columns="100" rows="100" splitter="yes">
	<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Database distribution by size (Mb)" id="DatabasesSize.HTML.ru">
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
						Instance
					</th>
					<th>
						Query
					</th>
					<th>
						Hierarchy
					</th>
					<th>
						#
					</th>
					<th>
						Code
					</th>
					<th>
						Number
					</th>
					<th>
						ErrorMessage
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
					<th rowspan="2">
						Instance
					</th>
					<th rowspan="2">
						Name
					</th>
					<th rowspan="2">
						Total
					</th>
					<th colspan="2">
						Data
					</th>
					<th colspan="2">
						Transaction log
					</th>
				</tr>
				<tr>
					<th>
						Reserved
					</th>
					<th>
						In usage
					</th>
					<th>
						Reserved
					</th>
					<th>
						In usage
					</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="MSSQLResults/GetDatabaseSize">
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
								<xsl:when test="DatabaseName != ''">
									<xsl:value-of select="DatabaseName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseDataSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseDataSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseDataSizeUsedMB != ''">
									<xsl:value-of select="format-number(DatabaseDataSizeUsedMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseLogSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseLogSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseLogSizeUsedMB != ''">
									<xsl:value-of select="format-number(DatabaseLogSizeUsedMB, '###,###,##0.00')"/>
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

