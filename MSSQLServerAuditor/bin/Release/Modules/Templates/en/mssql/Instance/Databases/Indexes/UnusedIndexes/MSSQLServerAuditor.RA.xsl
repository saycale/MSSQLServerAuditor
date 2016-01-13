<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Unused Indexes" id="UnusedIndexes.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Unused Indexes" id="UnusedIndexes.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Discovering Unused Indexes</h1>

				<h2 class="secondHeading">Overview</h2>

				<p>To ensure that data access can be as fast as possible, SQL Server like other
				relational database systems utilizes indexing to find data quickly. SQL Server has
				different types of indexes that can be created such as clustered indexes, non-clustered
				indexes, XML indexes and Full Text indexes.</p>

				<p>The benefit of having more indexes is that SQL Server can access the data quickly if
				an appropriate index exists. The downside to having too many indexes is that SQL Server
				has to maintain all of these indexes which can slow things down and indexes also require
				additional storage. So as you can see indexing can both help and hurt performance.</p>

				<p>In this section we will focus on how to identify indexes that exist, but are not
				being used and therefore can be dropped to improve performance and decrease storage
				requirements.</p>

				<h2 class="secondHeading">Explanation</h2>

				<p>When SQL Server 2005 was introduced it added Dynamic Management Views (DMVs) that
				allow you to get additional insight as to what is going on within SQL Server. One of
				these areas is the ability to see how indexes are being used. There are two DMVs that we
				will discuss. Note that these views store cumulative data, so when SQL Server is
				restated the counters go back to zero, so be aware of this when monitoring your index
				usage.</p>

				<h2 class="secondHeading">DMV - sys.dm_db_index_operational_stats</h2>

				<p>This DMV allows you to see insert, update and delete information for various aspects
				for an index. Basically this shows how much effort was used in maintaining the index
				based on data changes.</p>

				<p>If you query the table and return all columns, the output may be confusing. So the
				query below focuses on a few key columns. To learn more about the output for all columns
				you can check out Books Online.</p>

				<h2 class="secondHeading">DMV - sys.dm_db_index_usage_stats</h2>

				<p>This DMV shows you how many times the index was used for user queries. Again there
				are several other columns that are returned if you query all columns and you can refer
				to Books Online for more information.</p>

				<h2 class="secondHeading">Identifying Unused Indexes</h2>

				<p>So based on the output above you should focus on the output from the second query. If
				you see indexes where there are no seeks, scans or lookups, but there are updates this
				means that SQL Server has not used the index to satisfy a query but still needs to
				maintain the index. Remember that the data from these DMVs is reset when SQL Server is
				restarted, so make sure you have collected data for a long enough period of time to
				determine which indexes may be good candidates to be dropped.</p>

				<p><strong>Level of difficulty:</strong> Moderate</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://www.mssqltips.com/sqlservertutorial/256/discovering-unused-indexes/" target="_blank">Discovering Unused Indexes </a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

