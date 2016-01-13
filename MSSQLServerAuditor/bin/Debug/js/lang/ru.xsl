<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output encoding="UTF-8" method="html"/>

	<!-- myErrorTable -->
	<xsl:variable name="lang.myErrorTable.Instance"            select="'Зкземпляр'"/>
	<xsl:variable name="lang.myErrorTable.Query"               select="'Запрос'"/>
	<xsl:variable name="lang.myErrorTable.Hierarchy"           select="'Категория'"/>
	<xsl:variable name="lang.myErrorTable.RecordSets"          select="'Наборов'"/>
	<xsl:variable name="lang.myErrorTable.Numbers"             select="'#'"/>
	<xsl:variable name="lang.myErrorTable.Code"                select="'Код'"/>
	<xsl:variable name="lang.myErrorTable.Number"              select="'Номер'"/>
	<xsl:variable name="lang.myErrorTable.Message"             select="'Сообщение'"/>

	<!-- NetworkInformation -->
	<xsl:variable name="lang.NetworkInformation.Instance"      select="'Сервер'"/>
	<xsl:variable name="lang.NetworkInformation.IpAddress"     select="'IP Адрес'"/>
	<xsl:variable name="lang.NetworkInformation.NetworkStatus" select="'Сетевой статус'"/>
	<xsl:variable name="lang.NetworkInformation.TimeInMs"      select="'Время (ms)'"/>

</xsl:stylesheet>
