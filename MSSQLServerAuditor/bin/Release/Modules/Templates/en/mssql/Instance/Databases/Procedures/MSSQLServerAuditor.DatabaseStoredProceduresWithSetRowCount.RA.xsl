<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Procedures With Set RowCount" id="DatabaseProceduresWithSetRowCount.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Procedures With Set RowCount" id="DatabaseProceduresWithSetRowCount.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">SET ROWCOUNT will not be supported in future version of SQL Server</h1>

				<p>Things are changing very rapidly in SQL Server future versions. Some of the features
				which were once treated as standard solutions to some typical scenarios of TSQL are now
				scheduled to be deprecated in future versions of SQL Server. One of these feature was
				the SET ROWCOUNT statement. This statement is also scheduled for being deprecated in
				future versions of SQL Server. The only way to limit your results will be by using the
				TOP keyword.</p>

				<p>Similarly some other SET options will no longer be supported like</p>

				SET ANSI_NULLS<br />
				SET ANSI_PADDING<br />
				SET CONCAT_NULL_YIELDS_NULL<br />

				<p>I will suggest you to study the complete list of deprecated database features here
				and make sure you are not relying too much on these features, if you want to make sure
				that your database is compatible with future versions of SQL Server. </p>

				<p><strong>How to correct it:</strong> Rewrite your functionality without using 'TOP'.</p>

				<p><strong>Level of difficulty:</strong> Low</p>

				<p><strong>Level of severity:</strong> Low</p>

				<p><a href="http://blog.namwarrizvi.com/?p=81" target="_blank">SET ROWCOUNT will not be supported in future version of SQL Server</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

