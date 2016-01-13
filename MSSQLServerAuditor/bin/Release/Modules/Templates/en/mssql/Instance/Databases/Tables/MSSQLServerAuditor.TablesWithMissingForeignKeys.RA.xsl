<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Description" id="TablesWithMissingForeignKeys.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Description" id="TablesWithMissingForeignKeys.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Missing foreign key constraints</h1>

				<p>References are at the heart of a database. It is possible to create a beautiful
				database with perfectly working front end code that always, 100% of the time, does the
				right thing with your data. But, writing code is hard. Very hard! Your data is often the
				most important asset you own. You need to protect it with every bit of technology you
				can find. At the heart of protecting your data is referential integrity. What does this
				mean? It means that you shouldn't be missing data, ever!</p>

				<p>The code below will check for columns that have ID in the name of the column where
				that column is not part of a primary key or foreign key constraint. Often times, this
				represents a missing constraint, but not always. The code presented below exists to
				highlight potential problems. You must still determine if this potential problem is
				real, and then act accordingly.</p>

				<p><strong>How to correct it:</strong> Correcting this problem seems simple at first.
				Just declare your foreign keys, right? Well, it's not so simple. You see, there could be
				code running that deletes all the necessary data from the related tables. If you have
				code that deletes data in related tables in the wrong order, you will get referential
				constraint errors. Similar problems can occur with updates and inserts. The order in
				which you do things is important when you have referential constraints.</p>

				<p><strong>Level of difficulty:</strong> High</p>

				<p><strong>Level of severity:</strong> High</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DataDesign/missing-foreign-key-constraints" target="_blank">Missing foreign key constraints</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

