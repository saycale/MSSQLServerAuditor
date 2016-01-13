<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Full list of instance databases (info)" id="DatabaseInfo.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Full list of instance databases (info)" id="DatabaseInfo.HTML.en" column="1" row="1" colspan="1" rowspan="1">
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
				<xsl:if test="MSSQLResult[@name='DatabaseInfo' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Instance
						</th>
						<th>
							DatabaseName
						</th>
						<th>
							DatabaseSource
						</th>
						<th>
							Version
						</th>
						<th>
							Owner
						</th>
						<th>
							CreationDate
						</th>
						<th>
							Collation
						</th>
						<th>
							Status
						</th>
						<th>
							DatabaseRecovery
						</th>
						<th>
							DatabaseUpdateability
						</th>
						<th>
							StandbyMode
						</th>
						<th>
							IsAutoClose
						</th>
						<th>
							IsAutoShrink
						</th>
						<th>
							IsAutoCreateStatistics
						</th>
						<th>
							IsAutoUpdateStatistics
						</th>
						<th>
							IsPublished
						</th>
						<th>
							IsSubscribed
						</th>
						<th>
							IsEncrypted
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
									<xsl:value-of select="ms:format-date(DatabaseCreationDate, 'dd/MM/yyyy')"/>
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

	<mssqlauditorpreprocessors name="Description" id="DatabaseInfo.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Description" id="DatabaseInfo.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">SQL Server and the Auto Close Setting</h1>

				<p>If you have a database that was recently created and you find "Auto
				Close" set to True, change it to False. It is a best practice, won't hurt
				anything and will prevent things like this situation from coming up. Now
				lets see if I can restart this Saturday morning.</p>

				<p><strong>How to correct it:</strong> Set the database option "Auto Close"
				to false.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://www.sql-server-performance.com/tips/optimizing_indexes_general_p1.aspx" target="_blank">General Tips on Optimizing SQL Server Indexes</a></p>

				<h1 class="firstHeading">Collation conflicts with temp tables and table variables</h1>

				<p>When the collation of your user database does not match the collation of
				TempDB, you have a potential problem. Temp tables and table variables are
				created in TempDB. When you do not specify the collation for string columns
				in your table variables and temp tables, they will inherit the default
				collation for TempDB. Whenever you compare and/or join to the temp table or
				table variable, you may get a collation conflict.</p>

				<p>Under normal circumstances, it is best if all your collations match. This
				includes TempDB, Model (used for creating a new database), your user
				database, and all your string columns (VARCHAR, NVARCHAR, CHAR, NCHAR, TEXT,
				NTEXT).</p>

				<p><strong>How to correct it:</strong> There are several ways to correct
				this problem. The long term solution is to change the default collation for
				your database (affecting new string columns) and then change the collation
				for your existing columns. Alternatively, you could modify any code that
				creates a temp table or table variable so that it specifies a collation on
				your string columns. You can hard code the collation or use the default
				database collation.</p>

				<p><strong>Level of difficulty:</strong> Moderate</p>

				<p><strong>Level of severity:</strong> High. This is a hidden, hard to find
				bug, just waiting to happen.</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/collation-conflicts-with-temp-tables-and" target="_blank">Collation conflicts with temp tables and table variables</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Description2" id="DatabaseInfo.Description2.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Description2" id="DatabaseInfo.Description2.HTML.en" column="1" row="1" colspan="1" rowspan="1">
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

