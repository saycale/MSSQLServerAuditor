<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Active processes (statistics for 15 sec)" id="UpMonitor.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Active processes (statistics for 15 sec)" id="UpMonitor.HTML.en" column="1" row="1" colspan="1" rowspan="1">
		<xsl:stylesheet version="1.0"
			  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
			  xmlns:ms="urn:schemas-microsoft-com:xslt"
			  xmlns:dt="urn:schemas-microsoft-com:datatypes">

		<xsl:template match="/">

		<xsl:variable name="SQLErrorCode" select="MSSQLResults/MSSQLResult/@SqlErrorCode"/>

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
			<table id="myTable">
			<thead>
				<tr>
					<th>
						User
					</th>
					<th>
						#
					</th>
					<th>
						Blocked
					</th>
					<th>
						CPU (%)
					</th>
					<th>
						IO (%)
					</th>
					<th>
						Number
					</th>
					<th>
						Commands
					</th>
					<th>
						Command name
					</th>
					<th>
						Program
					</th>
					<th>
						Computer
					</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="MSSQLResults/UpMonitor">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="nt_username != ''">
									<xsl:value-of select="nt_username"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="spid != ''">
									<xsl:value-of select="spid"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="blocked != ''">
									<xsl:value-of select="blocked"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="cpu != ''">
									<xsl:value-of select="format-number(cpu, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="IO != ''">
									<xsl:value-of select="format-number(IO, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="Snapshots != ''">
									<xsl:value-of select="Snapshots"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="cmd_history != ''">
									<xsl:value-of select="cmd_history"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="last_batch != ''">
									<xsl:value-of select="ms:format-date(last_batch, 'dd/MM/yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(last_batch, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="program_name != ''">
									<xsl:value-of select="program_name"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="hostname != ''">
									<xsl:value-of select="hostname"/>
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
		</body>
		</html>
		</xsl:template>
		</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

