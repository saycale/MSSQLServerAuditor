<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Database Users" id="DatabaseUsers.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Database Users" id="DatabaseUsers.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Database Users</h1>

				<p>Attaching and restoring databases from one server instance to another are common
				tasks executed by a DBA. After attaching or restoring of a database, previously created
				and configured logins in that database do not provide access. The most common symptoms
				of this problem are that the application may face login failed errors or you may get a
				message like the user already exists in the current database when you try to add the
				login to the database. This is a common scenario when performing an attach or a restore,
				so how do you resolve this?</p>

				<p><strong>How to correct it:</strong> The stored procedure sp_change_users_login can be
				used.</p>

				<p><strong>Level of difficulty:</strong> High</p>

				<p><strong>Level of severity:</strong> High</p>

				<a
				href="http://www.mssqltips.com/sqlservertip/1590/understanding-and-dealing-with-orphaned-users-in-a-sql-server-database/"
				target="_blank">
				Understanding and dealing with orphaned users in a SQL Server database</a>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

