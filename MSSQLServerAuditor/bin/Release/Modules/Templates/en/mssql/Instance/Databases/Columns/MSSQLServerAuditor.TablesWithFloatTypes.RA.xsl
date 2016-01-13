<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Tables with float type columns" id="TablesWithFloatTypes.Description.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Tables with float type columns" id="TablesWithFloatTypes.Description.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>

				<h1 class="firstHeading">Do not use the float data type</h1>

				<p>That may seem a little harsh, and it's not always true. However, most of the time,
				the float data type should be avoided. Unfortunately, the float (and real) data types
				are approximate data types that can lead to significant rounding errors.</p>

				<h1 class="firstHeading">Query on Float DataType may return inconsistent result</h1>

				<p>Floating point data is approximate; therefore, not all values in the data type range
				can be represented exactly. When you add a small number with a very big number, the
				small number might just lost in the end result.</p>

				<p>As a end-user, we want consistent and correct result. However, float data type has no
				correct result since the value is not accurate. Query result on float data type is also
				not consistent as well. In the above example, we change the order of + and – operator,
				we will see different result. Suppose we want aggregate float values, such as
				sum(floatcolumn), SQL Server may choose to parallel scan the whole table with different
				thread, and sum up the result together. In this case, the order of data values are
				random, and the end-result will be random. In one of our customer’s case, he run the
				same query multiple times, and each time he get a totally different result.</p>

				<p>So, please try to avoid using float data type, especially you want to do some
				aggregation on float types. A workaround is to covert the type to a numeric type before
				doing aggregate. For example, support X is float</p>

				<p>you can use sum(cast(x as decimal(30,4))) to get consistent result</p>

				<p><strong>How to correct it:</strong> Examine the data you are using and identify the
				precision and scale required. Change the data type (or code) to use a decimal with the
				precision and scale you require.</p>

				<p><strong>Level of difficulty:</strong> Easy</p>

				<p><strong>Level of severity:</strong> Moderate</p>

				<p><a href="http://blogs.lessthandot.com/index.php/DataMgmt/DBProgramming/do-not-use-the-float-data-type" target="_blank">Do not use the float data type</a></p>

				<p><a href="http://blogs.msdn.com/b/qingsongyao/archive/2009/11/14/float-datatype-is-evil.aspx" target="_blank">Float datatype is evil</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

