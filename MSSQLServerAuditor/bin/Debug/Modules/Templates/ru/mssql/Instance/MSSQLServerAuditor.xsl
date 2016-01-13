<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:template match="/">
<html>
<body>
	<h2>Global information</h2>

	<table border="1">
		<tr bgcolor="#9acd32">
			<th>
				Instance name
			</th>
			<th>
				Version
			</th>
			<th>
				Type
			</th>
		</tr>
		<xsl:for-each select="Instance/Audit/Server">
			<tr>
				<td>
					<xsl:value-of select="ServerName"/>
				</td>
				<td>
					<xsl:value-of select="Edition"/>
				</td>
				<td>
					<xsl:value-of select="ProductVersion"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

	<h2>Instance's databases</h2>

	<h3>Databases count</h3>

	<table border="1">
		<tr bgcolor="#9acd32">
			<th>
				Count
			</th>
		</tr>

		<xsl:for-each select="Instance/DatabasesCount">
			<tr>
				<td>
					<xsl:value-of select="NumberOfDatabases"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

	<h3>Databases</h3>

	<table border="1">
		<tr bgcolor="#9acd32">
			<th>
				Name
			</th>
			<th>
				Type
			</th>
			<th>
				Owner
			</th>
		</tr>

		<xsl:for-each select="Instance/Database">
			<tr>
				<xsl:if test="position() mod 2 = 0">
					<xsl:attribute name="bgcolor">#CCCCCC</xsl:attribute>
				</xsl:if>
				<td>
					<xsl:value-of select="Name"/>
				</td>
				<td>
					<xsl:value-of select="CompatibilityLevel"/>
				</td>
				<td>
					<xsl:value-of select="Owner"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

</body>
</html>
</xsl:template>
</xsl:stylesheet>
