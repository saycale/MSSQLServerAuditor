<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables with problems in column names" id="TablesWithColumnNameProblems.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables with problems in column names" id="TablesWithColumnNameProblems.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Do not use spaces or other invalid characters in your column
				names</h1>

				<p>Column names (and table names) should not have spaces or any other invalid characters
				in them. This is considered bad practice because it requires you to use square brackets
				around your names. Square brackets make the code harder to read and understand. The
				query (presented below) will also highlight columns and tables with numbers in the
				names. Most of the time, when there is a number in a column name, it represents a
				de-normalized database. There are exceptions to this rule, so not all occurrences of
				this problem need to be fixed.</p>

				<p>It is recognized that some organizations allow (and may even encourage) the use of
				the underscore character. In the newly modified code below, you can include a list of
				acceptable symbols. The code below allows the underscore symbol and the
				<strong>$</strong> symbol. You may ask the utility support to modify this local variable
				to include any symbol that is acceptable within your organization.</p>

				<p><strong>How to correct it:</strong> If this is a number issue, you may need to
				redesign your database structure to include more tables. For example, if you have a
				StudentGrade table with (StudentId, Grade1, Grade2, Grade3, Grade4) you should change it
				to be StudentGrade with (StudentId, Grade, Identifier). Each student would have multiple
				rows in this table (one for each grade). You would need to add an identifier column to
				indicate what the grade is for (test on November 10, book report, etc).</p>

				<p><strong>Level of difficulty:</strong> Moderate</p>

				<p><strong>Level of severity:</strong> Mild</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/do-not-use-spaces-or-other-invalid-chara" target="_blank">Do not use spaces or other invalid characters in column name</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

