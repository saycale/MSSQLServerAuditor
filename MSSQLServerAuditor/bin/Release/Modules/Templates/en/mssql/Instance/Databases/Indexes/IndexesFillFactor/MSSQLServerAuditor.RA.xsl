<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Indexes Fill Factor Summary" id="IndexesFillFactorSummary.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Indexes Fill Factor Summary" id="IndexesFillFactorSummary.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Specify Fill Factor for an Index</h1>

				<p>The fill-factor option is provided for fine-tuning index data storage and
				performance. When an index is created or rebuilt, the fill-factor value determines the
				percentage of space on each leaf-level page to be filled with data, reserving the
				remainder on each page as free space for future growth. For example, specifying a
				fill-factor value of 80 means that 20 percent of each leaf-level page will be left
				empty, providing space for index expansion as data is added to the underlying table. The
				empty space is reserved between the index rows rather than at the end of the index. The
				fill-factor value is a percentage from 1 to 100, and the server-wide default is 0 which
				means that the leaf-level pages are filled to capacity.</p>

				<p><a href="http://technet.microsoft.com/en-us/library/ms177459.aspx" target="_blank">Specify Fill Factor for an Index</a></p>

				<p><a href="http://blog.sqlauthority.com/2009/12/16/sql-server-fillfactor-index-and-in-depth-look-at-effect-on-performance/" target="_blank">SQL SERVER – Fillfactor, Index and In-depth Look at Effect on Performance</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

