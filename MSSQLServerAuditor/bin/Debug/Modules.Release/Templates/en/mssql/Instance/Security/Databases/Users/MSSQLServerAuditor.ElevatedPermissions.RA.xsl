<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="User with Elevated Database Permissions" id="DatabaseUsersMembersOfGroupsWithElevatedPermissions.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="User with Elevated Database Permissions" id="DatabaseUsersMembersOfGroupsWithElevatedPermissions.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">User with Elevated Database Permissions</h1>

				Inside a database, users can be set up in the roles
				<strong>db_owner</strong>, <strong>db_accessadmin</strong>,
				<strong>db_securityadmin</strong>, and <strong>db_ddladmin</strong>. The
				Books Online page on database-level roles explains the permissions for these
				roles, all of which involve more than just reading and writing data. Some of
				these can get you fired if somebody even just clicks around too much in SSMS
				– it's way too easy to rename an object accidentally.

				<p><strong>How to correct it:</strong> Talk to the users listed and find out
				if they really need to change database objects or security. If not, remove
				them from the roles. In SQL Server Management Studio, you can go into
				Security, Logins, and right-click on a login. Go to User Mapping and you can
				see their roles for each database. Unchecking them from the roles for each
				database will take them out immediately – but just make sure they've got
				SOME kind of access so they can still do their queries. We're a big fan of
				<strong>db_datareader</strong> and <strong>db_datawriter</strong> – those
				two roles give people rights to read and write to any table in the database.
				(It's not nearly as good as doing fine-grained permissions for just the
				tables they need, but it's way better than making them
				<strong>db_owner</strong>.)</p>

				<p><strong>Level of difficulty:</strong> High</p>

				<p><strong>Level of severity:</strong> High</p>

				<p><a href="http://msdn.microsoft.com/en-us/library/ms189121.aspx" target="_blank">Database-Level Roles</a></p>

				<p><a href="http://BrentOzar.com/go/elevated" target="_blank">User with Elevated Database Permissions</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

