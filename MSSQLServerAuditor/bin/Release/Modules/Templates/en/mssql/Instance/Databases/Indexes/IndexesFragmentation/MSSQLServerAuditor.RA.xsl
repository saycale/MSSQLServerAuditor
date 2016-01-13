<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Fragmented Indexes" id="IndexesFragmented.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Fragmented Indexes" id="IndexesFragmented.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Fragmented Indexes</h1>

				The SQL Server Database Engine automatically maintains indexes whenever insert,
				update, or delete operations are made to the underlying data. Over time these
				modifications can cause the information in the index to become scattered in the
				database (fragmented). Fragmentation exists when indexes have pages in which the
				logical ordering, based on the key value, does not match the physical ordering
				inside the data file. Heavily fragmented indexes can degrade query performance and
				cause your application to respond slowly.

				You can remedy index fragmentation by reorganizing or rebuilding an index. For
				partitioned indexes built on a partition scheme, you can use either of these methods
				on a complete index or a single partition of an index. Rebuilding an index drops and
				re-creates the index. This removes fragmentation, reclaims disk space by compacting
				the pages based on the specified or existing fill factor setting, and reorders the
				index rows in contiguous pages. When ALL is specified, all indexes on the table are
				dropped and rebuilt in a single transaction. Reorganizing an index uses minimal
				system resources. It defragments the leaf level of clustered and nonclustered
				indexes on tables and views by physically reordering the leaf-level pages to match
				the logical, left to right, order of the leaf nodes. Reorganizing also compacts the
				index pages. Compaction is based on the existing fill factor value.

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="https://msdn.microsoft.com/library/ms189858.aspx" target="_blank">Reorganize and Rebuild Indexes</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
