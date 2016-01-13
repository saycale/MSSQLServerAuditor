<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Indexes for Rebuild" id="RebuildIndexes.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Indexes for Rebuild" id="RebuildIndexes.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Finding Fragmentation Of An Index And Fixing It</h1>

				<p>A lof of time your index will get framented over time if you do a lot of updates or
				insert and deletes. We will look at an example by creating a table, fragmenting the heck
				out of it and then doing a reorganize and rebuild on the index.</p>

				<p><strong>How to correct it:</strong>There are two ways to fix fragmentation, one is to
				reorganize the index and the other is to rebuild the index. Reorganize is an online
				operation while rebuild is not unless you specify ONLINE = ON, ONLINE = ON will only
				work on Enterprise editions of SQL Server.</p>

				<p>Here are two differences between REBUILD ONLINE = ON and REBUILD ONLINE = OFF</p>

				<p><strong>ON:</strong>Long-term table locks are not held for the duration of the index
				operation. During the main phase of the index operation, only an Intent Share (IS) lock
				is held on the source table. This allows queries or updates to the underlying table and
				indexes to continue. At the start of the operation, a Shared (S) lock is very briefly
				held on the source object. At the end of the operation, an S lock is very briefly held
				on the source if a nonclustered index is being created, or an SCH-M (Schema
				Modification) lock is acquired when a clustered index is created or dropped online, or
				when a clustered or nonclustered index is being rebuilt. ONLINE cannot be set to ON when
				an index is being created on a local temporary table.</p>

				<p><strong>OFF:</strong>Table locks are applied for the duration of the index operation.
				An offline index operation that creates, rebuilds, or drops a clustered, spatial, or XML
				index, or rebuilds or drops a nonclustered index, acquires a Schema modification (Sch-M)
				lock on the table. This prevents all user access to the underlying table for the
				duration of the operation. An offline index operation that creates a nonclustered index
				acquires a Shared (S) lock on the table. This prevents updates to the underlying table
				but allows read operations, such as SELECT statements.</p>

				<h1 class="firstHeading">Indexes for "Rebuild"</h1>

				<p>Report provide the list of indexes, which are required to perform operation "Rebuild".</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://technet.microsoft.com/en-us/library/ms189858.aspx" target="_blank">Reorganize and Rebuild Indexes</a></p>

				<p><a href="http://wiki.lessthandot.com/index.php/Finding_Fragmentation_Of_An_Index_And_Fixing_It" target="_blank">Finding Fragmentation Of An Index And Fixing It</a></p>

				<p><a href="http://blog.sqlauthority.com/2010/01/12/sql-server-fragmentation-detect-fragmentation-and-eliminate-fragmentation/" target="_blank">SQL SERVER – Fragmentation – Detect Fragmentation and Eliminate Fragmentation</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

