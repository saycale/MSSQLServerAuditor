<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Orphan users" id="OrphanUseres.HTML.ru" columns="100" rows="100" splitter="yes">
	<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Orphan users" id="OrphanUseres.HTML.ru">
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
						Database
					</th>
					<th>
						User
					</th>
					<th>
						Creation date
					</th>
					<th>
						Last update date
					</th>
				</tr>
			</thead>
			<tbody>
				<xsl:for-each select="MSSQLResults/GetOrphanUsers">
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
						<td>
							<xsl:choose>
								<xsl:when test="UserName != ''">
									<xsl:value-of select="UserName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="CreateDate != ''">
									<xsl:value-of select="ms:format-date(CreateDate, 'dd.MM.yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(CreateDate, 'HH:mm:ss')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="CreateDate != ''">
									<xsl:value-of select="ms:format-date(UpdateDate, 'dd.MM.yyyy')"/>
									<xsl:text>&#160;</xsl:text>
									<xsl:value-of select="ms:format-time(UpdateDate, 'HH:mm:ss')"/>
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

	<mssqlauditorpreprocessors name="Orphan users (description)" id="OrphanUseres.Description.HTML.ru" columns="100" rows="100" splitter="yes">
	<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Orphan users (description)" id="OrphanUseres.Description.HTML.ru">
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

			<h1 class="firstHeading">Fix Orphaned Database Users</h1>

			<p> Most common when performing restores or re-attaching databases, database users become
			orphaned. This simply means the database user is not assocaited with a sql server login.
			To fix these situations you can run the following in the database context that the
			database user exists in.</p>

		</body>
		</html>
		</xsl:template>
		</xsl:stylesheet>
	</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

