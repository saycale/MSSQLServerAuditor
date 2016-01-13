<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Foreign Keys Constraints Not Trusted" id="TablesWithForeignKeysNotTrusted.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Foreign Keys Constraints Not Trusted" id="TablesWithForeignKeysNotTrusted.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
			<h1 class="firstHeading">Foreign Keys or Check Constraints Not Trusted</h1>

				<p>If you need to load a lot of data quickly, you can disable keys and
				constraints in order to improve performance. After the data load finishes,
				enable them again, and SQL Server will check them behind the scenes. This
				technique works best in large data warehouse environments where entire
				dimension tables might be reloaded from scratch every night. Disabling
				constraints is usually safer and easier than dropping and recreating
				them.</p>

				<p>As long as you remember to enable them correctly again – and most people
				don't.</p>

				<p>This report checks sys.foreign_keys looking for indexes with
				is_not_trusted = 1.</p>

				<p>It turns out this can have a huge performance impact on queries, too,
				because SQL Server won't use untrusted constraints to build better execution
				plans.</p>

				<p><strong>How to fix the problem:</strong> You have to tell SQL Server to
				not just enable the constraint, but to recheck all of the data that’s been
				loaded. The word CHECK appears twice on purpose – this tells SQL Server that
				it needs to check the data, not just enable the constraint. The checking of
				the existing data can take time and burn a lot of IO throughput, so you may
				want to do this during maintenance windows if the table is large. Also, it
				might turn out that some of the data violates your constraints – but that’s
				a good thing to find out too. After this change, you may see improved query
				performance for tables with trusted keys and constraints.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Easy</p>

				<p><a href="http://www.brentozar.com/blitz/foreign-key-trusted/" target="_blank">Blitz Result: Foreign Keys or Check Constraints Not Trusted</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

