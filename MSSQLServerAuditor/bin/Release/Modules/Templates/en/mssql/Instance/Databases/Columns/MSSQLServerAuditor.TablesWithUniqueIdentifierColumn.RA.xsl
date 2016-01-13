<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables with NewID keys" id="TablesWithUniqueIdentifierColumn.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables with NewID keys" id="TablesWithUniqueIdentifierColumn.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<xsl:variable name="SQLErrorCode" select="MSSQLResults/MSSQLResult/@SqlErrorCode"/>

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Best Practice: Do not cluster on UniqueIdentifier when you use
				NewId</h1>

				<p>There can only be one clustered index per table because SQL Server stores the data in
				the table in the order of the clustered index . When you use a UniqueIdentifier as the
				first column in a clustered index, every time you insert a row in the table, it is
				almost guaranteed to be inserted in to the middle of the table. SQL server stores data
				in 8K pages. If a page is full, SQL Server will do a page split, which causes another 8k
				page to be allocated and half the data from the previous page to be moved to the new
				page. Individually, each page split is fast but does take a little bit of time. In a
				high transaction environment, there could be many page splits happening frequently,
				which ultimately result in slower performance.</p>

				<p>When you use an Identity column for a clustered index, the next value inserted is
				guaranteed to be higher than the previous value. This means that new rows will always be
				added to the end of the table and you will not get unnecessary page splits for table
				fragmentation.</p>

				<p>SQL Server 2005 introduced a new function called NewSequentialId(). This function can
				only be used as a default for a column of type UniqueIdentifier. The benefit of
				NewSequentialId is that it always generates a value greater than any other value already
				in the table. This causes the new row to be inserted at the end of the table and
				therefore no page splits.</p>

				<p><strong>How to correct it:</strong> There are several ways to prevent this problem.
				The best method is to use NewSequentialId() instead of NewId. Alternatively (if you are
				using SQL 2000), you could set the fill factor for the index to be less than 100%. Fill
				factor identifies how "full" the data pages are when you recreate the index. With a 100%
				fill factor there is no room in the index to accommodate new rows. If you need to use a
				UniqueIdentifier, and it must be clustered and you cannot use NewSequentialId, then you
				should modify the Fill Factor to minimize page splits. If you do this, it's important to
				rebuild the index periodically.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/best-practice-don-t-not-cluster-on-uniqu" target="_blank">Best Practice: Do not cluster on UniqueIdentifier when you use NewId</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

