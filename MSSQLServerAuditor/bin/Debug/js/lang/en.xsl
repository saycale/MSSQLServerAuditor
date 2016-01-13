<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output encoding="UTF-8" method="html"/>

	<!-- myErrorTable -->
	<xsl:variable name="lang.myErrorTable.Instance"            select="'Instance'"/>
	<xsl:variable name="lang.myErrorTable.Query"               select="'Query'"/>
	<xsl:variable name="lang.myErrorTable.Hierarchy"           select="'Category'"/>
	<xsl:variable name="lang.myErrorTable.RecordSets"          select="'RecordSets'"/>
	<xsl:variable name="lang.myErrorTable.Numbers"             select="'#'"/>
	<xsl:variable name="lang.myErrorTable.Code"                select="'Code'"/>
	<xsl:variable name="lang.myErrorTable.Number"              select="'Number'"/>
	<xsl:variable name="lang.myErrorTable.Message"             select="'Message'"/>

	<!-- NetworkInformation -->
	<xsl:variable name="lang.NetworkInformation.Instance"      select="'Server'"/>
	<xsl:variable name="lang.NetworkInformation.IpAddress"     select="'IP Address'"/>
	<xsl:variable name="lang.NetworkInformation.NetworkStatus" select="'Network status'"/>
	<xsl:variable name="lang.NetworkInformation.TimeInMs"      select="'Round Time (ms)'"/>

</xsl:stylesheet>
