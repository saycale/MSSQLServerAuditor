<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Finding Forwarded Records" id="IndexesWithForwardedRecords.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Finding Forwarded Records" id="IndexesWithForwardedRecords.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Finding Forwarded Records SQL Server 2008</h1>

				<p>HEAP tables and forwarded records are a major and overlooked performance problem.</p>

				<p>Imagine a customer using an ISV application that stores certain product information
				in a varchar(200) column in a SQL Server database table. When the system was first being
				used nobody ever entered a product description with more than 10 characters. However,
				over time new products were added to the portfolio which required the introduction of a
				prefix in the description. On the SQL level this was done by running an update which
				added the appropriate prefix. Additionally, the company merged with another one and the
				product description entries changed again and became on average 35 characters long.
				Again an update statement was used to implement the changes. Of course since the column
				was defined as a varchar(200) there was no problem storing the changed values.</p>

				<p>Unfortunately, the customer found a significant, unexplainable slowdown of the
				system. Why did this happen? </p>

				<ul> <li>The updates hadn't changed any of the indexes on the table. </li> <li>The
				product table was used very heavily but an investigation of the user queries proved that
				the query plans were correct and hadn't changed over time. </li> <li>There were no
				hardware issues and the number of concurrent users hadn't increased. </li> <li>The table
				size difference due to new records shouldn't have had a huge impact because the queries
				always selected a limited number of rows using an index. </li></ul>

				<p>So what was left as a potential root cause? A closer look at several performance
				counters indicated that there were an unusually high number of logical reads on the
				product table.</p>

				<p><strong>And the reason for this:</strong> Due to the new product descriptions, the
				row size increased. Updates of rows led to <strong>forward pointers</strong> because the
				rows didn't fit into their old page slots any more. This phenomena leaves the data row
				pointers in the indexes unchanged but adds forward pointers in the data table ( heap ).
				If the new row gets too big SQL Server will move it to a new page slot and leave a
				pointer at its original spot. Therefore, looking for a row will be more expensive
				afterwards. A lookup of the data row in a heap is no longer a direct access using the
				page and slot address. Instead of getting the data row the server might have to follow a
				forward pointer first.</p>

				<p>With SQL Server 2000 I used the dbcc showcontig command to prove the theory. But you
				have to use the option "with tableresults" to get the info about "ForwardedRecords". In
				the default dbcc showcontig output you won't be albe to recognize the issue. </p>

				<p>SQL Server 2005 on the other hand offers a DMV ( sys.dm_db_index_physical_stats() )
				which shows information regarding "ForwardedRecords" ( column "forwarded_record_count"
				in the result set ). In both cases the number of rows in the output can be a little bit
				misleading as it's the sum of the "real" number of rows and the number of the
				"Forwarded Records". A select count(*) on the table still returns the "real" number
				which you expect.</p>

				<p><strong>How to correct it:</strong> Once a data table (heap) includes forward
				pointers there is only one way to get rid of them : table reorg. There are a few options
				to do this: The simplest one would be to create a clustered index on the data table and
				drop it again. But there is another option to avoid forward pointers entirely; by
				creating a clustered index from the beginning. A clustered index keeps the data rows in
				its leaf node level. Therefore the data is always sorted according to the index keys and
				forward pointers won't be used. It's like a continuous online reorg in this regard. The
				SQL script below shows the difference between using a data table ( heap ) and a
				clustered index as well as the difference between char() and varchar().</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://wiki.lessthandot.com/index.php/Finding_Forwarded_Records_SQL_Server_2008" target="_blank">Finding Forwarded Records SQL Server 2008</a></p>

				<p><a href="http://blogs.msdn.com/b/mssqlisv/archive/2006/12/01/knowing-about-forwarded-records-can-help-diagnose-hard-to-find-performance-issues.aspx8" target="_blank">Knowing about forwarded records</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
