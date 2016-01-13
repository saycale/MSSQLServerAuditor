<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Stored procedures calling to undocumented procedures" id="DatabaseProceduresWithUndocumented.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Stored procedures calling to undocumented procedures" id="DatabaseProceduresWithUndocumented.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Identify procedures that call SQL Server undocumented
				procedures</h1>

				<p>When you use an undocumented stored procedure, you run the risk of not being able to
				upgrade your database to a new version. What's worse... you could have broken
				functionality and not even know it. With undocumented stored procedures, Microsoft may
				not document when they decide to deprecate it, so you may not know about your broken
				functionality until it's too late.</p>

				<p>Presented below is a hard coded list of undocumented stored procedures. By their very
				nature, it is hard to find documentation on undocumented procedures. Therefore, the
				procedures in the list below is likely to be incomplete.</p>

				<p><strong>How to correct it:</strong> Rewrite your functionality so that is does not
				rely upon undocumented procedures.</p>

				<p><strong>Level of difficulty:</strong> moderate to high. Undocumented stored
				procedures are often used because it's easy. Replacing it is usually NOT easy.</p>

				<p><strong>Level of severity:</strong> Moderate to high</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DataDesign/identify-procedures-that-call-sql-server" target="_blank">Identify procedures that call SQL Server undocumented procedures</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

