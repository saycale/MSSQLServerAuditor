<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Привелигированные пользователи баз данных" id="DatabaseUsersMembersOfGroupsWithElevatedPermissions.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Привелигированные пользователи баз данных" id="DatabaseUsersMembersOfGroupsWithElevatedPermissions.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Привелигированные пользователи баз данных</h1>

				<p>В каждой базе данных есть особые, привелигированные роли баз данных,
				такие как <strong>db_owner</strong>, <strong>db_accessadmin</strong>,
				<strong>db_securityadmin</strong> и <strong>db_ddladmin</strong>.
				Пользователи этих ролей имеют больше полномочий, чем просто чтение и запись
				данных. Например, они могут удалить или переименовать таблицу, функцию или
				хранимую процедуру.</p>

				<p><strong>Как исправить:</strong> Внимательно пересмотрите схему
				безопасности баз данных, и, возможно, используйте специально созданных роли
				вместо привелигированного доступа.</p>

				<p><strong>Уровень сложности:</strong> Высокий</p>

				<p><strong>Уровень опасности:</strong> Высокий</p>

				<p><a href="http://msdn.microsoft.com/en-us/library/ms189121.aspx" target="_blank">Database-Level Roles</a></p>

				<p><a href="http://BrentOzar.com/go/elevated" target="_blank">User with Elevated Database Permissions</a></p>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

