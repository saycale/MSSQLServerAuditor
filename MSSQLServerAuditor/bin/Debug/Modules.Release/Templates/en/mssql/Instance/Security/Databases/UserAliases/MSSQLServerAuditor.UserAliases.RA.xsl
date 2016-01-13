<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="User Aliased" id="DatabaseUserAliases.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="User Aliased" id="DatabaseUserAliases.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<body>
				<h1 class="firstHeading">Security issues when using aliased users in SQL Server</h1>

				<p>SQL Server has a lot of little features that are nice to use, but sometimes these things come back to get you.  One such issue is the use of aliased users.  This tip shows you how to find security holes when aliased users are setup in your databases and also that this feature will be deprecated in SQL Server 2008.</p>

				<p>Another interesting point to note is that, you cannot drop an aliased user using the sp_dropuser stored procedure, instead, use sp_dropalias.</p>

				<p><strong>How to correct it:</strong> If there are aliased users that have "dbo" level rights look at removing these and using roles instead.</p>

				<p><strong>Level of difficulty:</strong> Moderate</p>

				<p><strong>Level of severity:</strong> High</p>

				<a
				href="http://www.mssqltips.com/sqlservertip/1675/security-issues-when-using-aliased-users-in-sql-server/"
				target="_blank">Security issues when using aliased users in SQL Server</a>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
