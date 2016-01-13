<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Data on MS SQL server instance" id="AuditInstanceParameters.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Data on MS SQL server instance" id="AuditInstanceParameters.HTML.en" column="1" row="1" colspan="1" rowspan="1">
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
							Instance
						</th>
						<th>
							Query
						</th>
						<th>
							Hierarchy
						</th>
						<th>
							RecordSets
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
							Message
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
				<xsl:if test="MSSQLResult[@name='GetServerInfo' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Name
						</th>
						<th>
							Installed version
						</th>
						<th>
							The latest released version
						</th>
						<th>
							Type
						</th>
						<th>
							Account
						</th>
						<th>
							Cluster unit
						</th>
						<th>
							Server
						</th>
						<th>
							Tcp/Ip port
						</th>
						<th>
							Server collation
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetServerInfo' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
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
						<td align="right">
							<xsl:choose>
								<xsl:when test="IsLatestVersion != 'True'">
									<xsl:attribute name="style">font-weight: bold; color: red;</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="style">font-weight: bold; color: green;</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
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

	<mssqlauditorpreprocessors name="The Latest MS SQL server versions" id="LatestMSSQLServerVersions.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="The Latest MS SQL server versions" id="LatestMSSQLServerVersions.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h2>Summary table:</h2>

				<table border="1" cellpadding="4" cellspacing="0">
					<tr>
						<th bgcolor="#f0f0f0">&#160;</th>
						<th bgcolor="#f0f0f0">RTM (Gold, no SP)</th>
						<th bgcolor="#f0f0f0">SP1</th>
						<th bgcolor="#f0f0f0">SP2</th>
						<th bgcolor="#f0f0f0">SP3</th>
						<th bgcolor="#f0f0f0">SP4</th>
						<th bgcolor="#f0f0f0">Latest</th>
					</tr>
					<tr>
						<td><strong>SQL Server 2016</strong> (codename ?)</td>
						<td bgcolor="#FFCCCC">&#160;</td>
						<td>&#160;</td>
						<td>&#160;</td>
						<td>&#160;</td>
						<td>&#160;</td>
						<td bgcolor="#3CB371">13.00.300.44</td>
					</tr>
					<tr>
						<td><strong>SQL Server 2014</strong> (codename Hekaton)</td>
						<td bgcolor="#FFCCCC">12.00.2000.8</td>
						<td>12.00.4100.1</td>
						<td>&#160;</td>
						<td>&#160;</td>
						<td>&#160;</td>
						<td bgcolor="#3CB371">12.00.2402.0</td>
					</tr>
					<tr>
						<td><strong>SQL Server 2012</strong> (codename Denali)</td>
						<td bgcolor="#FFCCCC">11.00.2100.60</td>
						<td>11.00.3000</td>
						<td>11.00.5058</td>
						<td>&#160;</td>
						<td>&#160;</td>
						<td bgcolor="#3CB371">11.00.5623.0</td>
					</tr>
					<tr>
						<td><strong>SQL Server 2008 R2</strong> (codename Kilimanjaro)</td>
						<td bgcolor="#FFCCCC">10.50.1600.1</td>
						<td>10.50.2500</td>
						<td>10.50.4000</td>
						<td>10.50.6000.34</td>
						<td>&#160;</td>
						<td bgcolor="#3CB371">10.50.6525.0</td>
					</tr>
					<tr>
						<td><strong>SQL Server 2008</strong> (codename Katmai)</td>
						<td bgcolor="#FFCCCC">10.00.1600.22</td>
						<td>10.00.2531</td>
						<td>10.00.4000</td>
						<td>10.00.5500</td>
						<td>10.00.6000.29</td>
						<td bgcolor="#3CB371">10.00.6526.0</td>
					</tr>
					<tr>
						<td><strong>SQL Server 2005</strong> (codename Yukon)</td>
						<td bgcolor="#FFCCCC">9.00.1399.06</td>
						<td>9.00.2047</td>
						<td>9.00.3042</td>
						<td>9.00.4035</td>
						<td>9.00.5000</td>
						<td bgcolor="#3CB371">9.00.5324</td>
					</tr>
					<tr>
						<td><strong>SQL Server 2000</strong> (codename Shiloh)</td>
						<td bgcolor="#FFCCCC">8.00.194</td>
						<td>8.00.384</td>
						<td>8.00.532</td>
						<td>8.00.760</td>
						<td>8.00.2039</td>
						<td bgcolor="#3CB371">8.00.2305</td>
					</tr>
					<tr>
						<td><strong>SQL Server 7.0</strong> (codename Sphinx)</td>
						<td bgcolor="#FFCCCC">7.00.623</td>
						<td>7.00.699</td>
						<td>7.00.842</td>
						<td>7.00.961</td>
						<td>7.00.1063</td>
						<td bgcolor="#3CB371">7.00.1152</td>
					</tr>
				</table>

				<p><a href="http://sqlserverbuilds.blogspot.com.au/" target="_blank">What version of SQL Server do I have?</a></p>
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

