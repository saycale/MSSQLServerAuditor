<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Data about MS SQL server" id="AuditInstanceParameters.HTML.en" columns="100" rows="100" splitter="yes">
	<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Data about MS SQL server" id="AuditInstanceParameters.HTML.en">
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
					<th>
						Instance
					</th>
					<th>
						Installed version
					</th>
					<th>
						The latest release
					</th>
					<th>
						Type
					</th>
					<th>
						Account
					</th>
					<th>
						IsCluster
					</th>
					<th>
						Server
					</th>
					<th>
						Tcp/Ip Port
					</th>
					<th>
						Server Collation
					</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="MSSQLResults/GetServerInfo">
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
					<td align="right">
						<xsl:if test="ProductVersion != LatestVersion">
							<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="ProductVersion != ''">
								<xsl:value-of select="ProductVersion"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td align="right">
						<xsl:choose>
							<xsl:when test="LatestVersion != ''">
								<xsl:value-of select="LatestVersion"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="Edition != ''">
								<xsl:value-of select="Edition"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="ServiceAccount != ''">
								<xsl:value-of select="ServiceAccount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="IsClustered = '1'">
								<xsl:text>Yes</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>No</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="ComputerNamePhysicalNetBIOS != ''">
								<xsl:value-of select="ComputerNamePhysicalNetBIOS"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="MachineName != ''">
										<xsl:value-of select="MachineName"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>&#160;</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td align="right">
						<xsl:choose>
							<xsl:when test="TcpPort != ''">
								<xsl:value-of select="TcpPort"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>&#160;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td>
						<xsl:choose>
							<xsl:when test="Collation != ''">
								<xsl:value-of select="Collation"/>
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

