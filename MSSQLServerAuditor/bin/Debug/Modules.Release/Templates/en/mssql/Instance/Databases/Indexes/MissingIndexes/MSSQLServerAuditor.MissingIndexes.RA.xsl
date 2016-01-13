<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Missing indexes" id="MissingIndexes.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Missing indexes" id="MissingIndexes.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Missing indexes</h1>

				<p>Performance Tuning is quite interesting and Index plays a vital role in
				it. A proper index can improve the performance and a bad index can hamper
				the performance.</p>

				<p>Please note, if you should not create all the missing indexes this script
				suggest. This is just for guidance. You should not create more than 5-10
				indexes per table. Additionally, this script sometime does not give accurate
				information so use your common sense.</p>

				<p>Any way, the scripts is good starting point. You should pay attention to
				<strong>Avg_Estimated_Impact</strong> when you are going to create index.
				The index creation script is also provided in the last column.</p>

				<p>The suggested indexes have an extremely narrow view - they only look at a
				single query, or a single operation within a single query. They don't take
				into account what already exists or your other query patterns.</p>

				<p>You still need a thinking human being to analyze the overall indexing
				strategy and make sure that you index structure is efficient and
				cohesive.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://technet.microsoft.com/en-us/library/ms345524.aspx" target="_blank">About the Missing Indexes Feature</a></p>

				<p><a href="http://blog.sqlauthority.com/2011/01/03/sql-server-2008-missing-index-script-download/" target="_blank">SQL SERVER – 2008 – Missing Index Script – Download</a></p>

				<p><a href="http://basitaalishan.com/2013/03/13/find-missing-indexes-using-sql-servers-index-related-dmvs/" target="_blank">Find missing indexes using SQL Servers index related DMVs</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

