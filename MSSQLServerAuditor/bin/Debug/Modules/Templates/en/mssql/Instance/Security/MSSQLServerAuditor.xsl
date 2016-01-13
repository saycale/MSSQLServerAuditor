<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:template match="/">
<html>
<body>
	<h2>Общие сведения</h2>

	<table border="1">
		<tr bgcolor="#9acd32">
			<th>
				Название инстанса
			</th>
			<th>
				Версия
			</th>
			<th>
				Тип
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

	<h2>Базы данных инстанса</h2>

	<h3>Число баз данных</h3>

	<table border="1">
		<tr bgcolor="#9acd32">
			<th>
				Количество
			</th>
		</tr>

		<xsl:for-each select="Instance/Databases/DatabasesCount">
			<tr>
				<td>
					<xsl:value-of select="NumberOfDatabases"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

	<h3>Базы данных</h3>

	<table border="1" width="100%" style="font-size : 12px; font-family : Courier New">
		<tr bgcolor="#9acd32">
			<th>
				Имя
			</th>
			<th>
				Тип совместимости
			</th>
			<th>
				Владелец
			</th>
		</tr>

		<xsl:for-each select="Instance/Databases/CompatibilityLevel/Database">
			<tr>
				<xsl:if test="position() mod 2 = 0">
					<xsl:attribute name="bgcolor">#CCCCCC</xsl:attribute>
				</xsl:if>
				<td>
					<xsl:value-of select="Name"/>
				</td>
				<td align="right">
					<xsl:value-of select="CompatibilityLevel"/>
				</td>
				<td>
					<xsl:value-of select="Owner"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

	<table border="1" width="100%" style="font-size : 12px; font-family : Courier New">
		<tr bgcolor="#9acd32">
			<th>
				Имя
			</th>
			<th>
				Время создания
			</th>
			<th>
				Наличие доступа
			</th>
		</tr>

		<xsl:for-each select="Instance/Security/Logins/LoginsList">
			<tr>
				<xsl:if test="position() mod 2 = 0">
					<xsl:attribute name="bgcolor">#CCCCCC</xsl:attribute>
				</xsl:if>
				<td>
					<xsl:value-of select="LoginName"/>
				</td>
				<td align="right">
					<xsl:value-of select="CreateDateTime"/>
				</td>
				<td align="right">
					<xsl:value-of select="IsHasAccess"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

	<table border="1">
		<tr bgcolor="#9acd32">
			<th>
				Имя объекта
			</th>
			<th>
				Имя счётчика
			</th>
			<th>
				Значение
			</th>
		</tr>

		<xsl:for-each select="Instance/Audit/Memory/GetInstanceMemoryStatus">
			<tr>
				<xsl:if test="position() mod 2 = 0">
					<xsl:attribute name="bgcolor">#CCCCCC</xsl:attribute>
				</xsl:if>
				<td>
					<xsl:value-of select="object_name"/>
				</td>
				<td>
					<xsl:value-of select="counter_name"/>
				</td>
				<td align="right">
					<xsl:value-of select="cntr_value"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>

	<table border="1" width="100%" style="font-size : 12px; font-family : Courier New">
		<tr bgcolor="#9acd32">
			<th>
				Номер процесса
			</th>
			<th>
				Имя программы
			</th>
			<th>
				Команда
			</th>
			<th>
				Имя пользователя
			</th>
		</tr>

		<xsl:for-each select="Instance/Audit/Processes/GetInstanceProcesses">
			<tr>
				<xsl:if test="position() mod 2 = 0">
					<xsl:attribute name="bgcolor">#CCCCCC</xsl:attribute>
				</xsl:if>
				<td align="right">
					<xsl:value-of select="spid"/>
				</td>
				<td>
					<xsl:value-of select="program_name"/>
				</td>
				<td>
					<xsl:value-of select="cmd"/>
				</td>
				<td>
					<xsl:value-of select="loginame"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>
