<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Пользователи баз данных" id="DatabaseUsers.Description.HTML.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Пользователи баз данных" id="DatabaseUsers.Description.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<html>
			<head>
			</head>
			<body>
				<h1 class="firstHeading">Пользователи баз данных</h1>

				<p>Добавление и восстановление баз данных одного экземпляра сервера на другой является задачей, часто выполняемой администратором
				базы данных. После присоединения или восстановления базы данных, вход с ранее созданными и настроенныит логинами данной базы данных невозможен.
				Наиболее распространенный симптом этой проблемы – приложение выдает ошибки входа, либо при попытке добавить
				имя пользователя в базу данных, сообщает, что данный пользователь уже существует в текущей базе данных. Это часто происходит
				при добавлении или восстановлении базы данных, как решить эту проблему? </p>

				<p><strong>Как исправить:</strong> The stored procedure sp_change_users_login can be
				used.</p>

				<p><strong>Уровень сложности:</strong> Высокий</p>

				<p><strong>Уровень опасности:</strong> Высокий</p>

				<a href="http://www.mssqltips.com/sqlservertip/1590/understanding-and-dealing-with-orphaned-users-in-a-sql-server-database/"
				target="_blank">Поиск и решение проблемы осиротевших пользователей базы данных SQL Server</a>

			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

